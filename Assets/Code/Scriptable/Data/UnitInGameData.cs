using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "UnitInGameData", menuName = "Units/UnitInGameData", order = 0)]
    public class UnitInGameData: ScriptableObject
    {
        #region Fields

        [SerializeField] private int _maxCountOfMilitary;
        [SerializeField] private int _maxCountOfWorkers;
        [SerializeField] private int _currentCountOfMilitary;
        [SerializeField] private int _currentCountOfWorkers;

        #endregion

        
        #region Methods

        public void AddSomeToMaxM(int number)
        {
            _maxCountOfMilitary += number;
        }

        public void AddSomeToMaxW(int number)
        {
            _maxCountOfWorkers += number;
        }
        
        public void AddSomeToCurW(int number)
        {
            _currentCountOfMilitary += number;
        }
        
        public void AddSomeToCurM(int number)
        {
            _currentCountOfWorkers += number;
        }
        
        public int GetMaxM()
        {
            return _maxCountOfMilitary;
        }

        public int GetMaxW()
        {
            return _maxCountOfWorkers;
        }

        public int GetCurM()
        {
            return _currentCountOfMilitary;
        }
        
        public int GetCurW()
        {
            return _currentCountOfWorkers;
        }
        
        #endregion
        
    }
}