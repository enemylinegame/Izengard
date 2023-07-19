using ResourceSystem;
using System;

[Serializable]
public struct Prescription
{
    public ResourceType TargetResource;
    public int ResultAmount;
    public PrescriptionComponent[] Components;
}