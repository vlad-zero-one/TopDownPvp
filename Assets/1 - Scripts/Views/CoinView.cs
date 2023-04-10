using DependencyInjection;
using Game.Controllers;
using Photon.Pun;
using UnityEngine;

namespace Game.Views
{
    public class CoinView : MonoBehaviourPun, IPunInstantiateMagicCallback
    {
        private MapController mapController;

        public delegate void CollectedEventHandler(CoinView sender);
        public event CollectedEventHandler Collected;

        private MapController MapController => 
            mapController = mapController != null ? mapController : DI.Get<MapController>();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                if (player.photonView.IsMine)
                {
                    player.AddCoint();
                    var point = MapController.GetCoinPoint();

                    photonView.RPC("MoveCoin", RpcTarget.AllViaServer, point.x, point.y);
                }
            }
        }

        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            MapController.AddCoin(this);
        }

        [PunRPC]
        private void MoveCoin(float x, float y)
        {
            transform.position = new(x, y);
        }
    }
}