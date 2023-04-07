using UnityEngine;
using UnityEngine.UI;

namespace Game.Controllers
{
    public class BulletController : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rbody;

        private PlayerController owner;
        private float speed;
        private Vector3 moveDirection;

        public void Shoot(PlayerController owner, Vector2 moveDirection, float speed = 4f)
        {
            this.owner = owner;
            this.speed = speed;
            this.moveDirection = moveDirection.normalized;

            transform.up = moveDirection;
        }

        private void FixedUpdate()
        {
            rbody.MovePosition(transform.position + speed * Time.fixedDeltaTime * moveDirection);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null && player != owner)
            {
                player.Damage();
            }
        }
    }
}