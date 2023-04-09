using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu]
    public class PlayerSettings : ScriptableObject
    {
        [SerializeField] private float playerSpeed;
        [SerializeField] private int playerHealth;
        [SerializeField] private float bulletSpeed;

        public float PlayerSpeed => playerSpeed;
        public int PlayerHealth => playerHealth;
        public float BulletSpeed => bulletSpeed;
    }
}