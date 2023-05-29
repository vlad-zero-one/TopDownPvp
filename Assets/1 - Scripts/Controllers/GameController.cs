using UnityEngine;
using DependencyInjection;
using UnityEngine.UI;
using Photon.Pun;
using Game.Configs;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using Game.Views;
using System.Collections.Generic;
using Game.UI;
using Game.Static;
using Game.UI.Abstract;
using Game.Managers;

namespace Game.Controllers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private Button leaveButton;
        [SerializeField] private TouchPadMoveController touchPadMoveController;
        [SerializeField] private ButtonShootController buttonShootController;
        [SerializeField] private KeyboardController keyboardController;

        [SerializeField] private MapController mapController;
        [SerializeField] private PlayerStatsController playerStatsController;
        [SerializeField] private EndBattleScreenController endBattleScreen;

        [SerializeField] private BulletPoolPun bulletPoolPun;
        [SerializeField] private BulletView bulletViewPrefab;

        private ConnectionManager connectionManager;
        private GameSettings gameSettings;

        private PlayerView player;

        private List<PlayerView> otherPlayers = new();
        private bool defeated;

        private WeaponManager weaponManager;
        private MoveManager moveManager;

        public void AddOtherPlayer(PlayerView otherPlayer)
        {
            otherPlayers.Add(otherPlayer);

            otherPlayer.Die += RemoveFromList;
        }

        private void Awake()
        {
            DI.Add(this);

            connectionManager = DI.Get<ConnectionManager>();
            gameSettings = DI.Get<GameSettings>();

            var data = new object[1];
            data[0] = DI.Get<PlayerAppearanceData>().GetRandomSkinName();

            player = PhotonNetwork.Instantiate(NetworkPrefabs.Player,
                    mapController.GetSpawnPoint().transform.position,
                    Quaternion.identity,
                    data: data)
                .GetComponent<PlayerView>();

            var playerSettings = DI.Get<PlayerSettings>();

            playerStatsController.Init(playerSettings, player);

            IMoveController moveController = 
                gameSettings.KeyBoardControl ? keyboardController : touchPadMoveController;
            IShootController shootController = 
                gameSettings.KeyBoardControl ? keyboardController : buttonShootController;

            moveManager = new MoveManager(moveController, player);

            weaponManager = new WeaponManager(
                connectionManager, 
                shootController,
                bulletPoolPun, 
                player, 
                bulletViewPrefab, 
                playerSettings, 
                gameSettings);

            InitSubscribtions();
        }

        private void InitSubscribtions()
        {
            leaveButton.onClick.AddListener(LeaveRoom);

            connectionManager.LeftRoom += LoadLobbyScene;
            connectionManager.NewMaster += SyncCoinsForNewPlayer;

            player.Die += OnPlayerDied;

            if (PhotonNetwork.IsMasterClient)
            {
                connectionManager.NewPlayerJoined += mapController.SyncCoins;
            }
        }

        private void RemoveFromList(PlayerView otherPlayer)
        {
            otherPlayer.Die -= RemoveFromList;

            if (otherPlayers.Contains(otherPlayer))
            {
                otherPlayers.Remove(otherPlayer);
                Destroy(otherPlayer.gameObject, gameSettings.DestroyDeadTime);

                if (otherPlayers.Count == 0 && !defeated)
                {
                    Win();
                }
            }
        }

        private void SyncCoinsForNewPlayer(Player newMaster)
        {
            if (newMaster == PhotonNetwork.LocalPlayer)
            {
                connectionManager.NewPlayerJoined += mapController.SyncCoins;
            }
        }

        private void Win()
        {
            endBattleScreen.Show(true, player.Coins);

            if (PhotonNetwork.CurrentRoom != null)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }

        private void OnPlayerDied(PlayerView _)
        {
            defeated = true;

            endBattleScreen.Show(false, player.Coins);

            Destroy(player.gameObject, gameSettings.DestroyDeadTime);
        }

        private void LoadLobbyScene()
        {
            SceneManager.LoadScene(Scenes.LobbyScene);
        }

        private void LeaveRoom()
        {
            connectionManager.LeaveRoom();
        }

        private void OnDestroy()
        {
            leaveButton.onClick.RemoveListener(LeaveRoom);

            moveManager.Dispose();
            weaponManager.Dispose();

            connectionManager.LeftRoom -= LoadLobbyScene;
            connectionManager.NewMaster -= SyncCoinsForNewPlayer;

            player.Die -= OnPlayerDied;

            foreach (var otherPlayer in otherPlayers)
            {
                otherPlayer.Die -= RemoveFromList;
            }

            connectionManager.NewPlayerJoined -= mapController.SyncCoins;

            DI.Remove(this);
        }
    }
}