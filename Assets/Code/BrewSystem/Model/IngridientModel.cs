

namespace BrewSystem.Model
{
    public class IngridientModel
    {
        private readonly int _id;
        private readonly IIngridienData _data;

        public int Id => _id;
        public IIngridienData Data => _data;

        public IngridientModel(int id, IIngridienData data) 
        {
            _id = id;
            _data = data;
        }
            
    }
}
