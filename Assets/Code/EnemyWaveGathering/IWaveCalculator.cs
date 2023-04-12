using System.Collections;
namespace Wave.Interfaces
{
    public interface IWaveCalculator
    {
        float GetWaveCost(int waveNumber, bool isDowntime);
    }
}