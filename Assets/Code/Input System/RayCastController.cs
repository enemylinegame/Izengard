using System;
using Abstraction;
using BuildingSystem;
using Izengard;
using NewBuildingSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace UserInputSystem
{
    public class RayCastController : IOnController, IOnUpdate
    {
        public event Action<String> LeftClick;
        public event Action<String> RightClick;
        public event Action KeyDownOne;
        public event Action KeyDownTwo;
        public event Action Delete;
        public event Action<Vector3> MousePosition;

        private Camera _camera;
        private LayerMask _groundMask;
        private InputAction _pointerPosition;
        private EventSystem _eventSystem;

        private bool _pointerOverUI;

        public RayCastController(UserInput input, GameConfig gameConfig)
        {
            _camera = Camera.main;
            _groundMask = gameConfig.MouseLayerMask;
            _eventSystem = EventSystem.current;
            _pointerPosition = input.PointerParameters.PointerPosition;

            input.PlayerControl.LeftClick.started += context => Click(context, true);
            input.PlayerControl.RightClick.started += context => Click(context, false);
            input.PlayerControl.Key1.started += _ => KeyDownOne?.Invoke();
            input.PlayerControl.Key2.started += _ => KeyDownTwo?.Invoke();
            input.PlayerControl.Delete.started += _ => Delete?.Invoke();
            input.PointerParameters.PointerPosition.performed += GetSelectedMapPosition;
        }

        private void Click(InputAction.CallbackContext context, bool isClick)
        {
            if (_pointerOverUI) return;

            var screenPosition = _pointerPosition.ReadValue<Vector2>();
            var ray = _camera.ScreenPointToRay(screenPosition);

            if (Physics.Raycast(ray, out var hit, 100, _groundMask))
            {
                if (isClick)
                {
                    Debug.Log($"<color=aqua> Left Click</color>");
                    if (hit.collider.GetComponent<BaseGameObject>())
                        LeftClick?.Invoke(hit.collider.GetComponent<ITarget>().Id); //TODO: Add raycast component
                    else LeftClick?.Invoke(null);
                }
                else
                {
                    Debug.Log($"<color=aqua> Right Click</color>");
                    if (hit.collider.GetComponent<BaseGameObject>())
                        RightClick?.Invoke(hit.collider.GetComponent<ITarget>().Id); //TODO: Add raycast component
                    else RightClick?.Invoke(null);
                }
            }
        }

        public bool IsPointerOverUI() => _pointerOverUI;

        private void GetSelectedMapPosition(InputAction.CallbackContext context)
        {
            var screenPosition = _pointerPosition.ReadValue<Vector2>();
            Vector3 mousePos = new Vector3(screenPosition.x, screenPosition.y, _camera.nearClipPlane);
            var ray = _camera.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out var hit, 100, _groundMask))
                MousePosition?.Invoke(hit.point);
        }

        public void OnUpdate(float deltaTime)
        {
            _pointerOverUI = _eventSystem.IsPointerOverGameObject();
        }
    }
}