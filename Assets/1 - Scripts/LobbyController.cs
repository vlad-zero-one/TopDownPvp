using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DependencyInjection;
using Photon.Pun;
using System;
using WebSocketSharp;
using System.Threading;

namespace Game.Controllers
{
    public class LobbyController : MonoBehaviour
    {
        private const float AlertTime = 2.5f;

        [SerializeField] private InputField newRoomName;
        [SerializeField] private Button createRoomButton;

        [SerializeField] private InputField roomName;
        [SerializeField] private Button enterRoomButton;

        [SerializeField] private Text alertTextObject;

        private ConnectionManager connectionManager;

        private Coroutine alertRoutine;

        private void Awake()
        {
            connectionManager = new();
            PhotonNetwork.AddCallbackTarget(connectionManager);
            connectionManager.InitConnection();
            connectionManager.OnError += ShowAlert;

            DI.Add(connectionManager);

            InitButtons();
        }

        private void InitButtons()
        {
            createRoomButton.onClick.AddListener(CreateRoom);
            enterRoomButton.onClick.AddListener(EnterRoom);
        }

        private void EnterRoom()
        {
            if (roomName.text.IsNullOrEmpty())
            {
                ShowAlert("Enter room name");
            }
            else
            {
                connectionManager.JoinRoom(roomName.text);
            }
        }

        private void CreateRoom()
        {
            if (newRoomName.text.IsNullOrEmpty())
            {
                ShowAlert("Enter room name");
            }
            else
            {
                connectionManager.CreateRoom(newRoomName.text);
            }
        }

        private void ShowAlert(string alertText)
        {
            alertTextObject.text = alertText;

            if (alertRoutine != null)
            {
                StopCoroutine(alertRoutine);
            }

            alertRoutine = StartCoroutine(ShowAlert());
        }

        private IEnumerator ShowAlert()
        {
            alertTextObject.enabled = true;
            yield return new WaitForSeconds(AlertTime);
            alertTextObject.enabled = false;
            alertRoutine = null;
        }

        private void OnDestroy()
        {
            connectionManager.OnError -= ShowAlert;
        }
    }
}
