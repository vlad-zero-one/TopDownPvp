using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Controllers
{
    public class KeyboardShootController : MonoBehaviour, IShootController
    {
        public event IShootController.ShootEventHandler ShootDirective;

        public void Init()
        {
            
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ShootDirective?.Invoke();
            }
        }
    }
}