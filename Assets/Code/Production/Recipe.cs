using ResourceSystem;
using System;

[Serializable]
public struct Recipe
{
    public ResourceType TargetResource;
    public int ResultAmount;
    public RecipeComponent[] Components;
}