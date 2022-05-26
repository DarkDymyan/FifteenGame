using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace GameFifteen.UI
{
    public class PopUp : MonoBehaviour
    {
        public Button Button { get => _button; }

#pragma warning disable 0649
        [SerializeField, NotNull]
        private Button _button;
#pragma warning restore 0649
    }
}
