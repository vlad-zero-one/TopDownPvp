using DependencyInjection;
using Game.Configs;
using Game.Controllers;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Views
{
    public class PlayerView : MonoBehaviourPun, IPunInstantiateMagicCallback
    {
        [SerializeField] private Rigidbody2D rbody;
        [SerializeField] private Canvas playerCanvas;
        [SerializeField] private Text nickName;

        [SerializeField] private BulletView bulletPrefab;

        private PlayerSettings playerSettings;
        private float speed;

        private Vector3 moveDirection;

        private bool moving;

        private int hp;

        public delegate void DieEventHandler(PlayerView sender);
        public event DieEventHandler Die;

        public int Coins { get; private set; }

        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            var skinName = (string) info.photonView.InstantiationData[0];

            var appearanceData = DI.Get<PlayerAppearanceData>();
            playerSettings = DI.Get<PlayerSettings>();

            speed = playerSettings.PlayerSpeed;
            hp = playerSettings.PlayerHealth;

            if (photonView.IsMine)
            {
                nickName.color = appearanceData.PlayerNameColor;
                nickName.text = appearanceData.PlayerNameReplacement;
            }
            else
            {
                nickName.color = appearanceData.EnemyNameColor;
                nickName.text = photonView.Owner.NickName;
            }

            Instantiate(appearanceData.GetSkin(skinName), transform).transform.SetSiblingIndex(0);

            if (!photonView.IsMine)
            {
                DI.Get<GameController>().AddOtherPlayer(this);
            }
        }

        [PunRPC]
        private void Shoot(float positionX, float positionY, float directionX, float directionY, PhotonMessageInfo info)
        {
            var lag = (float)(PhotonNetwork.Time - info.SentServerTime);

            var bullet = Instantiate(bulletPrefab, new(positionX, positionY), Quaternion.identity);
            bullet.Init(photonView.Owner, 
                new(directionX, directionY),
                playerSettings.BulletSpeed,
                Mathf.Abs(lag));
        }

        public void StartMove(Vector2 moveDirection)
        {
            this.moveDirection = moveDirection;
            moving = true;
        }

        public void StopMove()
        {
            moving = false;
        }

        [PunRPC]
        private void Damage()
        {
            nickName.text = $"{--hp}";

            if (hp <= 0)
            {
                Die?.Invoke(this);
            }
        }

        public void AddCoint()
        {
            Coins++;
        }

        private void FixedUpdate()
        {
            if (moving)
            {
                var lastPos = transform.position;
                var pos = lastPos + speed * Time.fixedDeltaTime * moveDirection;
                rbody.MovePosition(pos);

                if (moveDirection.normalized != Vector3.down)
                {
                    transform.up = moveDirection;
                }
            }
        }

        private void Update()
        {
            if (playerCanvas.transform.up != Vector3.up)
            {
                playerCanvas.transform.up = Vector3.up;
            }
        }

        private void OnDestroy()
        {
            Die?.Invoke(this);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!photonView.IsMine) return;

            var bullet = collision.gameObject.GetComponent<BulletView>();
            if (bullet != null && bullet.Owner != photonView.Owner)
            {
                Damage();
                photonView.RPC("Damage", RpcTarget.OthersBuffered);
            }
        }
    }
}