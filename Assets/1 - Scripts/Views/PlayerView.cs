using DependencyInjection;
using Game.Configs;
using Game.Controllers;
using Game.Static;
using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Views
{
    public class PlayerView : MonoBehaviourPun, IPunInstantiateMagicCallback
    {
        [SerializeField] private Rigidbody2D rbody;
        [SerializeField] private Canvas playerCanvas;
        [SerializeField] private Text nickName;
        [SerializeField] private Slider slider;

        [SerializeField] private BulletView bulletPrefab;

        private PlayerSettings playerSettings;
        private GameSettings gameSettings;

        private int hp;
        private float speed;

        private Vector3 moveDirection;
        private bool moving;

        private bool justSpawned;

        public delegate void DieEventHandler(PlayerView sender);
        public delegate void EventHandler();

        public event DieEventHandler Die;
        public event EventHandler Damaged;
        public event EventHandler GotCoin;

        public int Coins { get; private set; }

        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            var skinName = (string) info.photonView.InstantiationData[0];

            playerSettings = DI.Get<PlayerSettings>();
            gameSettings = DI.Get<GameSettings>();

            hp = playerSettings.PlayerHealth;
            speed = playerSettings.PlayerSpeed;

            var appearanceData = DI.Get<PlayerAppearanceData>();

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
                slider.gameObject.SetActive(true);
                slider.maxValue = hp;
                slider.value = hp;
                DI.Get<GameController>().AddOtherPlayer(this);
            }

            justSpawned = true;
            StartCoroutine(SpawnSuffleCooldown());
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

        public void AddCoint()
        {
            Coins++;
            GotCoin?.Invoke();
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

        [PunRPC]
        private void Damage()
        {
            slider.value = --hp;
            Damaged?.Invoke();

            if (hp <= 0)
            {
                Die?.Invoke(this);
            }
        }

        private IEnumerator SpawnSuffleCooldown()
        {
            yield return new WaitForSeconds(gameSettings.SuffleOnSpawnTime);
            justSpawned = false;
        }

        private void FixedUpdate()
        {
            if (moving)
            {
                var lastPos = transform.position;
                var pos = lastPos + speed * Time.fixedDeltaTime * moveDirection;
                rbody.MovePosition(pos);

                transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, moveDirection));
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

        // ad hoc..
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (justSpawned)
            {
                if (collision.gameObject.CompareTag(Tags.Player))
                {
                    transform.position = DI.Get<MapController>().GetSpawnPoint().transform.position;
                }
            }
        }
    }
}