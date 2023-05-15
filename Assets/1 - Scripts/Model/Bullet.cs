using Photon.Realtime;
using UnityEngine;

namespace Game.Model
{
    public class Bullet
    {
        public Player Owner { get; private set; }
        public Vector3 Position { get; private set; }
        public Vector3 Direction { get; private set; }
        public float Speed { get; private set; }
        public float Lag { get; private set; }

        public Bullet(Player owner, Vector3 position, Vector3 direction, float speed, float lag = 0)
        {
            Owner = owner;
            Direction = direction;
            Position = position;
            Speed = speed;
            Lag = lag;
        }
    }
}