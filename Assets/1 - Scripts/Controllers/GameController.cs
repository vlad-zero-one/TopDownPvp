using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DependencyInjection;
using UnityEngine.UI;
using Game.Controllers;

namespace Game
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private Button leaveButton;
        [SerializeField] private TouchPadMoveController moveController;

        private ConnectionManager connectionManager;
        private Logger logger;

        private void Awake()
        {
            connectionManager = DI.Get<ConnectionManager>();
            logger = DI.Get<Logger>();

            leaveButton.onClick.AddListener(LeaveRoom);

            moveController.Init();
            moveController.OnMove += logger.Log;
        }

        private void LeaveRoom()
        {
            connectionManager.LeaveRoom();
        }
    }
}