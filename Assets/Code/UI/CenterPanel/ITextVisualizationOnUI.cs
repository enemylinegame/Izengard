using System;
using System.Threading.Tasks;

namespace Code.UI
{
    public interface ITextVisualizationOnUI
    {
        Task BasicTemporaryUIVisualization(String text, int time);
        Task SecondaryTemporaryUINotification(String text, int time);
        void BasicUIVisualization(String text, bool IsOn);
        void SecondaryUINotification(String text, bool IsOn);
    }
}