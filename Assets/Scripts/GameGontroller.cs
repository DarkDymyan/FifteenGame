using UnityEngine;
using GameFifteen.GameLogic;
using System;
using UnityEngine.UI;
using UnityEngine.Assertions;
using GameFifteen.gameStates;
using GameFifteen.Events;
using JetBrains.Annotations;

namespace GameFifteen
{
    public class GameGontroller : MonoBehaviour
    {
        const int size = 4;

        public int seed { get => _seed; }
        public float duration { get => _duration; }
        public float shuffleDuration { get => _shuffleDuration; }
        public float solveDuration { get => _solveDuration; }
        public bool gamePlaying { get => _gamePlaying; set => _gamePlaying = value; }

        [SerializeField]
        private float _duration = 0.25f;

        [SerializeField]
        private float _shuffleDuration = 0.01f;

        [SerializeField]
        private float _solveDuration = 0.125f;

        [SerializeField]
        private int _seed = 200;

        [SerializeField]
        private GameObject _prototype = null;

        [SerializeField, NotNull]
        private Sprite[] _Textures = null;

        [SerializeField]
        private Transform _parent = null;

        [SerializeField]
        private Text timerText = null;

#pragma warning disable 0649
        [SerializeField, NotNull]
        private Transform _uiRoot;

        [SerializeField, NotNull]
        private GameObject _startNewGamePopupPrefab;

        [SerializeField, NotNull]
        private GameObject _winGamePopupPrefab;
#pragma warning restore 0649

        private GameStatesManager _gameStatesManager = null;

        private Game _game;

        private DateTime _time;

        private bool _gamePlaying;

        private void Awake()
        {
            Assert.IsNotNull(_prototype, "Prototype must be not null!");
        }

        void Start()
        {
            _game = new Game(size);
            _gameStatesManager = new GameStatesManager();

            _gamePlaying = false;

            var completeCheck = new CheckGameCompleteEvent();

            _gameStatesManager.AddState(GameStateId.NewGamePopUp, new NewGamePopUp(_uiRoot, _startNewGamePopupPrefab, _gameStatesManager));

            _gameStatesManager.AddState(GameStateId.CreateGame, new CreateNewGame(this, _game, _prototype, _Textures, _parent, completeCheck, _gameStatesManager));

            _gameStatesManager.AddState(GameStateId.Shuffle, new ShuffleChipsState(this, _game, _gameStatesManager));

            _gameStatesManager.AddState(GameStateId.Play, new PlayGameState(size, this, _game, completeCheck, _gameStatesManager));

            _gameStatesManager.AddState(GameStateId.Solve, new SolveGameState(this, _game, _gameStatesManager));

            _gameStatesManager.AddState(GameStateId.Win, new WinGameState(this, _game, _uiRoot, _winGamePopupPrefab, timerText, _gameStatesManager));

            _gameStatesManager.SetState(GameStateId.NewGamePopUp);
        }

        public void Setup()
        {
            timerText.text = "00:00:00";
        }

        public void Solve()
        {
            _gameStatesManager.SetState(GameStateId.Solve);
        }
        public void QuitApplication()
        {
            Application.Quit();
        }
        public void TryStartGame()
        {
            if (!_gamePlaying)
            {
                _time = DateTime.UtcNow;
                _gamePlaying = true;
            }
        }
        private void Update()
        {
            if (timerText != null && _gamePlaying && !_game.IsSolving)
            {
                timerText.text = toStr((DateTime.UtcNow - _time));
            }
        }

        private string toStr(TimeSpan t)
        {
            return t.Hours.ToString("00") + ":" + t.Minutes.ToString("00") + ":" + t.Seconds.ToString("00");
        }
    }
}