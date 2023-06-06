using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class LoadContainersWithResources : MonoBehaviour
{
    
    [SerializeField] private GameObject _prefabContainer;
    [SerializeField] private GameObject _collectorOfResource;

    
    public List<GameObject> _listOfMinerals=new List<GameObject>();

    public void SetResources(List<GameObject> InputList) => _listOfMinerals = InputList;
    
    void Start()
    {
        for (var index = 0; index < _listOfMinerals.Count; index++)
        {
            Instantiate(_prefabContainer);
            ImageChangeInContainer _imageChangeInContainer = _prefabContainer.GetComponent<ImageChangeInContainer>();
            _imageChangeInContainer.MineTypeCheck(_listOfMinerals, index);
        }
       
    }
}
