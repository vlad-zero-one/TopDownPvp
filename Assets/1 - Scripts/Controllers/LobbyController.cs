using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DependencyInjection;
using WebSocketSharp;
using UnityEngine.SceneManagement;
using Game.Configs;
using Game.Static;
using Photon.Pun;

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

        private GameSettings gameSettings;

        private Coroutine alertRoutine;

        private void Awake()
        {
            connectionManager = DI.Get<ConnectionManager>();
            gameSettings = DI.Get<GameSettings>();

            InitButtons();

            connectionManager.Error += ShowAlert;
            connectionManager.JoinedRoom += LoadLoadingScene;
        }

        private void LoadLoadingScene()
        {
            if (connectionManager.EnoughPlayers || gameSettings.StartWithOnePlayer)
            {
                PhotonNetwork.LoadLevel(Scenes.GameScene);
            }
            else
            {
                SceneManager.LoadScene(Scenes.LoadingScene);
            }
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
            yield return new WaitForSeconds(gameSettings.AlertShowTime);
            alertTextObject.enabled = false;
            alertRoutine = null;
        }

        private void OnDestroy()
        {
            connectionManager.Error -= ShowAlert;
            connectionManager.JoinedRoom -= LoadLoadingScene;
        }
    }
}
