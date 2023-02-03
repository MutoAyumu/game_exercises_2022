using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineTest : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(ExecuteAsync());
    }

    private IEnumerator ExecuteAsync()
    {
        Debug.Log("ExecuteAsync: Begin");

        while (true)
        {
            var t = 0F;

            // XŽ²‚Ì‰ñ“]‚ð2•bŠÔ
            yield return RotateAsync(Vector3.right, 2);

            // 1•bŠÔ‘Ò‹@‚·‚é
            //yield return WaitClick();

            // YŽ²‚Ì‰ñ“]‚ð2•bŠÔ
            yield return RotateAsync(Vector3.up, 2);

            // 1•bŠÔ‘Ò‹@‚·‚é
            yield return AsyncUserActionOrSec(1, KeyCode.Space);

            // ZŽ²‚Ì‰ñ“]‚ð2•bŠÔ
            yield return RotateAsync(Vector3.forward, 2);

            // 1•bŠÔ‘Ò‹@‚·‚é
            yield return AsyncUserActionOrSec(1, KeyCode.Space);
        }

        // Debug.Log("ExecuteAsync: End");
    }

    private IEnumerator RotateAsync(Vector3 eulers, float duration)
    {
        Debug.Log($"RotateAsync: Begin eulers={eulers}, duration={duration}");
        var t = 0F;
        while (t < duration)
        {
            t += Time.deltaTime;
            transform.Rotate(eulers);
            yield return null;
        }
        Debug.Log("ExecuteAsync: End");
    }

    //ƒxƒ^‚È‚â‚è•û
    private IEnumerator AsyncUserActionOrSec(float duration, KeyCode code)
    {
        Debug.Log($"AsyncUserActionOrSec: duration = {duration}");
        var t = 0f;

        while(t < duration)
        {
            if(Input.GetKeyDown(code))
            {
                yield break;
            }

            t += Time.deltaTime;
            yield return null;
        }
        Debug.Log("ExecuteAsync : End");
    }
    
    //private IEnumerator WaitClick()
    //{
    //    var click = StartCoroutine(WaitForClick());
    //    StartCoroutine(WhenAny(new WaitForSeconds(1), click));
    //}
    
    //private IEnumerator WhenAny(YieldInstruction seconds,YieldInstruction click)
    //{
        
    //}
    //IEnumerator WaitForClick()
    //{
    //    while (!Input.GetMouseButtonDown(0)) { yield return null; }
    //}
}