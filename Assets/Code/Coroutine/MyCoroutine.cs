using System.Collections;
using UnityEngine;

public delegate void EndTimer();

public class MyCoroutine
{
    public event EndTimer End;
    private Coroutine _coroutine;
    public void StartMyCoroutine(float i)
    {
        if (_coroutine != null)
        {
            return;
        }
        _coroutine = Coroutines.StarRoutine(MyTimer(i));
    }
    
    public void StopMyCoroutine()
    {
        Coroutines.StopRoutine(_coroutine);
        _coroutine = null;
    }
    
    private IEnumerator MyTimer(float i)
    {
        while (true)
        {
            yield return new WaitForSeconds(i);
            End?.Invoke(); 
        }
    }
}
    