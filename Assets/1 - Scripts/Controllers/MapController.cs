using Game.Views;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Controllers
{
    public class MapController : MonoBehaviour
    {
        [SerializeField] private List<MapPoint> spawnPoints;

        public List<MapPoint> FreeSpawnPoints => spawnPoints.Where(point => !point.Occupied).ToList();

        public MapPoint GetSpawnPoint()
        {
            var points = spawnPoints.Where(point => !point.Occupied).ToList();

            return points.Count > 0 ? points[Random.Range(0, points.Count)] : null;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                FreeSpawnPoints.ForEach(point => Debug.LogError(point.transform.position));
            }
        }
    }
}
