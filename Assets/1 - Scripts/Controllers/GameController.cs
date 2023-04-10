using UnityEngine;
using DependencyInjection;
using UnityEngine.UI;
using Photon.Pun;
using Game.Configs;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using Game.Views;

namespace Game.Controllers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private Button leaveButton;
        [SerializeField] private TouchPadMoveController moveController;
        [SerializeField] private ButtonShootController shootController;

        [SerializeField] private MapController mapController;

        private ConnectionManager connectionManager;
        private Logger logger;

        private PlayerView player;
        private Vector2 lastDirection = Vector2.up;

        private void Awake()
        {
            connectionManager = DI.Get<ConnectionManager>();
            logger = DI.Get<Logger>();

            object[] data = new object[1];
            data[0] = DI.Get<PlayerAppearanceData>().GetRandomSkinName();

            player = PhotonNetwork.Instantiate(NetworkPrefabs.Player,
                    mapController.GetSpawnPoint().transform.position,
                    Quaternion.identity,
                    data: data)
                .GetComponent<PlayerView>();

            moveController.Init();
            shootController.Init(DI.Get<PlayerSettings>().ShootCooldown);

            InitSubscribtions();
        }

        private void InitSubscribtions()
        {
            leaveButton.onClick.AddListener(LeaveRoom);

            moveController.MoveDirective += MovePlayer;
            moveController.StopDirective += StopPlayer;

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
            SceneManager.LoadScene(Scenes.LobbyScene);
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
            leaveButton.onClick.RemoveListener(LeaveRoom);

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