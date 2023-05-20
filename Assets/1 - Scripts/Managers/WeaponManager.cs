using DependencyInjection;
using Game.Configs;
using Game.Controllers.Abstract;
using Game.Managers.Abstract;
using Game.Model;
using Game.Model.Upgrades;
using Game.UI.Abstract;
using Game.Views;
using System;

namespace Game.Managers
{
    public class WeaponManager : IWeaponManager, IDisposable
    {
        private IShootController shootController;
        private IBulletPool bulletPool;
        private PlayerView playerView;

        private PlayerSettings playerSettings;
        private GameSettings gameSettings;

        private Bullet bulletPrototype;

        public WeaponManager(PlayerView playerView, IShootController shootController, IBulletPool bulletPool)
        {
            this.playerView = playerView;
            this.shootController = shootController;
            this.bulletPool = bulletPool;

            playerSettings = DI.Get<PlayerSettings>();
            gameSettings = DI.Get<GameSettings>();

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