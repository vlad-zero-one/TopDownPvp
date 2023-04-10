﻿using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Game.Configs;
using DependencyInjection;

namespace Game.Controllers
{
    public class StartAppController : MonoBehaviour
    {
        [SerializeField] private Logger logger;
        [SerializeField] private PlayerSettings playerSettings;
        [SerializeField] private PlayerAppearanceData playerAppearanceData;

        private ConnectionManager connectionManager;

        private void Awake()
        {
            logger.Init();

            connectionManager = new();
            connectionManager.ConnectedToMaster += SwitchScene;

            PhotonNetwork.AddCallbackTarget(connectionManager);
            connectionManager.InitConnection();

            DI.Add(playerSettings);
            DI.Add(playerAppearanceData);
        }

        private void SwitchScene()
        {
            SceneManager.LoadScene(Scenes.LobbyScene);
        }

        private void OnDestroy()
        {
            connectionManager.ConnectedToMaster -= SwitchScene;
        }
    }
}