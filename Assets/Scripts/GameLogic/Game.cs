using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFifteen.GameLogic
{
    public class Game : IGame
    {
        public bool IsSolving { get; set; }

        public int Moves { get => _moves; }

        public int Size { get => _size; }

        private int _size;

        private Map _map;

        private Vector2Int _space;

        private Stack<Vector2Int> _movesHistory;

        private int _moves;
        public Game(int size)
        {
            _size = size;
            _map = new Map(size);
            _movesHistory = new Stack<Vector2Int>();
        }

        public void Start()
        {
            _moves = 0;
            IsSolving = false;
            _space = new Vector2Int(_size - 1, _size - 1);
        }

        public void Finish()
        {
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    Chip chip = GetAt(i, j);

                    if (chip != null)
                    {
                        UnityEngine.Object.Destroy(chip.gameObject);
                    }

                    SetAt(i, j, null);
                }
            }

            _movesHistory.Clear();
        }

        public void SetAt(int i, int j, Chip chip)
        {
            _map.Set(new Vector2Int(i, j), chip);
        }

        public void PressAt(Vector2Int pos, float duration)
        {
            if (!_map.IsAnyChipMoving())
            {
                _moves += Mathf.Abs(pos.x - _space.x) + Mathf.Abs(pos.y - _space.y);

                MoveChip(pos, duration);
            }
        }

        public bool IsOnOneLine(Vector2Int pos)
        {
            return Mathf.Abs(pos.x - _space.x) == 0 || Mathf.Abs(pos.y - _space.y) == 0;
        }
        public Chip GetAt(int i, int j)
        {
            return GetAt(new Vector2Int(i, j));
        }
        public Chip GetAt(Vector2Int pos)
        {
            return _map.Get(pos);
        }

        public IEnumerator Solve(float duration)
        {
            IsSolving = true;
            while (_movesHistory.Count > 0)
            {
                if (!_map.IsAnyChipMoving())
                {
                    Vector2Int pos = _movesHistory.Pop();
                    MoveChip(pos, duration);
                }

                yield return new WaitForSeconds(duration);
            }
        }
        public IEnumerator Shuffle(int seed, float duration)
        {
            System.Random rnd = new System.Random(seed + DateTime.Now.Second);
            for (int j = 0; j < seed; ++j)
            {
                if (!_map.IsAnyChipMoving())
                {
                    MoveChip(rnd.Next(_size), rnd.Next(_size), duration);
                }

                yield return new WaitForSeconds(duration);
            }
        }

        public bool IsSolved()
        {
            if (!_space.Equals(new Vector2Int(_size - 1, _size - 1)))
            {
                return false;
            }

            int digit = 0;
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    if (_map.Get(i, j) && _map.Get(i, j).ID != ++digit)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void MoveChip(int i, int j, float duration)
        {
            MoveChip(new Vector2Int(i, j), duration);
        }
        private void MoveChip(Vector2Int pos, float duration)
        {
            if (_space.Equals(pos))
            {
                return ;
            }

            int i = 1;
            while (pos.x != _space.x)
            {
                Shift(Math.Sign(pos.x - _space.x), 0, duration * i);
                ++i;
            }

            while (pos.y != _space.y)
            {
                Shift(0, Math.Sign(pos.y - _space.y), duration * i);
                ++i;
            }
        }
        private void Shift(int i, int j, float duration)
        {
            if (!IsSolving)
            {
                _movesHistory.Push(_space);
            }

            Vector2Int newPos = _space + new Vector2Int(i, j);
            _map.Copy(_space, newPos, duration);
            _space = newPos;
        }
    }
}
