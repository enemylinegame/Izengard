using System;

namespace UserInputSystem
{

    public class UserInputController: IDisposable
    {
        public UserInput UserInput { get; private set; }

        public UserInputController()
        {
            UserInput = new UserInput();
            UserInput.Enable();
        }

        public void Dispose()
        {
            UserInput.Disable();
        }
    }
}