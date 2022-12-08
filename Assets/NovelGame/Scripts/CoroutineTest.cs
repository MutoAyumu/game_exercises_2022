using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineTest : MonoBehaviour
{
    IEnumerator _coroutine;

    private void Start()
    {
        _coroutine = Test();
    }

    IEnumerator Test()
    {
        Debug.Log("Start");
        yield return null;

        Debug.Log("End");
        yield break;
    }

    private void Update()
    {
        if(_coroutine != null)
        {
            var result = _coroutine.MoveNext();

            if(result)
            {
                Debug.Log("Next");
            }    
            else
            {
                _coroutine = null;
            }
        }
    }
}
