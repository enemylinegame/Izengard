using Abstraction;
using System.Collections.Generic;

namespace Tools
{
    public class PauseController : IOnController
    {
        private readonly List<IPaused> _pausedObjects;

        public PauseController()
        {
            _pausedObjects = new();
        }

        public void Pause()
        {
            for(int i = 0; i < _pausedObjects.Count; i++)
            {
                var pauseObj = _pausedObjects[i];
                pauseObj.OnPause();
            }
        }
        
        public void Release()
        {
            for (int i = 0; i < _pausedObjects.Count; i++)
            {
                var pauseObj = _pausedObjects[i];
                pauseObj.OnRelease();
            }
        }


        public void Add(IPaused pausedObject)
        {
            if (pausedObject == null)
                return;

            _pausedObjects.Add(pausedObject);
        }

        public void Remove(IPaused pausedObject)
        {
            if (pausedObject == null)
                return;
            if (_pausedObjects.Count == 0)
                return;
 
            _pausedObjects.Remove(pausedObject);
        }

        public void Clear()
        {
            _pausedObjects.Clear();
        }
    }
}
