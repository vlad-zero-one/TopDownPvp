using Game.UI.Abstract;
using System.Collections;
using UnityEngine;

namespace Game.UI
{
    public class KeyboardController : MonoBehaviour, IMoveController, IShootController
    {
        private float shootCooldown;
        private bool shootEnabled = true;
        private bool moving;

        private Vector2 moveDirection = Vector2.zero;

        public event IMoveController.MoveEventHandler MoveDirective;
        public event IMoveController.StopEventHandler StopDirective;

        public event IShootController.ShootEventHandler ShootDirective;

        public void Init()
        {
            gameObject.SetActive(true);
        }

        public void SetCooldown(float shootCooldown)
        {
            gameObject.SetActive(true);

            this.shootCooldown = shootCooldown;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && shootEnabled)
            {
                ShootDirective?.Invoke();
                StartCoroutine(Cooldown());
            }

            if (Input.GetKey(KeyCode.A))
            {
                moveDirection.x = -1;
            }
            if (Input.GetKey(KeyCode.D))
            {
                moveDirection.x = 1;
            }
            if (Input.GetKey(KeyCode.W))
            {
                moveDirection.y = 1;
            }
            if (Input.GetKey(KeyCode.S))
            {
                moveDirection.y = -1;
            }

            if (Input.GetKeyUp(KeyCode.A) && moveDirection.x < 0)
            {
                moveDirection.x = 0;
            }
            if (Input.GetKeyUp(KeyCode.D) && moveDirection.x > 0)
            {
                moveDirection.x = 0;
            }
            if (Input.GetKeyUp(KeyCode.W) && moveDirection.y > 0)
            {
                moveDirection.y = 0;
            }
            if (Input.GetKeyUp(KeyCode.S) && moveDirection.y < 0)
            {
                moveDirection.y = 0;
            }

            if (moveDirection != Vector2.zero)
            {
                moving = true;
                MoveDirective?.Invoke(moveDirection.normalized);
            }
            else if (moving)
            {
                moving = false;
                moveDirection = Vector2.zero;
                StopDirective?.Invoke();
            }
        }

        private IEnumerator Cooldown()
        {
            shootEnabled = false;
            yield return new WaitForSeconds(shootCooldown);
            shootEnabled = true;
        }
    }
}