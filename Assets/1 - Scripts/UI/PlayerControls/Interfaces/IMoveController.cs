using UnityEngine;

namespace Game.UI.Abstract
{
    public interface IMoveController
    {
        public void Init();
        public delegate void MoveEventHandler(Vector2 direction);
        public delegate void StopEventHandler();
        public event MoveEventHandler MoveDirective;
        public event StopEventHandler StopDirective;
    }
}