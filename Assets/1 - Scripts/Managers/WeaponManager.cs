using Game.Controllers.Abstract;
using Game.Managers.Abstract;
using Game.Model.Upgrades;
using Game.UI.Abstract;

namespace Game.Managers
{
    public class WeaponManager : IWeaponManager
    {
        public void Init(IShootController shootController, IBulletPool bulletPool)
        {

        }

        public void Upgrade(BaseUpgrade upgrade)
        {
            UnityEngine.Debug.LogError("UPGRADE ");
        }

        public void Shoot()
        {

        }
    }
}