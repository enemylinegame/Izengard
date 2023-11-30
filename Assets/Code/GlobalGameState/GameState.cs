namespace Code.GlobalGameState
{
    public partial class GameStateManager
    {
        private enum GameState
        {
            None            = 0,
            Menu            = 1,
            PeacePhase      = 2,
            BattlePhase     = 3,
        }
    }
}