using UnityEngine;
using UnityEngine.UI;

namespace Game.Controllers
{
    public class EndBattleScreenController : MonoBehaviour
    {
        [SerializeField] private GameObject victoryContainer;
        [SerializeField] private GameObject defeatContainer;
        [SerializeField] private Text coinsText;

        public void Show(bool victory, int coins)
        {
            gameObject.SetActive(true);
            victoryContainer.SetActive(victory);
            defeatContainer.SetActive(!victory);
            coinsText.text = $"{coins}";
        }
    }
}
