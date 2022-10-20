using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextWindow : MonoBehaviour
{
    [SerializeField] TMP_Text _text;
    [SerializeField] string[] _texts;
    [SerializeField] float _speed = 0.1f;

    int _count;

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            DrawText();
        }
    }
    void DrawText()
    {
        if (_texts.Length <= _count) return;

        Debug.Log("�X�^�[�g");
        StartCoroutine(DrawTextCorutine());
    }
    /// <summary>
    /// �񓯊��Ńe�L�X�g�������\��������
    /// </summary>
    /// <returns></returns>
    IEnumerator DrawTextCorutine()
    {
        float time = 0;

        while(true)
        {
            time += Time.deltaTime;

            //�ꉞ��O
            if(_speed <= 0)
            {
                _speed = 0.01f;
            }


            var length = (int)(time / _speed);

            if (length > _texts[_count].Length)
                break;

            _text.text = _texts[_count].Substring(0, length);

            yield return null;
        }

        _count++;
        Debug.Log("�I��");
    }
}
