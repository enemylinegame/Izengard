using System;

namespace StartupMenu
{
    public class MethodExtension<T>
    {
        private Action<T> _callBackAction;

        public MethodExtension(Action<T> callBackAction)
        {
            _callBackAction = callBackAction;
        }

        public void OnChange(object sender) 
            => _callBackAction?.Invoke((T)sender);
    }
}
