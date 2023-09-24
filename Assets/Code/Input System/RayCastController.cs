using System;
using BuildingSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace UserInputSystem
{
    public class RayCastController
    {
        public event Action LeftClick;
        public event Action RightClick;

        private Camera _camera;
        private LayerMask _groundMask;
        private InputAction _pointerPosition;
        private EventSystem _eventSystem;

        public RayCastController(UserInput input)
        {
            _camera = Camera.main;
        
            _eventSystem = EventSystem.current;
            _groundMask = LayerMask.GetMask("UI");
            _pointerPosition = input.PointerParameters.PointerPosition;

            input.PlayerControl.LeftClick.started += context => Click(context, true);
            input.PlayerControl.RightClick.started += context => Click(context, false);
        }

        private void Click(InputAction.CallbackContext context, bool isClick)
        {
            var screenPosition = _pointerPosition.ReadValue<Vector2>();
            var ray = _camera.ScreenPointToRay(screenPosition);

            if (_eventSystem.IsPointerOverGameObject()) return;

            if (Physics.Raycast(ray, out var hit, 100))
            {
                
                if (isClick)
                {
                    Debug.Log($"<color=aqua> Left Click</color>");
                    LeftClick?.Invoke();//TODO: Add raycast component
                }
                else
                {
                    Debug.Log($"<color=aqua> Right Click</color>");
                    RightClick?.Invoke();//TODO: Add raycast component
                }
                
            }
        }
    }
}