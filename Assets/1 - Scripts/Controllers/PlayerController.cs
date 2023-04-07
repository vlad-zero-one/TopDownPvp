using UnityEngine;
using UnityEngine.UI;

namespace Game.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rbody;
        [SerializeField] private Text nickName;

        private float speed;

        private Vector3 moveDirection;

        private bool moving;

        public void Init(string nickName, float speed = 4f)
        {
            this.nickName.text = nickName;
            this.speed = speed;
        }

        public void StartMove(Vector2 moveDirection)
        {
            this.moveDirection = moveDirection;
            moving = true;
        }

        public void StopMove()
        {
            moving = false;
        }

        private void FixedUpdate()
        {
            if (moving)
            {
                rbody.MovePosition(transform.position + speed * Time.fixedDeltaTime * moveDirection);
            }
        }
    }
}