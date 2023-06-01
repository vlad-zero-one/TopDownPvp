using DependencyInjection;
using Game.Configs;
using Game.Controllers;
using Game.Model;
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

        private PlayerSettings playerSettings;
        private GameSettings gameSettings;

        private float speed;

        private Vector3 moveDirection;
        private bool moving;

        private bool justSpawned;

        public PlayerModel PlayerModel { get; private set; }

        // TODO: if one machine instantiate PlayerView before other machine awake GameController,
        // AddOtherPlayer will throw exception, rewrite this !!
        // maybe instantiating everything on the master client is not such a bad idea
        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            var skinName = (string) info.photonView.InstantiationData[0];

            gameSettings = DI.Get<GameSettings>();
            playerSettings = DI.Get<PlayerSettings>();

            PlayerModel = new(photonView.Owner, playerSettings.PlayerHealth);
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
                slider.maxValue = PlayerModel.Hp;
                slider.value = PlayerModel.Hp;
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

        public void AddCoins(int value = 1)
        {
            PlayerModel.AddCoins(value);
        }

        [PunRPC]
        private void Damage(int value)
        {
            PlayerModel.Damage(value);
            slider.value = PlayerModel.Hp;
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

        private void OnDestroy() => PlayerModel.OnViewDestroyed();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!photonView.IsMine) return;

            var bullet = collision.gameObject.GetComponent<BulletView>();
            if (bullet != null && bullet.Owner != photonView.Owner)
            {
                Damage(bullet.Damage);
                photonView.RPC("Damage", RpcTarget.OthersBuffered, bullet.Damage);
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