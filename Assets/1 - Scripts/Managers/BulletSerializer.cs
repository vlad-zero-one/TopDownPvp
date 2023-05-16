using Photon.Pun;
using System;
using UnityEngine;

namespace Game.Model
{
    public static class BulletSerializer
    {
        private const int ByteArraySize = sizeof(int) + 5 * sizeof(float);

        public static byte[] Serialize(object bulletObject)
        {
            byte[] result = new byte[ByteArraySize];

            var bullet = (Bullet)bulletObject;

            BitConverter.GetBytes(bullet.Owner.ActorNumber).CopyTo(result, 0);
            BitConverter.GetBytes(bullet.Position.x).CopyTo(result, sizeof(int));
            BitConverter.GetBytes(bullet.Position.y).CopyTo(result, sizeof(int) + sizeof(float));
            BitConverter.GetBytes(bullet.Direction.x).CopyTo(result, sizeof(int) + 2 * sizeof(float));
            BitConverter.GetBytes(bullet.Direction.y).CopyTo(result, sizeof(int) + 3 * sizeof(float));
            BitConverter.GetBytes(bullet.Speed).CopyTo(result, sizeof(int) + 4 * sizeof(float));

            return result;
        }

        public static Bullet Deserialize(byte[] data)
        {
            if (data.Length != ByteArraySize || PhotonNetwork.CurrentRoom == null)
            {
                return null;
            }

            var playerId = BitConverter.ToInt32(data, 0);
            var player = PhotonNetwork.CurrentRoom.GetPlayer(playerId);
            if (player == null)
            {
                return null;
            }

            var position = new Vector2(
                BitConverter.ToSingle(data, sizeof(int)),
                BitConverter.ToSingle(data, sizeof(int) + sizeof(float)));

            var direction = new Vector2(
                BitConverter.ToSingle(data, sizeof(int) + 2 * sizeof(float)),
                BitConverter.ToSingle(data, sizeof(int) + 3 * sizeof(float)));

            var speed = BitConverter.ToSingle(data, sizeof(int) + 4 * sizeof(float));

            return new(player, position, direction, speed);
        }
    }
}