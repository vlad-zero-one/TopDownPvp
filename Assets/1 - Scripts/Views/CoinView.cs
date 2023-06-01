using DependencyInjection;
using Game.Controllers;
using Photon.Pun;
using UnityEngine;

namespace Game.Views
{
    public class CoinView : MonoBehaviourPun, IPunInstantiateMagicCallback
    {
        private MapController mapController;

        private MapController MapController => 
            mapController = mapController != null ? mapController : DI.Get<MapController>();

        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            MapController.AddCoin(this);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var player = collision.gameObject.GetComponent<PlayerView>();
            if (player != null)
            {
                if (player.photonView.IsMine)
                {
                    player.AddCoins();
                    var point = MapController.GetCoinPoint();

                    photonView.RPC("MoveCoin", RpcTarget.AllViaServer, point.x, point.y);
                }
            }
        }

        [PunRPC]
        private void MoveCoin(float x, float y)
        {
            transform.position = new(x, y);
        }
    }
}