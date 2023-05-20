using Game.UI.Abstract;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ButtonShootController : MonoBehaviour, IShootController
    {
        [SerializeField] private Button shootButton;

        private float shootCooldown;

        public event IShootController.ShootEventHandler ShootDirective;

        public void SetCooldown(float shootCooldown)
        {
            gameObject.SetActive(true);

            this.shootCooldown = shootCooldown;

            shootButton.onClick.AddListener(Shoot);
        }

        private void Shoot()
        {
            ShootDirective?.Invoke();
            StartCoroutine(Cooldown());
        }

        private IEnumerator Cooldown()
        {
            shootButton.enabled = false;
            yield return new WaitForSeconds(shootCooldown);
            shootButton.enabled = true;
        }

        private void OnDestroy()
        {
            shootButton.onClick.RemoveListener(Shoot);
        }
    }
}