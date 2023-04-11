using Game.Configs;
using Game.Views;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class PlayerStatsController : MonoBehaviour
    {
        [SerializeField] private Slider hpBar;
        [SerializeField] private Text coinsValueText;

        private PlayerView player;

        public void Init(PlayerSettings playerSettings, PlayerView player)
        {
            this.player = player;
            hpBar.maxValue = playerSettings.PlayerHealth;
            hpBar.value = playerSettings.PlayerHealth;
            coinsValueText.text = "0";

            player.Damaged += DecreaseHp;
            player.GotCoin += UpdateCoins;
        }

        private void DecreaseHp()
        {
            hpBar.value--;
        }

        private void UpdateCoins()
        {
            coinsValueText.text = $"{player.Coins}";
        }

        private void OnDestroy()
        {
            player.Damaged -= DecreaseHp;
            player.GotCoin -= UpdateCoins;
        }
    }
}