using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code.UI
{
    public class CenterUI : MonoBehaviour
    {
        [SerializeField] private GameObject _buildingBuy;

        public GameObject BuildingBuy => _buildingBuy;
    }
}

