using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu]
    public class GameSettings : ScriptableObject
    {
        [SerializeField] private float alertShowTime;
        [SerializeField] private int minPlayersForGame;
        [SerializeField] private int coinsOnField;

        public float AlertShowTime => alertShowTime;
        public int MinPlayersForGame => minPlayersForGame;
        public int CoinsOnField => coinsOnField;
    }
}