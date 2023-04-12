using UnityEngine;
using Wave.Interfaces;
using Wave.Settings;


namespace Wave
{
    public class WaveCalculator : IWaveCalculator
    {
        private readonly WaveSettings _waveSettings;


        public WaveCalculator()
        {
            _waveSettings = Resources.Load<WaveSettings>(nameof(WaveSettings));
        }

        public float GetWaveCost(int waveNumber, bool isDowntime)
        {
            return _waveSettings.WaveBaseCost + Mathf.Pow(_waveSettings.WaveBaseCost * (waveNumber - 1), _waveSettings.Ñoefficient);
        }
    }
}