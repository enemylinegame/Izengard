using System;
using BuildingSystem;
using Izengard;
using NewBuildingSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace UserInputSystem
{
    public class RayCastController
    {
        public event Action LeftClick;
        public event Action<BuildingView> RightClick;
        public event Action KeyDownOne;
        public event Action KeyDownTwo;
        public event Action Delete;
        public event Action<Vector3> MousePosition;

        private Camera _camera;
        private LayerMask _groundMask;
        private InputAction _pointerPosition;
        private EventSystem _eventSystem;

        public RayCastController(UserInput input, GameConfig gameConfig)
        {
            _camera = Camera.main;
            _groundMask = gameConfig.MouseLayerMask;
            _eventSystem = EventSystem.current;
            _pointerPosition = input.PointerParameters.PointerPosition;

            input.PlayerControl.LeftClick.started += context => Click(context, true);
            input.PlayerControl.RightClick.started += context => Click(context, false);
            input.PlayerControl.Key1.started += context => KeyDownOne?.Invoke();
            input.PlayerControl.Key2.started += context => KeyDownTwo?.Invoke();
            input.PlayerControl.Delete.started += context => Delete?.Invoke();
            input.PointerParameters.PointerPosition.performed += GetSelectedMapPosition;
        }

        private void Click(InputAction.CallbackContext context, bool isClick)
        {
            var screenPosition = _pointerPosition.ReadValue<Vector2>();
            var ray = _camera.ScreenPointToRay(screenPosition);

            //if (_eventSystem.IsPointerOverGameObject()) return;

            if (Physics.Raycast(ray, out var hit, 100, _groundMask))
            {
                if (isClick)
                {
                    Debug.Log($"<color=aqua> Left Click</color>");
                    LeftClick?.Invoke(); //TODO: Add raycast component
                }
                else
                {
                    Debug.Log($"<color=aqua> Right Click</color>");
                    RightClick?.Invoke(hit.collider.GetComponent<BuildingView>()); //TODO: Add raycast component
                }
            }
        }

        public bool IsPointerOverUI() => _eventSystem.IsPointerOverGameObject();

        private void GetSelectedMapPosition(InputAction.CallbackContext context)
        {
            var screenPosition = _pointerPosition.ReadValue<Vector2>();
            Vector3 mousePos = new Vector3(screenPosition.x, screenPosition.y, _camera.nearClipPlane);
            var ray = _camera.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out var hit, 100, _groundMask))
                MousePosition?.Invoke(hit.point);
        }
    }
}