using ResourceSystem;

namespace UI
{
    public class ResPanelController
    {
        private readonly ResPanel _panel;
        public ResPanelController(ResPanelFactory factory)
        {
            _panel = factory.GetView(factory.UIElementsConfig.ResPanel);
        }

        public void UpdateResursesCount(ResourceType type, int value)
        {
            
        }
    }
}