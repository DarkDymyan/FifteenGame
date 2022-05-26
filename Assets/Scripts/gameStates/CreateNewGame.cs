using GameFifteen.Events;
using GameFifteen.GameLogic;
using System.Collections;
using UnityEngine;

namespace GameFifteen.gameStates
{
    public class CreateNewGame : GameState
    {
        private readonly IGame _game;

        private readonly GameGontroller _gameGontroller;

        private readonly GameObject _prototype;

        private readonly Sprite[] _Textures;

        private readonly Transform _parent;

        private readonly CheckGameCompleteEvent _completeCheck;

        private readonly IGameStatesSwitch _gameStatesSwitch;
        public override GameStateId GameStateId => GameStateId.CreateGame;

        private Coroutine _coroutine;

        public CreateNewGame(GameGontroller gameGontroller, IGame game, GameObject prototype, Sprite[] textures, Transform parent, CheckGameCompleteEvent completeCheck, IGameStatesSwitch gameStatesSwitch)
        {
            _game = game;
            _gameGontroller = gameGontroller;
            _prototype = prototype;
            _Textures = textures;
            _parent = parent;
            _completeCheck = completeCheck;
            _gameStatesSwitch = gameStatesSwitch;
        }
        
        public override void Activate()
        {
            _game.Start();
            _gameGontroller.Setup();

            _coroutine = _gameGontroller.StartCoroutine(DoCreate());
        }

        private IEnumerator DoCreate()
        {
            int digit = 0;

            for (int i = 0; i < _game.Size; i++)
            {
                for (int j = 0; j < _game.Size; j++)
                {
                    Chip chip = UnityEngine.Object.Instantiate(_prototype).GetComponent<Chip>();
                    chip.ID = ++digit;
                    chip.transform.SetParent(_parent);
                    Sprite sprite = (_Textures.Length > chip.ID - 1) ? _Textures[chip.ID - 1] : null;
                    chip.Setup(sprite, _completeCheck);

                    _game.SetAt(i, j, chip);

                    yield return null;
                }
            }

            yield return null;

            _gameStatesSwitch.SetState(GameStateId.Shuffle);
        }


        public override void Deactivate()
        {
            if (_coroutine != null)
            {
                _gameGontroller.StopCoroutine(_coroutine);
            }

            _coroutine = null;
        }
    }
}