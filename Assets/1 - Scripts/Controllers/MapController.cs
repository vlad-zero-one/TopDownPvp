using DependencyInjection;
using Game.Views;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Controllers
{
    public class MapController : MonoBehaviour
    {
        [SerializeField] private List<MapPoint> spawnPoints;
        [SerializeField] private List<MapPoint> coinPoints;

        [SerializeField] private CoinView coinPrefab;

        private List<CoinView> coins;

        private void Awake()
        {
            DI.Add(this);

            coins = new();

            if (PhotonNetwork.IsMasterClient)
            {
                SpawnCoins();
            }
        }

        public void AddCoin(CoinView coin)
        {
            coins.Add(coin);
        }

        private void SpawnCoins()
        {
            coins.Add(
                PhotonNetwork
                .InstantiateRoomObject("Coin", GetCoinPoint(), Quaternion.identity)
                .GetComponent<CoinView>());
        }

        public void SyncCoins(Player player)
        {
            foreach(var coin in coins)
            {
                var pos = coin.transform.position;
                coin.photonView.RPC("MoveCoin", RpcTarget.Others, pos.x, pos.y);
            }
        }

        public Vector2 GetCoinPoint()
        {
            var index = Random.Range(0, coinPoints.Count);
            if (coinPoints[index].Occupied)
            {
                // limit the number of attempts to spawn in an unoccupied slot, in case each slot is occupied
                for (var i = 0; i < 5; i++)
                {
                    index = Random.Range(0, coinPoints.Count);
                    if (!coinPoints[index].Occupied) break;
                }
            }

            return coinPoints[index].transform.position;
        }

        public MapPoint GetSpawnPoint()
        {
            return spawnPoints[Random.Range(0, spawnPoints.Count)];
        }

        private void OnDestroy()
        {
            DI.Remove(this);
        }
    }
}
