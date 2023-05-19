using Game.Controllers.Abstract;
using Game.Model.Upgrades;
using Game.UI.Abstract;

namespace Game.Managers.Abstract
{
    public interface IWeaponManager
    {
        public void Init(IShootController shootController, IBulletPool bulletPool);
        public void Upgrade(BaseUpgrade upgrade);
        public void Shoot();
    }
}