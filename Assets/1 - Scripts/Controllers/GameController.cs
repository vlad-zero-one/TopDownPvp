using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DependencyInjection;
using UnityEngine.UI;

namespace Game
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private Button leaveButton;

        private ConnectionManager connectionManager;

        private void Awake()
        {
            if(connectionManager == null)
            {
                connectionManager = DI.Get<ConnectionManager>();
            }

            leaveButton.onClick.AddListener(LeaveRoom);
        }

        private void LeaveRoom()
        {
            connectionManager.LeaveRoom();
        }
    }
}