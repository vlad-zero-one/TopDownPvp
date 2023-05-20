namespace Game.UI.Abstract
{
    public interface IShootController
    {
        public void SetCooldown(float shootCooldown);
        public delegate void ShootEventHandler();
        public event ShootEventHandler ShootDirective;
    }
}