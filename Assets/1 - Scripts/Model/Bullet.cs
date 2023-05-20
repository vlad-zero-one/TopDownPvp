using Photon.Realtime;
using UnityEngine;

namespace Game.Model
{
    public class Bullet
    {
        public Player Owner { get; private set; }
        public Vector3 StartPosition { get; private set; }
        public Vector3 Direction { get; private set; }
        public float Speed { get; private set; }
        public int Damage { get; private set; }

        public Bullet(Player owner, Vector3 startPosition, Vector3 direction, float speed, int damage):
            this(owner, speed, damage)
        {
            Direction = direction;
            StartPosition = startPosition;
        }

        public Bullet(Player owner, float speed, int damage)
        {
            Owner = owner;
            Speed = speed;
            Damage = damage;
        }

        public Bullet Clone(Vector3 startPosition, Vector3 direction)
        {
            return new(Owner, startPosition, direction, Speed, Damage);
        }
    }
}