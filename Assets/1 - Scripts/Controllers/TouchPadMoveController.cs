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
        public event IMoveController.StopEventHandler StopDirective;

        public void Init()
        {
            range = (joystickHandler.rect.width - joystick.rect.width) / 2
                * GetComponentInParent<Canvas>().scaleFactor;
        }

        public void OnDrag(PointerEventData data)
        {
            if (data.delta.magnitude > 0.1f)
            {
                var pressPosition = data.pressPosition;
                var position = data.position;

                joystickHandler.position = pressPosition;
                Vector2 moveDirection;

                if ((position - pressPosition).magnitude < range)
                {
                    joystick.position = position;
                    moveDirection = new(
                        (position.x - pressPosition.x) / range,
                        (position.y - pressPosition.y) / range);
                }
                else
                {
                    var deltaX = position.x - pressPosition.x;
                    var deltaY = position.y - pressPosition.y;

                    var state = Vector2.ClampMagnitude(new(deltaX, deltaY), range);

                    joystick.position = state + pressPosition;
                    moveDirection = new(state.x / range, state.y / range);
                }

                MoveDirective?.Invoke(moveDirection);
            }
        }

        public void OnEndDrag(PointerEventData data)
        {
            joystickHandler.anchoredPosition = Vector2.zero;
            joystick.anchoredPosition = Vector2.zero;
            StopDirective?.Invoke();
        }
    }
}