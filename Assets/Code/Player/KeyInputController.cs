using System;
using UnityEngine;


namespace Code.Player
{
    public class KeyInputController : IOnController, IOnUpdate
    {

        public event Action OnCancelAxisClick; 
        
        
        public void OnUpdate(float deltaTime)
        {

            if (Input.GetButtonDown("Cancel"))
            {
                OnCancelAxisClick?.Invoke();
            }
            
        }

    }
}