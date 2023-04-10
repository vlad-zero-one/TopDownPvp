using UnityEngine;

namespace Game.Views
{
    public class MapPoint : MonoBehaviour
    {
        public bool Occupied { get; protected set; }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(Tags.Player)
                || collision.gameObject.CompareTag(Tags.Coin))
            {
                Occupied = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(Tags.Player)
                || collision.gameObject.CompareTag(Tags.Coin))
            {
                Occupied = false;
            }
        }
    }
}