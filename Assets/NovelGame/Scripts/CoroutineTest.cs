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

            // X���̉�]��2�b��
            yield return RotateAsync(Vector3.right, 2);

            // 1�b�ԑҋ@����
            //yield return WaitClick();

            // Y���̉�]��2�b��
            yield return RotateAsync(Vector3.up, 2);

            // 1�b�ԑҋ@����
            yield return AsyncUserActionOrSec(1, KeyCode.Space);

            // Z���̉�]��2�b��
            yield return RotateAsync(Vector3.forward, 2);

            // 1�b�ԑҋ@����
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

    //�x�^�Ȃ���
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