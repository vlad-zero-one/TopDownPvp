using System.Collections;
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
            StartCoroutine(Cooldown());
        }

#if UNITY_EDITOR || PLATFORM_STANDALONE_WIN
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ShootDirective?.Invoke();
                StartCoroutine(Cooldown());
            }
        }
#endif

        private IEnumerator Cooldown()
        {
            shootButton.enabled = false;
            yield return new WaitForSeconds(1f);
            shootButton.enabled = true;
        }

        private void OnDestroy()
        {
            shootButton.onClick.RemoveListener(Shoot);
        }
    }
}