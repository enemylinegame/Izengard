using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageChangeInContainer : MonoBehaviour
{
    [SerializeField] private Image _spriteOfResource;
    
    [SerializeField] private Image _forestIcon;
    [SerializeField] private Image _textileIcon;
    [SerializeField] private Image _deersIcon;
    [SerializeField] private Image _horseIcon;
    [SerializeField] private Image _ironIcon;
    [SerializeField] private Image _steelIcon;
    [SerializeField] private Image _magicStonesIcon;
    
    public  void MineTypeCheck(List<GameObject> _listOfMinerals,int index)
    {
        var minerals = _listOfMinerals[index];
        Mineral mineral = minerals.GetComponent<Mineral>();
        /*switch (mineral.ThisResourceMine.TypeOfMine)
        {

            case 601:
                _spriteOfResource = _forestIcon;
                break;
            case 602:
                _spriteOfResource = _textileIcon;
                break;
            case 603:
                _spriteOfResource = _deersIcon;
                break;
            case 604:
                _spriteOfResource = _horseIcon;
                break;
            case 605:
                _spriteOfResource = _ironIcon;
                break;
            case 606:
                _spriteOfResource = _steelIcon;
                break;
            case 607:
                _spriteOfResource = _magicStonesIcon;
                break;

        }*/
    }

}
