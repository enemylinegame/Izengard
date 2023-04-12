using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResurseSystem
{ 

    public interface IResurseCost 
    {
        public List<ResurseHolder> CostInResurse { get; }
        public bool PricePaidFlag { get; }

        public void CheckRequiredResurses();
        public ResurseHolder AddResurse(ResurseHolder holder);
        public void GetNeededResurse(ResurseStock stock);
        public void ResetPaid();

    }
}
