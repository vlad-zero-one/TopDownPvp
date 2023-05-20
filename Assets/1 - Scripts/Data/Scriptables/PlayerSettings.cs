using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu]
    public class PlayerSettings : ScriptableObject
    {
        [SerializeField] private float playerSpeed;
        [SerializeField] private int playerHealth;
        [SerializeField] private float bulletSpeed;
        [SerializeField] private int bulletDamage;
        [SerializeField] private float shootCooldown;

        public float PlayerSpeed => playerSpeed;
        public int PlayerHealth => playerHealth;
        public float BulletSpeed => bulletSpeed;
        public int BulletDamage => bulletDamage;
        public float ShootCooldown => shootCooldown;
    }
}