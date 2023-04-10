using UnityEngine;
using DependencyInjection;
using UnityEngine.UI;
using Photon.Pun;
using Game.Configs;
using Photon.Realtime;

namespace Game.Controllers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private Button leaveButton;
        [SerializeField] private TouchPadMoveController moveController;
        [SerializeField] private ButtonShootController shootController;

        [SerializeField] private PlayerView playerPrefab;
        [SerializeField] private BulletView bulletPrefab;

        [SerializeField] private MapController mapController;

        private ConnectionManager connectionManager;
        private Logger logger;

        private PlayerView player;
        private Vector2 lastDirection = Vector2.up;
        private PlayerSettings playerSettings;

        private void Awake()
        {
            connectionManager = DI.Get<ConnectionManager>();
            logger = DI.Get<Logger>();

            var playerSkins = DI.Get<PlayerSkinsData>();
            playerSettings = DI.Get<PlayerSettings>();

            leaveButton.onClick.AddListener(LeaveRoom);

            var playerGO = PhotonNetwork.Instantiate(playerPrefab.name,
                    mapController.GetSpawnPoint().transform.position,
                    Quaternion.identity);

            player = playerGO.GetComponent<PlayerView>();
            player.Init(PhotonNetwork.LocalPlayer.NickName, playerSkins, playerSettings.PlayerSpeed, playerSettings.PlayerHealth);

            moveController.Init();
            moveController.MoveDirective += MovePlayer;
            moveController.StopDirective += StopPlayer;

            shootController.Init();
            shootController.ShootDirective += Shoot;

            connectionManager.LeftRoom += LoadLobbyScene;
            connectionManager.NewMaster += SyncCoinsForNewPlayer;

            if (PhotonNetwork.IsMasterClient)
            {
                connectionManager.NewPlayerJoined += mapController.SyncCoins;
            }
        }

        private void SyncCoinsForNewPlayer(Player newMaster)
        {
            if (newMaster == PhotonNetwork.LocalPlayer)
            {
                connectionManager.NewPlayerJoined += mapController.SyncCoins;
            }
        }

        private void Shoot()
        {
            player.photonView.RPC("Shoot",
                RpcTarget.AllViaServer,
                player.transform.position.x,
                player.transform.position.y,
                lastDirection.x,
                lastDirection.y);
        }

        private void LoadLobbyScene()
        {
            PhotonNetwork.LoadLevel(Scenes.LobbyScene);
        }

        private void MovePlayer(Vector2 direction)
        {
            if (direction != Vector2.zero)
            {
                lastDirection = direction;
                player.StartMove(direction);
            }
        }

        private void StopPlayer()
        {
            player.StopMove();
        }

        private void LeaveRoom()
        {
            connectionManager.LeaveRoom();
        }

        private void OnDestroy()
        {
            moveController.MoveDirective -= MovePlayer;
            moveController.StopDirective -= StopPlayer;
            connectionManager.LeftRoom -= LoadLobbyScene;
            connectionManager.NewMaster -= SyncCoinsForNewPlayer;

            if (PhotonNetwork.IsMasterClient)
            {
                connectionManager.NewPlayerJoined -= mapController.SyncCoins;
            }
        }
    }
}