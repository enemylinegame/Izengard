using ResourceSystem;
using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Recipes", menuName = "Recipes")]
public class RecipesStorage : ScriptableObject
{
    [SerializeField]
    private Recipe[] _recipes;

    internal Recipe GetPrescription(ResourceType resource) =>
        System.Array.Find(_recipes, x => resource == x.TargetResource);
}
