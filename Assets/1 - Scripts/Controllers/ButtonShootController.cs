using UnityEngine;
using UnityEngine.UI;

namespace Game.Controllers
{
    public class ButtonShootController : MonoBehaviour, IShootController
    {
        [SerializeField] private Button shootButton;

        public event IShootController.ShootEventHandler ShootDirective;

        public void Init()
        {
            shootButton.onClick.AddListener(Shoot);
        }

        private void Shoot()
        {
            ShootDirective?.Invoke();
        }
#if UNITY_EDITOR || PLATFORM_STANDALONE_WIN
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ShootDirective?.Invoke();
            }
        }
#endif
        private void OnDestroy()
        {
            shootButton.onClick.RemoveListener(Shoot);
        }
    }
}