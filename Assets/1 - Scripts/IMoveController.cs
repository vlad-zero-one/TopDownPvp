using UnityEngine;

namespace Game.Controllers
{
    public interface IMoveController
    {
        public void Init();
        public delegate void MoveEventHandler(Vector2 direction);
        public event MoveEventHandler MoveDirective;
    }
}