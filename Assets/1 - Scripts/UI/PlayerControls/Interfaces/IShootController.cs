namespace Game.UI.Abstract
{
    public interface IShootController
    {
        public void Init(float shootCooldown);
        public delegate void ShootEventHandler();
        public event ShootEventHandler ShootDirective;
    }
}