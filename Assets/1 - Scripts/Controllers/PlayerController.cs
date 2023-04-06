using UnityEngine;
using UnityEngine.UI;

namespace Game.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rbody;
        [SerializeField] private Text nickName;

        private float speed;

        public void Init(string nickName, float speed = 5f)
        {
            this.nickName.text = nickName;
            this.speed = speed;
        }

        public void Move(Vector2 direction)
        {
            rbody.velocity = direction * speed;
        }
    }
}