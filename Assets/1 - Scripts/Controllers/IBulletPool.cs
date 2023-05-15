using Game.Model;
using Game.Views;

namespace Game.Controllers.Abstract
{
    public interface IBulletPool
    {
        public void Init(BulletView bulletView, int poolSize);
        public void Shoot(Bullet bullet);
    }
}