using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DependencyInjection;
using WebSocketSharp;
using UnityEngine.SceneManagement;
using Game.Configs;
using Game.Static;
using Photon.Pun;
using Photon.Realtime;

namespace Game.Controllers
{
    public class LobbyController : MonoBehaviour
    {
        [SerializeField] private Text alertTextObject;

        [Header("Create and enter block")]
        [SerializeField] private GameObject createAndEnterContainer;
        [SerializeField] private InputField newRoomName;
        [SerializeField] private InputField roomName;
        [SerializeField] private Button createRoomButton;
        [SerializeField] private Button enterRoomButton;

        [Header("Start match block")]
        [SerializeField] private GameObject startMatchContainer;
        [SerializeField] private Text startMatchInfoText;
        [SerializeField] private Text playersCountText;
        [SerializeField] private Button playButton;
        [SerializeField] private Button leaveRoomButton;

        private ConnectionManager connectionManager;

        private GameSettings gameSettings;

        private Coroutine alertRoutine;

        private string startMatchInfo;
        private string playersCount;

        private void Awake()
        {
            connectionManager = DI.Get<ConnectionManager>();
            gameSettings = DI.Get<GameSettings>();

            InitButtons();

            startMatchInfo = startMatchInfoText.text;
            playersCount = playersCountText.text;

            connectionManager.Error += ShowAlert;
            connectionManager.JoinedRoom += ShowStartMatchContainer;

            leaveRoomButton.onClick.AddListener(LeaveRoom);
            playButton.onClick.AddListener(Play);
        }

        private void LeaveRoom()
        {
            startMatchContainer.SetActive(false);
            createAndEnterContainer.SetActive(true);

            connectionManager.LeaveRoom();
        }

        private void ShowStartMatchContainer()
        {
            createAndEnterContainer.SetActive(false);
            startMatchContainer.SetActive(true);

            startMatchInfoText.text = string.Format(startMatchInfo, gameSettings.MinPlayersForGame);
            CountPlayers();

            connectionManager.CountOfPlayersInRoomsChanged += CountPlayers;
        }

        private void CountPlayers(int countOfPlayers = 0)
        {
            playersCountText.text = string.Format(playersCount,
                countOfPlayers == 0 ? PhotonNetwork.CurrentRoom.PlayerCount : countOfPlayers);

            playButton.interactable = PhotonNetwork.IsMasterClient && 
                (connectionManager.EnoughPlayers || gameSettings.StartWithOnePlayer);
        }

        private void Play()
        {
            PhotonNetwork.LoadLevel(Scenes.GameScene);
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
            connectionManager.JoinedRoom -= ShowStartMatchContainer;

            connectionManager.CountOfPlayersInRoomsChanged -= CountPlayers;

            leaveRoomButton.onClick.RemoveListener(LeaveRoom);
            playButton.onClick.RemoveListener(Play);
        }
    }
}
