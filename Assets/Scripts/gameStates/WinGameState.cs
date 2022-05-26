using GameFifteen.GameLogic;
using GameFifteen.UI;
using UnityEngine;
using UnityEngine.UI;

namespace GameFifteen.gameStates
{
    public class WinGameState : GameState
    {
        private readonly IGame _game;

        private readonly GameGontroller _gameGontroller;

        private readonly Transform _uiRoot;

        private readonly GameObject _popupPrefab;

        private readonly Text _timeText;

        private readonly IGameStatesSwitch _gameStatesSwitch;

        public override GameStateId GameStateId => GameStateId.Win;

        private PopUpText _popup;

        public WinGameState(GameGontroller gameGontroller, IGame game, Transform uiRoot, GameObject popupPrefab, Text timeText, IGameStatesSwitch gameStatesSwitch)
        {
            _gameGontroller = gameGontroller;
            _game = game;
            _uiRoot = uiRoot;
            _popupPrefab = popupPrefab;
            _timeText = timeText;
            _gameStatesSwitch = gameStatesSwitch;
        }
        
        public override void Activate()
        {
            var gameObject = UnityEngine.Object.Instantiate(_popupPrefab, _uiRoot);

            _popup = gameObject.GetComponent<PopUpText>();

            _popup.textTime.text = _timeText.text;
            _popup.textMoves.text = _game.Moves.ToString();
            _popup.Button.onClick.AddListener(OnClickButton);

            Finish();
        }

        public void Finish()
        {
            _gameGontroller.gamePlaying = false;

            _game.Finish();
        }

        private void OnClickButton()
        {
            _gameStatesSwitch.SetState(GameStateId.NewGamePopUp);
        }

        public override void Deactivate()
        {
            _popup.Button.onClick.RemoveListener(OnClickButton);

            UnityEngine.Object.Destroy(_popup.gameObject);

            _popup = null;
        }
    }
}