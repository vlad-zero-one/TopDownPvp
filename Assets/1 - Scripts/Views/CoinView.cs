using Game.Controllers;
using UnityEngine;

namespace Game.Views
{
    public class CoinView : MonoBehaviour
    {
        public delegate void CollectedEventHandler(CoinView sender);
        public event CollectedEventHandler Collected;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                if (player.photonView.IsMine)
                {
                    player.AddCoint();
                }
                Collected?.Invoke(this);
            }
        }
    }
}