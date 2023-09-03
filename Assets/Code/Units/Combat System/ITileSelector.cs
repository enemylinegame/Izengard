using Code.TileSystem;


namespace Interfaces
{
    public interface ITileSelector
    {
        void Cancel();
        void SelectTile(TileView tile, TileModel model);
    }
}
