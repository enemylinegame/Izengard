using TMPro;
using Wave.Interfaces;


namespace Wave
{
    public class TimerCountUI : ITimeCountShower
    {
        private readonly TMP_Text _text;


        public TimerCountUI(TMP_Text text)
        {
            _text = text;
        }

        public void TimeCountShow(float time)
        {
            _text.text = time.ToString();
        }
    }
}