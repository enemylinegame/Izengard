namespace Audio_System
{
    public interface IAudioReproducer
    {
        IAudio SoundData { get; }
        IAudioSource SoundSourceData { get; }
    }
}
