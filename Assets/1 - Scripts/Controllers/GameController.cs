using UnityEngine;
using DependencyInjection;
using UnityEngine.UI;
using Game.Controllers;
using System;
using UnityEditor;
using Photon.Pun;

namespace Game.Controllers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private Button leaveButton;
        [SerializeField] private TouchPadMoveController moveController;

        [SerializeField] private PlayerController playerPrefab;

        private ConnectionManager connectionManager;
        private Logger logger;

        private PlayerController player;

        private void Awake()
        {
            connectionManager = DI.Get<ConnectionManager>();
            logger = DI.Get<Logger>();

            leaveButton.onClick.AddListener(LeaveRoom);

            var playerGO = PhotonNetwork.Instantiate(playerPrefab.name,
                    new(UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(-5, 5)),
                    Quaternion.identity);

            player = playerGO.GetComponent<PlayerController>();
            player.Init(PhotonNetwork.LocalPlayer.NickName);

            moveController.Init();
            moveController.MoveDirective += MovePlayer;
            moveController.StopDirective += StopPlayer;

            connectionManager.LeftRoom += LoadLobbyScene;
        }
        private void LoadLobbyScene()
        {
            PhotonNetwork.LoadLevel(Scenes.LobbyScene);
        }

        private void MovePlayer(Vector2 direction)
        {
            player.StartMove(direction);
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
        }
    }
}