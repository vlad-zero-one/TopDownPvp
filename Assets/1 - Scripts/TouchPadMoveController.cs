using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Controllers
{
    public class TouchPadMoveController : MonoBehaviour, IDragHandler, IEndDragHandler, IMoveController
    {
        [SerializeField] private RectTransform joystickHandler;
        [SerializeField] private RectTransform joystick;

        private float range;

        public event IMoveController.MoveEventHandler MoveDirective;

        public void Init()
        {
            range = joystickHandler.rect.width / 2 - joystick.rect.width / 2;
        }

        public void OnDrag(PointerEventData data)
        {
            if (data.delta.magnitude > 0.1f)
            {
                joystickHandler.position = data.pressPosition;
                Vector2 moveDirection;

                if ((data.position - data.pressPosition).magnitude < range)
                {
                    joystick.position = data.position;
                    moveDirection = new(
                        (data.position.x - data.pressPosition.x) / range,
                        (data.position.y - data.pressPosition.y) / range);
                }
                else
                {
                    float deltaX = data.position.x - data.pressPosition.x;
                    float deltaY = data.position.y - data.pressPosition.y;

                    var state = Vector2.ClampMagnitude(new(deltaX, deltaY), range);

                    joystick.position = state + data.pressPosition;
                    moveDirection = new(state.x / range, state.y / range);
                }
                MoveDirective?.Invoke(moveDirection);
            }
        }

        public void OnEndDrag(PointerEventData data)
        {
            joystickHandler.anchoredPosition = Vector2.zero;
            joystick.anchoredPosition = Vector2.zero;
            MoveDirective?.Invoke(Vector2.zero);
        }
    }
}