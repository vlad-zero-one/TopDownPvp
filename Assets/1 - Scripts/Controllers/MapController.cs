using DependencyInjection;
using Game.Configs;
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

        private List<CoinView> coins;

        private GameSettings gameSettings;

        public Vector2 GetCoinPoint()
        {
            var index = Random.Range(0, coinPoints.Count);
            if (coinPoints[index].Occupied)
            {
                // limit the number of attempts to spawn in an unoccupied slot, in case each slot is occupied
                for (var i = 0; i < gameSettings.AttemptsToSpawnCoin; i++)
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

        public void AddCoin(CoinView coin)
        {
            coins.Add(coin);
        }

        public void SyncCoins(Player player)
        {
            foreach (var coin in coins)
            {
                var pos = coin.transform.position;
                coin.photonView.RPC("MoveCoin", player, pos.x, pos.y);
            }
        }

        private void Awake()
        {
            DI.Add(this);

            gameSettings = DI.Get<GameSettings>();

            coins = new();

            if (PhotonNetwork.IsMasterClient)
            {
                SpawnCoins(gameSettings.CoinsOnField);
            }
        }

        private void SpawnCoins(int coinsAmount)
        {
            for (var i = 0; i < coinsAmount; i++)
            {
                coins.Add(
                  PhotonNetwork
                  .InstantiateRoomObject(NetworkPrefabs.Coin, GetCoinPoint(), Quaternion.identity)
                  .GetComponent<CoinView>());
            }
        }

        private void OnDestroy()
        {
            DI.Remove(this);
        }
    }
}
