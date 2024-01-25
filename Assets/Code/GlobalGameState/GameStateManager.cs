namespace Code.GlobalGameState
{
    public partial class GameStateManager : IOnController, IOnStart, IOnUpdate, IOnFixedUpdate
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

        public void OnUpdate(float deltaTime)
        {
            _battlePhase.OnUpdate(deltaTime);
        }

        public void OnFixedUpdate(float fixedDeltaTime)
        {
            _battlePhase.OnFixedUpdate(fixedDeltaTime);
        }

    }
}