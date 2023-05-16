using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Game.Configs;
using DependencyInjection;
using Game.Static;
using Game.Model;

namespace Game.Controllers
{
    public class StartAppController : MonoBehaviour
    {
        [SerializeField] private Logger logger;
        [SerializeField] private PlayerSettings playerSettings;
        [SerializeField] private PlayerAppearanceData playerAppearanceData;
        [SerializeField] private GameSettings gameSettings;

        private ConnectionManager connectionManager;

        private void Awake()
        {
            DI.Add(playerSettings);
            DI.Add(playerAppearanceData);
            DI.Add(gameSettings);

            logger.Init();

            PhotonCustomTypesRegistrationManager.Register();

            connectionManager = new();
            connectionManager.ConnectedToMaster += SwitchScene;

            PhotonNetwork.AddCallbackTarget(connectionManager);
            connectionManager.InitConnection();
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