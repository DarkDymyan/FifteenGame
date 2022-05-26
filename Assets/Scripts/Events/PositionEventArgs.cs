using System;
using UnityEngine;

namespace GameFifteen.Events
{
    public class PositionEventArgs : EventArgs
    {
        public Vector2Int Position;

        public PositionEventArgs(Vector2Int pos)
        {
            Position = pos;
        }
    }

    public interface INotifyPositionChanged
    {
        event EventHandler<PositionEventArgs> PointerClick;
    }
}
