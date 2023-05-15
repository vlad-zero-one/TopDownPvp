using Game.Controllers.Abstract;
using Game.Model;
using Game.Views;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Controllers
{
    public class BulletPoolPun : MonoBehaviourPun, IBulletPool
    {
        private Stack<BulletView> bulletViewsPool;
        private HashSet<BulletView> activeBulletViews;

        private BulletView bulletViewPrefab;

        public void Init(BulletView bulletView, int poolSize)
        {
            bulletViewPrefab = bulletView;
            bulletViewsPool = new(poolSize);
            activeBulletViews = new(poolSize);

            gameObject.SetActive(false);

            for (var i = 0; i < poolSize; i++)
            {
                var instantiatedBullet = Instantiate(bulletViewPrefab, gameObject.transform);
                instantiatedBullet.gameObject.SetActive(false);
                bulletViewsPool.Push(instantiatedBullet);
            }

            gameObject.SetActive(true);
        }

        public void Shoot(Bullet bullet)
        {
            if (!bulletViewsPool.TryPop(out var bulletView))
            {
                bulletView = Instantiate(bulletViewPrefab, gameObject.transform);
            }

            // TODO: sprite
            bulletView.NewInit(bullet, null);
            bulletView.Hit += PoolBulletView;
            activeBulletViews.Add(bulletView);

            bulletView.gameObject.SetActive(true);
            bulletView.StartMove();
        }

        private void PoolBulletView(BulletView bulletView)
        {
            bulletView.Hit -= PoolBulletView;
            bulletView.gameObject.SetActive(false);

            bulletViewsPool.Push(bulletView);
            activeBulletViews.Remove(bulletView);
        }

        private void OnDestroy()
        {
            foreach (var view in activeBulletViews)
            {
                view.Hit -= PoolBulletView;
            }
        }
    }
}