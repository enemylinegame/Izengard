namespace Code.UI.Controllers
{
    public class UIController
    {
        private LeftUI _leftUI;
        private RightUI _rightUI;
        private BottonUI _bottonUI;
        private CenterUI _centerUI;
        
        public UIController(LeftUI leftUI, RightUI rightUI, BottonUI bottonUI, CenterUI centerUI)
        {
            _leftUI = leftUI;
            _rightUI = rightUI;
            _bottonUI = bottonUI;
            _centerUI = centerUI;
            
            IsOffUI(UIType.All);
        }

        public void IsOnUI(UIType type)
        {
            
        }
        public void IsOffUI(UIType type)
        {
            
        }
    }

    public enum UIType
    {
        All,
        Buy,
        Сonfirmation,
        Tile,
        Unit,
    }
}