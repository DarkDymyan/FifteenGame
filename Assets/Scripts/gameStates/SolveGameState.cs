using GameFifteen.GameLogic;
using System.Collections;
using UnityEngine;

namespace GameFifteen.gameStates
{
    public class SolveGameState : GameState
    {
        private readonly IGame _game;

        private readonly GameGontroller _gameGontroller;

        private readonly IGameStatesSwitch _gameStatesSwitch;

        public override GameStateId GameStateId => GameStateId.Solve;

        private Coroutine _coroutine;

        public SolveGameState(GameGontroller gameGontroller, IGame game, IGameStatesSwitch gameStatesSwitch)
        {
            _gameGontroller = gameGontroller;
            _game = game;
            _gameStatesSwitch = gameStatesSwitch;
        }
        
        public override void Activate()
        {
            _gameGontroller.StopAllCoroutines();
            _coroutine = _gameGontroller.StartCoroutine(DoSolve());
        }

        private IEnumerator DoSolve()
        {
            yield return _game.Solve(_gameGontroller.solveDuration);

            _gameStatesSwitch.SetState(GameStateId.Win);
        }

        public override void Deactivate()
        {
            _gameGontroller.StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }
}