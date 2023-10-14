namespace Code.GlobalGameState
{
    public class GameStateManager : IOnController, IOnStart
    {

        private enum GameState
        {
            None   = 0,
            Menu,
            Peace,
            Battle
        }

        private PeaceStateManager _peaceState;
        private BattleStateManager _battleState;

        private GameState _state;
        

        public GameStateManager(PeaceStateManager peace, BattleStateManager battle)
        {
            _peaceState = peace;
            _battleState = battle;
        }


        public void OnStart()
        {
            
        }
        
        
        
        
        
    }
}