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
using System;
using System.Threading;

namespace Game.Controllers
{
    public class LobbyController : MonoBehaviour
    {
        [SerializeField] private Text alertTextObject;
        [SerializeField] private LoadingPanel loadingPanel;

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
        private Coroutine recountCoroutine;
        private byte playersInRoom;

        private void Awake()
        {
            connectionManager = DI.Get<ConnectionManager>();
            gameSettings = DI.Get<GameSettings>();

            createRoomButton.onClick.AddListener(CreateRoom);
            enterRoomButton.onClick.AddListener(EnterRoom);

            startMatchInfo = startMatchInfoText.text;
            playersCount = playersCountText.text;

            connectionManager.Error += ShowAlert;
            connectionManager.Error += ShowCreateAndEnterContainer;
            connectionManager.JoinedRoom += ShowStartMatchContainer;

            leaveRoomButton.onClick.AddListener(LeaveRoom);
            playButton.onClick.AddListener(Play);
        }

        private void ShowCreateAndEnterContainer(string message)
        {
            createAndEnterContainer.SetActive(true);
            loadingPanel.Hide();
        }

        private void LeaveRoom()
        {
            startMatchContainer.SetActive(false);
            loadingPanel.Show();

            connectionManager.ConnectedToMaster += HideLoading;
            connectionManager.LeaveRoom();

            StopCoroutine(recountCoroutine);
        }

        private void HideLoading()
        {
            connectionManager.ConnectedToMaster -= HideLoading;

            loadingPanel.Hide();
            createAndEnterContainer.SetActive(true);
        }

        private void ShowStartMatchContainer()
        {
            loadingPanel.Hide();
            startMatchContainer.SetActive(true);

            startMatchInfoText.text = string.Format(startMatchInfo, gameSettings.MinPlayersForGame);

            recountCoroutine ??= StartCoroutine(RecountPlayers());
        }

        // We need this because PhotonNetwork.CurrentRoom.PlayerCount ambiguous
        // in IInRoomCallbacks.OnPlayerEnteredRoom IInRoomCallbacks.OnPlayerLeftRoom
        private IEnumerator RecountPlayers()
        {
            while (this != null)
            {
                if (PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.PlayerCount != playersInRoom)
                {
                    playersInRoom = PhotonNetwork.CurrentRoom.PlayerCount;

                    playersCountText.text = string.Format(playersCount, playersInRoom);

                    playButton.interactable = PhotonNetwork.IsMasterClient &&
                        (connectionManager.EnoughPlayers || gameSettings.StartWithOnePlayer);
                }

                yield return new WaitForSeconds(1f);
            }
        }

        private void Play()
        {
            PhotonNetwork.LoadLevel(Scenes.GameScene);
        }

        private void EnterRoom()
        {
            if (roomName.text.IsNullOrEmpty())
            {
                ShowAlert("Enter room name");
            }
            else
            {
                createAndEnterContainer.SetActive(false);
                loadingPanel.Show();

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
                createAndEnterContainer.SetActive(false);
                loadingPanel.Show();

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
            connectionManager.Error -= ShowCreateAndEnterContainer;
            connectionManager.JoinedRoom -= ShowStartMatchContainer;

            leaveRoomButton.onClick.RemoveListener(LeaveRoom);
            playButton.onClick.RemoveListener(Play);

            connectionManager.ConnectedToMaster -= HideLoading;
        }
    }
}
