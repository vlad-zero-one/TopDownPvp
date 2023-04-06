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

            player = PhotonNetwork.Instantiate(playerPrefab.name,
                    new(UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(-5, 5)),
                    Quaternion.identity)
                .GetComponent<PlayerController>();

            player.Init(PhotonNetwork.LocalPlayer.NickName);

            moveController.Init();
            moveController.MoveDirective += MovePlayer;
        }

        private void MovePlayer(Vector2 direction)
        {
            player.Move(direction);
        }

        private void LeaveRoom()
        {
            connectionManager.LeaveRoom();
        }

        private void OnDestroy()
        {
            moveController.MoveDirective -= MovePlayer;
        }
    }
}