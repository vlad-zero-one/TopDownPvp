using Photon.Realtime;

namespace Game.Model
{
    public class PlayerModel
    {
        public delegate void DieEventHandler(PlayerModel sender);
        public delegate void EventHandler();

        public event DieEventHandler Die;
        public event EventHandler Damaged;
        public event EventHandler GotCoins;

        public PlayerModel(Player photonPlayer, int initialHp)
        {
            PhotonPlayer = photonPlayer;
            Hp = initialHp;
        }

        public Player PhotonPlayer { get; }
        public int Hp { get; private set; }
        public int Coins { get; private set; }

        public void AddCoins(int value = 1)
        {
            Coins += value;
            GotCoins.Invoke();
        }

        public void Damage(int value)
        {
            Hp = Hp - value > 0 ? Hp - value : 0;
            Damaged?.Invoke();

            if (Hp <= 0)
            {
                Die?.Invoke(this);
            }
        }

        public void OnViewDestroyed()
        {
            Die?.Invoke(this);
        }
    }
}