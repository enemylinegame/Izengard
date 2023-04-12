using UnityEngine;
using System;
using ResurseSystem;

public class Mineral : BaseBuildAndResources
{
    public ResurseMine ThisResurseMine => _thisResurseMine;
    [SerializeField]
    private Sprite mineralIcon;
    [Header("Mine Config")]
    [SerializeField]
    private ResurseMine _thisResurseMine;
    private void Awake()
    {
        if (_thisResurseMine==null)
        {
            throw new Exception($"Not implement mine model in {gameObject}");
        }
        _thisResurseMine.SetIconMine(mineralIcon);
    }
    public void SetModelOfMine(ResurseMine mine)
    {
        _thisResurseMine=new ResurseMine(mine);
    }
}