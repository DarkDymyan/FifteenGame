using System.Collections;
using UnityEngine;

namespace GameFifteen.GameLogic
{
    public interface IGame
    {
        public bool IsSolving { get; set; }
        public int Size { get; }
        public int Moves { get; }
        public void Start();
        public void PressAt(Vector2Int pos, float duration);
        public void SetAt(int i, int j, Chip chip);
        public Chip GetAt(int i, int j);
        public Chip GetAt(Vector2Int pos);
        public bool IsSolved();
        public IEnumerator Shuffle(int seed, float duration);
        public IEnumerator Solve(float duration);
        public bool IsOnOneLine(Vector2Int pos);
        public void Finish();
    }
}
