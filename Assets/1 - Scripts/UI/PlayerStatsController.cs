using Game.Configs;
using Game.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class PlayerStatsController : MonoBehaviour
    {
        [SerializeField] private Slider hpBar;
        [SerializeField] private Text coinsValueText;

        private PlayerModel playerModel;

        public void Init(PlayerSettings playerSettings, PlayerModel playerModel)
        {
            this.playerModel = playerModel;
            hpBar.maxValue = playerSettings.PlayerHealth;
            hpBar.value = playerSettings.PlayerHealth;
            coinsValueText.text = "0";

            playerModel.Damaged += UpdateHp;
            playerModel.GotCoins += UpdateCoins;
        }

        private void UpdateHp()
        {
            hpBar.value = playerModel.Hp >= 0 ? playerModel.Hp : 0;
        }

        private void UpdateCoins()
        {
            coinsValueText.text = $"{playerModel.Coins}";
        }

        private void OnDestroy()
        {
            playerModel.Damaged -= UpdateHp;
            playerModel.GotCoins -= UpdateCoins;
        }
    }
}