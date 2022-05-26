using GameFifteen.GameLogic;
using System.Collections;
using UnityEngine;

namespace GameFifteen.gameStates
{
    public class ShuffleChipsState : GameState
    {
        private readonly IGame _game;

        private readonly GameGontroller _gameGontroller;

        private readonly IGameStatesSwitch _gameStatesSwitch;

        public override GameStateId GameStateId => GameStateId.Shuffle;

        private Coroutine _coroutine;

        public ShuffleChipsState(GameGontroller gameGontroller, IGame game, IGameStatesSwitch gameStatesSwitch)
        {
            _gameGontroller = gameGontroller;
            _game = game;
            _gameStatesSwitch = gameStatesSwitch;
        }
        
        public override void Activate()
        {
            _coroutine = _gameGontroller.StartCoroutine(DoShuffle());
        }

        private IEnumerator DoShuffle()
        {
            if (_gameGontroller.seed > 0)
            {
                yield return _game.Shuffle(_gameGontroller.seed, _gameGontroller.shuffleDuration);
            }
            
            _gameStatesSwitch.SetState(GameStateId.Play);
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