using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu]
    public class GameSettings : ScriptableObject
    {
        [SerializeField] private float alertShowTime;
        [SerializeField] private float destroyDeadTime;
        [SerializeField] private int minPlayersForGame;
        [SerializeField] private int coinsOnField;
        [SerializeField] private int attemptsToSpawnCoin;

        public float AlertShowTime => alertShowTime;
        public float DestroyDeadTime => destroyDeadTime;
        public int MinPlayersForGame => minPlayersForGame;
        public int CoinsOnField => coinsOnField;
        public int AttemptsToSpawnCoin => attemptsToSpawnCoin;
    }
}