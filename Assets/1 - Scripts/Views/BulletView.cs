using Game.Model;
using Game.Static;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Game.Views
{
    public class BulletView : MonoBehaviourPun
    {
        [SerializeField] private Rigidbody2D rbody;
        [SerializeField] private SpriteRenderer spriteRenderer;

        public Player Owner { get; private set; }

        public delegate void HitEventArgs(BulletView bulletView);
        public event HitEventArgs Hit;

        private Bullet bullet;

        public void Init(Player owner, Vector3 direction, float speed, float lag)
        {
            Owner = owner;

            transform.up = direction;

            rbody.velocity = direction.normalized * speed;
            rbody.position += rbody.velocity * lag;
        }

        public void NewInit(Bullet bullet, Sprite sprite)
        {
            Owner = bullet.Owner;
            this.bullet = bullet;

            transform.position = bullet.Position;
            transform.up = bullet.Direction;

            // TODO: sprites
            //spriteRenderer.sprite = sprite;
        }

        public void StartMove()
        {
            rbody.velocity = bullet.Direction.normalized * bullet.Speed;
            rbody.position += rbody.velocity * bullet.Lag;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(Tags.Bullet)) return;

            if (collision.gameObject.CompareTag(Tags.Obstacle))
            {
                Hit?.Invoke(this);
                return;
            }

            var player = collision.gameObject.GetComponent<PlayerView>();
            if (player != null && player.photonView.Owner != Owner)
            {
                Hit?.Invoke(this);
            }
        }
    }
}