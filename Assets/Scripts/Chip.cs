using GameFifteen.Events;
using JetBrains.Annotations;
using System;
using System.Collections;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace GameFifteen
{
	[RequireComponent(typeof(SpriteRenderer)), RequireComponent(typeof(Animation))]
	public class Chip : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, INotifyPositionChanged
	{
		[SerializeField, ReadOnly]
		private const string animName = "OverObject";
		public int ID
		{
			get => _ID;
			set =>_ID = value;
		}

		public bool isMoving { get; set; }

		public event EventHandler<PositionEventArgs> PointerClick;

		public CheckGameCompleteEvent _completeCheck;

		[SerializeField, ReadOnly, NotNull]
		private SpriteRenderer _Sprite;

		[SerializeField, NotNull]
		private Animation _overAnimation;

		private int _ID;
		private Vector2Int _Position;

		void Awake()
		{
			_Sprite = GetComponent<SpriteRenderer>();
			Assert.IsNotNull(_Sprite, "Sprite must be not null!");

			_overAnimation = GetComponent<Animation>();
			Assert.IsNotNull(_overAnimation, "Animator must be not null!");

			isMoving = false;
		}

		public void Setup(Sprite sprite, CheckGameCompleteEvent gameCompleteCheck)
		{
			_Sprite.sprite = sprite;
			_completeCheck = gameCompleteCheck;
		}

		public void SetPos(Vector2Int pos)
		{
			_Position.x = pos.x;
			_Position.y = pos.y;
		}

		public void MoveTo(Vector2Int pos,Vector3 screenPos, float duration)
		{
			StopAllCoroutines();
			SetPos(pos);
			StartCoroutine(MoveChip(screenPos, duration));
		}
		public void OnPointerClick(PointerEventData eventData)
		{
			if (PointerClick != null)
			{
				PointerClick(this, new PositionEventArgs(_Position));
			}
		}

		private IEnumerator MoveChip(Vector3 targetPosition, float duration)
		{
			float timeElapsed = 0;
			isMoving = true;
			Vector3 startPosition = transform.position;
			while (timeElapsed < duration)
			{
				transform.position = Vector3.Lerp(startPosition, targetPosition, timeElapsed / duration);
				timeElapsed += Time.deltaTime;
				yield return null;
			}

			isMoving = false;
			transform.position = targetPosition;
			_completeCheck?.Invoke();
		}

        public void OnPointerExit(PointerEventData eventData)
		{
			_overAnimation.wrapMode = WrapMode.Default;
		}

        public void OnPointerEnter(PointerEventData eventData)
        {
			_overAnimation.wrapMode = WrapMode.Loop;
			_overAnimation.Play(animName);
		}
    }
}