using UnityEngine;

namespace GameFifteen.GameLogic
{
    public struct Map
    {
        int _size;
        Chip[,] _map;

        public Map(int size)
        {
            _size = size;
            _map = new Chip[_size, _size];
        }

        public bool IsAnyChipMoving()
        {
            foreach (Chip chip in _map)
            {
                if (chip && chip.isMoving)
                {
                    return true;
                }
            }

            return false;
        }
        public bool IsOnBoard(Vector2Int pos)
        {
            return IsOnBoard(pos.x, pos.y);
        }
        public bool IsOnBoard(int i, int j)
        {
            if (i < 0 || i > _size - 1)
            {
                return false;
            }

            if (j < 0 || j > _size - 1)
            {
                return false;
            }

            return true;
        }

        public void Set(Vector2Int pos, Chip chip)
        {
            if (IsOnBoard(pos))
            {
                if (chip)
                {
                    chip.SetPos(pos);
                }

                _map[pos.x, pos.y] = chip;
            }
        }

        public Chip Get(int i, int j)
        {
            if (IsOnBoard(i, j))
            {
                return _map[i, j];
            }

            return null;
        }
        public Chip Get(Vector2Int pos)
        {
            if (IsOnBoard(pos))
            {
                return _map[pos.x, pos.y];
            }

            return null;
        }

        internal void Copy(Vector2Int newPos, Vector2Int oldPos, float duration)
        {
            Chip chip = Get(oldPos);
            Chip space = Get(newPos);
            Vector3 screenPos = chip.transform.position;
            chip.MoveTo(newPos, space.transform.position, duration);
            space.MoveTo(oldPos, screenPos, 0);
            Set(newPos, chip);
            Set(oldPos, space);
        }
    }
}
