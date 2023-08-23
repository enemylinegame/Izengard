using System;
using Code.UI;

public class BtnUIController : IOnController, IOnStart, IDisposable
{
    private RightPanelController _rightUI;
    public event Action<int> TileSelected;
    
    public BtnUIController(RightPanelController controller, GameConfig gameConfig)
    {
        _rightUI = controller;
        controller.StartSpawnTiles(gameConfig);
    }

    public void OnStart()
    {
        _rightUI.SubscribeTileSelButtons();
        //TileSelected = _rightUI.TileSelected;
    }


    public void Dispose()
    {
      _rightUI.UnSubscribeTileSelButtons();
    }
}
