using UnityEngine;

namespace GameFifteen.UI
{
    public class MenuAnimation : MonoBehaviour
    {
        [SerializeField]
        private Animator m_Animator;

        [SerializeField]
        private bool toOpen;

        [SerializeField]
        private string nameProperty = "toOpen";

        void Start()
        {
            m_Animator = gameObject.GetComponent<Animator>();
        }

        public void State()
        {
            toOpen = m_Animator.GetBool(nameProperty);
            m_Animator.SetBool(nameProperty, !toOpen);
        }
    }
}
