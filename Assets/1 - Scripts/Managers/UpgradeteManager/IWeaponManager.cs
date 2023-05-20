using Game.Model.Upgrades;

namespace Game.Managers.Abstract
{
    public interface IWeaponManager
    {
        public void Upgrade(BaseUpgrade upgrade);
    }
}