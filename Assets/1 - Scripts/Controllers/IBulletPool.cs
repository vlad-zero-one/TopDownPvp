using Game.Model;
using Game.Views;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Controllers.Abstract
{
    public interface IBulletPool
    {
        public void Init(BulletView bulletView, int poolSize);
        public void Shoot(Bullet bullet);
        public void Clear();
    }
}