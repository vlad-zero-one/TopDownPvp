using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DependencyInjection;
using Photon.Pun;
using WebSocketSharp;

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
            connectionManager = DI.Get<ConnectionManager>();

            InitButtons();

            connectionManager.Error += ShowAlert;
            connectionManager.JoinedRoom += LoadGameScene;
        }

        private void LoadGameScene()
        {
            PhotonNetwork.LoadLevel(Scenes.GameScene);
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
            connectionManager.Error -= ShowAlert;
            connectionManager.JoinedRoom -= LoadGameScene;
        }
    }
}
