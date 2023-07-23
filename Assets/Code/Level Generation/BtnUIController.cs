using System;
using Code.UI;

public class BtnUIController : IOnController, IOnStart, IDisposable
{
    private RightUI _rightUI;
    public event Action<int> TileSelected;
    
    public BtnUIController(RightUI rightUI, GameConfig gameConfig)
    {
        _rightUI = rightUI;
        _rightUI.ButtonSelectTileFirst.image.sprite = gameConfig.FirstTile.IconTile;
        _rightUI.ButtonSelectTileSecond.image.sprite = gameConfig.SecondTile.IconTile;
        _rightUI.ButtonSelectTileThird.image.sprite = gameConfig.ThirdTile.IconTile;
    }

    public void OnStart()
    {
        _rightUI.ButtonSelectTileFirst.onClick.AddListener(() => TileSelected?.Invoke(0));
        _rightUI.ButtonSelectTileSecond.onClick.AddListener(() => TileSelected?.Invoke(1));
        _rightUI.ButtonSelectTileThird.onClick.AddListener(() => TileSelected?.Invoke(2));
    }


    public void Dispose()
    {
      _rightUI.ButtonSelectTileFirst.onClick.RemoveAllListeners();
      _rightUI.ButtonSelectTileSecond.onClick.RemoveAllListeners();
      _rightUI.ButtonSelectTileThird.onClick.RemoveAllListeners();
    }
}
