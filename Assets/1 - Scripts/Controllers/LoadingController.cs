using DependencyInjection;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Controllers
{
    public class LoadingController : MonoBehaviour
    {
        [SerializeField] private Text waitingText;

        private ConnectionManager connectionManager;

        private void Awake()
        {
            connectionManager = DI.Get<ConnectionManager>();

            if (!connectionManager.EnoughPlayers)
            {
                waitingText.enabled = true;
            }

            if (PhotonNetwork.IsMasterClient)
            {
                if (connectionManager.EnoughPlayers)
                {
                    PhotonNetwork.LoadLevel(Scenes.GameScene);
                }
                else
                {
                    connectionManager.NewPlayerJoined += CheckPlayersCount;
                }
            }

        }

        private void CheckPlayersCount(Player newPlayer)
        {
            if (connectionManager.EnoughPlayers)
            {
                PhotonNetwork.LoadLevel(Scenes.GameScene);
            }
        }

        private void OnDestroy()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                connectionManager.NewPlayerJoined -= CheckPlayersCount;
            }
        }
    }
}