using DependencyInjection;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Controllers
{
    public class PlayerController : MonoBehaviourPun
    {
        [SerializeField] private Rigidbody2D rbody;
        [SerializeField] private Canvas playerCanvas;
        [SerializeField] private Text nickName;

        private float speed;

        private Vector3 moveDirection;

        private bool moving;

        private int hp = 10;

        public void Init(string nickName, float speed = 4f)
        {
            this.nickName.text = nickName;
            this.speed = speed;

            photonView.RPC("SyncName", RpcTarget.OthersBuffered, nickName);
        }

        [PunRPC]
        public void SyncName(string nickName)
        {
            this.nickName.text = nickName;
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

        [PunRPC]
        public void Damage()
        {
            nickName.text = $"{--hp}";
        }

        private void FixedUpdate()
        {
            if (moving)
            {
                var lastPos = transform.position;
                var pos = lastPos + speed * Time.fixedDeltaTime * moveDirection;
                rbody.MovePosition(pos);

                transform.up = pos - lastPos;
            }
        }

        private void Update()
        {
            if (playerCanvas.transform.up != Vector3.up)
            {
                playerCanvas.transform.up = Vector3.up;
            }
        }
    }
}