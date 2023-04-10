using Photon.Pun;
using UnityEngine;

namespace Game.Controllers
{
    public class BulletView : MonoBehaviourPun
    {
        [SerializeField] private Rigidbody2D rbody;

        private float speed;
        private Vector3 moveDirection;

        public void Shoot(Vector2 moveDirection, float speed = 8f)
        {
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
            if (!photonView.IsMine) return;
            if (collision.gameObject.CompareTag(Tags.Bullet)) return;

            if (collision.gameObject.CompareTag(Tags.Obstacle))
            {
                photonView.RPC("Destroy", RpcTarget.All);
                return;
            }

            var player = collision.gameObject.GetComponent<PlayerView>();
            if (player != null && player.photonView.Owner != photonView.Owner)
            {
                player.photonView.RPC("Damage", RpcTarget.All);
                photonView.RPC("Destroy", RpcTarget.All);
            }
        }

        [PunRPC]
        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}