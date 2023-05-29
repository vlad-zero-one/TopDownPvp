using Game.Configs;
using Game.Controllers.Abstract;
using Game.Model;
using Game.Model.Upgrades;
using Game.UI.Abstract;
using Game.Views;
using System;

namespace Game.Managers
{
    /// <summary>
    /// Manager where weapon managment and shooting must be implemented
    /// </summary>
    public class WeaponManager : IDisposable
    {
        private readonly IShootController shootController;
        private readonly IBulletPool bulletPool;
        private readonly PlayerView playerView;

        private readonly PlayerSettings playerSettings;

        private Bullet bulletPrototype;

        public WeaponManager(
            ConnectionManager connectionManager,
            IShootController shootController,
            IBulletPool bulletPool,
            PlayerView playerView,
            BulletView bulletViewPrefab,
            PlayerSettings playerSettings,
            GameSettings gameSettings)
        {
            this.playerView = playerView;
            this.shootController = shootController;
            this.bulletPool = bulletPool;

            this.playerSettings = playerSettings;

            bulletPool.Init(bulletViewPrefab,
                gameSettings.BulletPoolCapacityPerPlayer * connectionManager.PlayersCountInTheRoom);

            bulletPrototype =
                new(playerView.photonView.Owner, playerSettings.BulletSpeed, playerSettings.BulletDamage);

            this.shootController.SetCooldown(playerSettings.ShootCooldown);
            shootController.ShootDirective += Shoot;
        }

        public void Upgrade(BaseUpgrade upgrade)
        {
            switch (upgrade.Type)
            {
                case UpgradeType.Damage:
                case UpgradeType.BulletSpeed:
                    SetNewBulletPrototype(upgrade);
                    break;
                case UpgradeType.ShootingRate:
                    shootController.SetCooldown(playerSettings.ShootCooldown / upgrade.CurrentMultiplier);
                    break;
                case UpgradeType.DoubleShoot:
                case UpgradeType.AngleDoubleShoot:
                default:
                    throw new($"{this}.Upgrade method can't handle {upgrade.Type}");
            }
        }

        public void Dispose()
        {
            shootController.ShootDirective -= Shoot;
        }

        private void Shoot()
        {
            bulletPool.Shoot(bulletPrototype.Clone(playerView.transform.position, playerView.transform.up));
        }

        private void SetNewBulletPrototype(BaseUpgrade upgrade)
        {
            switch (upgrade.Type)
            {
                case UpgradeType.Damage:
                    bulletPrototype = new(
                        bulletPrototype.Owner,
                        bulletPrototype.Speed,
                        bulletPrototype.Damage * (int)upgrade.CurrentMultiplier);
                    break;
                case UpgradeType.BulletSpeed:
                    bulletPrototype = new(
                        bulletPrototype.Owner,
                        bulletPrototype.Speed * upgrade.CurrentMultiplier,
                        bulletPrototype.Damage);
                    break;
                default:
                    throw new($"{this}.SetNewBulletPrototype method can't handle {upgrade.Type}");
            }
        }
    }
}