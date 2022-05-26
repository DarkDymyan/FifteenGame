using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace GameFifteen.UI
{
    public class PopUpText : PopUp
    {
        public Text textTime { get => _textTime; }

        public Text textMoves { get => _textMoves; }

#pragma warning disable 0649
        [SerializeField, NotNull]
        private Text _textTime;

        [SerializeField, NotNull]
        private Text _textMoves;
#pragma warning restore 0649
    }
}
