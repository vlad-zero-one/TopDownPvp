using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

namespace Game.Controllers
{
    public class StartAppController : MonoBehaviour
    {
        [SerializeField] private Logger logger;

        private ConnectionManager connectionManager;

        private void Awake()
        {
            logger.Init();

            connectionManager = new();
            PhotonNetwork.AddCallbackTarget(connectionManager);
            connectionManager.InitConnection();

            SceneManager.LoadSceneAsync(Scenes.LobbyScene);
        }
    }
}