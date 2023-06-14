using System.Collections;
using UnityEngine;
public sealed class Coroutines : MonoBehaviour
{
    private static Coroutines _instance;
    private static Coroutines Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject("[Coroutine manager]");
                _instance = go.AddComponent<Coroutines>();
            }

            return _instance;
        }
    }
    
    public static Coroutine StarRoutine(IEnumerator enumerator)
    {
        return Instance.StartCoroutine(enumerator);
    }
    
    public static void StopRoutine (Coroutine routine)
    {
        if (routine != null)
        {
            Instance.StopCoroutine(routine);
        }
    }
}