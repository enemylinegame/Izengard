using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "OutpostParametersData", menuName = "OutPost/OutpostParametersData", order = 0)]
    public class OutpostParametersData: ScriptableObject
    {
        
        #region Fields

        [SerializeField] private int _maxCountOfNPC;

        #endregion


        #region Methods

        public void AddMaxCountOfNPC(int number)
        {
            _maxCountOfNPC += number;
        }
        public void SetMaxCountOfWorkers(int number)
        {
            _maxCountOfNPC = number;
        }
        
        public int GetMaxCountOfNPC()
        {
            return _maxCountOfNPC;
        }
        
        #endregion

    }
}