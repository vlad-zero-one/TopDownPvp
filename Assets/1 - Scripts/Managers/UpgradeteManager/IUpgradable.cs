using Game.Model.Upgrades;

namespace Game.Managers.Abstract
{
    public interface IUpgradable
    {
        public void Upgrade(BaseUpgrade upgrade);
    }
}