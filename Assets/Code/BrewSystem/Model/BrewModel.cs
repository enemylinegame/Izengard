namespace BrewSystem.Model
{
    public class BrewModel
    {
        public float ABV { get; set; }
        public float Taste { get; set; }
        public float Flavor { get; set; }


        public void Reset()
        {
            ABV = 0f;
            Taste = 0f;
            Flavor = 0f;
        }
    }
}
