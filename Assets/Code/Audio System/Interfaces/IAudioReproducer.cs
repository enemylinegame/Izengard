namespace Audio_System
{
    public interface IAudioReproducer
    {
        ISound SoundData { get; }
        ISoundSource SoundSourceData { get; }
    }
}
