using Game.Views;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            if (PhotonNetwork.IsMasterClient)
            {
                SpawnCoin();
            }
        }

        private void SpawnCoin()
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

            var coin = PhotonNetwork
                .Instantiate("Coin", coinPoints[index].transform.position, Quaternion.identity)
                .GetComponent<CoinView>();

            coin.Collected += Move;
        }

        private void Move(CoinView coin)
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

            coin.transform.position = coinPoints[index].transform.position;
        }

        //[PunRPC]
        //private void SpawnCoin(CoinView sender)
        //{
        //    sender.Collected -= SpawnCoin;

        //    var index = Random.Range(0, coinPoints.Count);
        //    if (coinPoints[index].Occupied)
        //    {
        //        // limit the number of attempts to spawn in an unoccupied slot, in case each slot is occupied
        //        for (var i = 0; i < 5; i++)
        //        {
        //            index = Random.Range(0, coinPoints.Count);
        //            if (!coinPoints[index].Occupied) break;
        //        }
        //    }

        //    var coin = PhotonNetwork
        //        .Instantiate("Coin", coinPoints[index].transform.position, Quaternion.identity)
        //        .GetComponent<CoinView>();

        //    coin.Collected += SpawnCoin;
        //}

        public MapPoint GetSpawnPoint()
        {
            return spawnPoints[Random.Range(0, spawnPoints.Count)];
        }
    }
}
