namespace Code.GlobalGameState
{
    public partial class GameStateManager : IOnController, IOnStart
    {
        private PeacePhaseConttoller _peacePhase;
        private BattlePhaseController _battlePhase;

        private GameState _state;
        
        public GameStateManager(PeacePhaseConttoller peacePhase, BattlePhaseController battlePhase)
        {
            _peacePhase = peacePhase;
            _battlePhase = battlePhase;
        }

        public void OnStart()
        {
            _battlePhase.StartPhase();
        }   
    }
}