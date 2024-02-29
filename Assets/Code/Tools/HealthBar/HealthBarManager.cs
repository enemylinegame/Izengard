using System.Collections.Generic;

namespace Tools
{
    public class HealthBarManager:IOnController, IOnUpdate
    {
        private readonly List<HealthBarController> _controllers;

        public HealthBarManager() 
        {
            _controllers = new List<HealthBarController>();
        }

        public void Add(IHealthBarHandeled handeler)
        {
            var controller = handeler.HealthBarController;
            
            controller.Enable();

            _controllers.Add(controller);
        }

        public void Remove(IHealthBarHandeled handeler)
        {
            var controller = handeler.HealthBarController;

            controller.Disable();

            _controllers.Remove(controller);
        }

        public void OnUpdate(float deltaTime)
        {
            for(int i =0; i< _controllers.Count; i++)
            {
                _controllers[i].OnUpdate(deltaTime);
            }
        }
    }
}
