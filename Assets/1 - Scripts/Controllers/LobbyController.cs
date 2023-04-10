using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DependencyInjection;
using Photon.Pun;
using WebSocketSharp;
using UnityEngine.SceneManagement;
using Game.Configs;

namespace Game.Controllers
{
    public class LobbyController : MonoBehaviour
    {
        [SerializeField] private InputField newRoomName;
        [SerializeField] private Button createRoomButton;

        [SerializeField] private InputField roomName;
        [SerializeField] private Button enterRoomButton;

        [SerializeField] private Text alertTextObject;

        private ConnectionManager connectionManager;

        private Coroutine alertRoutine;
        private float alertTime;

        private void Awake()
        {
            connectionManager = DI.Get<ConnectionManager>();
            alertTime = DI.Get<GameSettings>().AlertShowTime;

            InitButtons();

            connectionManager.Error += ShowAlert;
            connectionManager.EnoughPlayersToStart += LoadGameScene;
        }

        private void LoadGameScene()
        {
            SceneManager.LoadScene(Scenes.GameScene);
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
            yield return new WaitForSeconds(alertTime);
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
