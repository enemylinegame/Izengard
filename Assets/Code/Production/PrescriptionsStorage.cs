using ResourceSystem;
using UnityEngine;


[CreateAssetMenu(fileName = "Prescriptions", menuName = "Prescriptions")]
public class PrescriptionsStorage : ScriptableObject
{
    [SerializeField]
    private Prescription[] _prescriptions;

    internal Prescription GetPrescription(ResourceType resource) =>
        System.Array.Find(_prescriptions, x => resource == x.TargetResource);
}
