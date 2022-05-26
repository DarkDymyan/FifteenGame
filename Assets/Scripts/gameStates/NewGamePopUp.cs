using GameFifteen.UI;
using UnityEngine;

namespace GameFifteen.gameStates
{
    public class NewGamePopUp : GameState
    {
        private readonly Transform _uiRoot;
        
        private readonly GameObject _popupPrefab;

        private readonly IGameStatesSwitch _gameStatesSwitch;
        public override GameStateId GameStateId => GameStateId.NewGamePopUp;

        private PopUp _popup;

        public NewGamePopUp(Transform uiRoot, GameObject popupPrefab, IGameStatesSwitch gameStatesSwitch)
        {
            _uiRoot = uiRoot;
            _popupPrefab = popupPrefab;
            _gameStatesSwitch = gameStatesSwitch;
        }
        
        public override void Activate()
        {
            var gameObject = UnityEngine.Object.Instantiate(_popupPrefab, _uiRoot);

            _popup = gameObject.GetComponent<PopUp>();

            _popup.Button.onClick.AddListener(OnClickButton);
        }

        private void OnClickButton()
        {
            _gameStatesSwitch.SetState(GameStateId.CreateGame);
        }

        public override void Deactivate()
        {
            _popup.Button.onClick.RemoveListener(OnClickButton);

            UnityEngine.Object.Destroy(_popup.gameObject);

            _popup = null;
        }
    }
}