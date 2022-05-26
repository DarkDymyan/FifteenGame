using GameFifteen.Events;
using GameFifteen.GameLogic;
using JetBrains.Annotations;

namespace GameFifteen.gameStates
{
    public class PlayGameState : GameState
    {
        private readonly int _size;

        private readonly IGame _game;

        private readonly GameGontroller _gameGontroller;

        private readonly IGameStatesSwitch _gameStatesSwitch;

        private readonly CheckGameCompleteEvent _completeCheck;
        public override GameStateId GameStateId => GameStateId.Play;

        public PlayGameState(int size, GameGontroller gameGontroller, IGame game, CheckGameCompleteEvent completeCheck, IGameStatesSwitch gameStatesSwitch)
        {
            _size = size;
            _gameGontroller = gameGontroller;
            _game = game;
            _completeCheck = completeCheck;
            _gameStatesSwitch = gameStatesSwitch;
        }
        
        public override void Activate()
        {
            _completeCheck.AddListener(OnGameCheck);

            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    Chip chip = _game.GetAt(i, j);
                    if (chip != null)
                    {
                        chip.PointerClick += OnClick;
                    }

                }
            }
        }
        public void OnClick(object sender, [NotNull] PositionEventArgs e)
        {
            if (_game.IsSolved())
            {
                _gameStatesSwitch.SetState(GameStateId.Win);
                return;
            }

            if (!_game.IsOnOneLine(e.Position) || _game.IsSolving)
            {
                return;
            }

            _gameGontroller.TryStartGame();

            _game.PressAt(e.Position, _gameGontroller.duration);
        }

        private void OnGameCheck()
        {
            if (_game.IsSolved())
            {
                _gameStatesSwitch.SetState(GameStateId.Win);
            }
        }

        public override void Deactivate()
        {
            _completeCheck.RemoveListener(OnGameCheck);

            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    Chip chip = _game.GetAt(i, j);
                    if (chip != null)
                    {
                        chip.PointerClick -= OnClick;
                    }
                }
            }
        }
    }
}