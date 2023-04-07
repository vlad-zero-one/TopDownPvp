namespace Game.Controllers
{
    public interface IShootController
    {
        public void Init();
        public delegate void ShootEventHandler();
        public event ShootEventHandler ShootDirective;
    }
}