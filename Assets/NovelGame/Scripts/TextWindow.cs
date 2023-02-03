using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextWindow : MonoBehaviour
{
    [SerializeField] TMP_Text _text;
    [SerializeField] string[] _texts;
    [SerializeField] float _drawSpeed = 0.5f;
    [SerializeField] float _colorChangeSpeed = 0.5f;
    [SerializeField] bool _colorChange = false;

    int _count;
    bool _isDrawText;

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            DrawText();
        }
    }
    void DrawText()
    {
        if (_isDrawText) return;

        if (_texts.Length <= _count) return;

        Debug.Log("スタート");
        StartCoroutine(DrawTextCorutine());
    }
    /// <summary>
    /// 非同期でテキストを順次表示させる
    /// </summary>
    /// <returns></returns>
    IEnumerator DrawTextCorutine()
    {
        _isDrawText = true;
        float time = 0;
        var interval = _drawSpeed / _texts[_count].Length;
        int len = 0;

        var color = Color.white;

        if (_colorChange)
        {
            color.a = 0;
            _text.color = color;
            //StartCoroutine(ColorChange(len, interval));
        }
        else
        {
            _text.color = color;
        }

        while (true)
        {
            yield return null;

            time += Time.deltaTime;

            if(time >= interval)
            {
                time = 0;
                len++;

                if (len > _texts[_count].Length)
                    break;

                _text.text = _texts[_count].Substring(0, len);

                //if (_colorChange)
                    //StartCoroutine(ColorChange(len, interval));
            }

            if(Input.GetButtonDown("Fire1"))
            {
                break;
            }
        }

        _isDrawText = false;
        _text.text = _texts[_count];

        _count++;
        Debug.Log("終了");
    }
    IEnumerator ColorChange(int num, float t)
    {
        //一文字づつのフェードはうまくいっていない(途中)
        
        float time = 0;
        int alpha = 0;
        float interval = t / 255;

        _text.ForceMeshUpdate();

        while (true)
        {
            yield return null;

            time += Time.deltaTime;

            if(time >= interval)
            {

                time = 0;
                alpha++;
            }

            VertexColors(num, (byte)alpha);

            if (alpha >= 255)
            {
                //Debug.Log($"{num}");
                break;
            }

            if(Input.GetButtonDown("Fire1"))
            {
                break;
            }
        }

        VertexColors(num, 255);
    }
    void VertexColors(int num, byte alpha)
    {
        var info = _text.textInfo;

        Color32[] newVertexColors;
        Color32 c0 = _text.color;

        int materialIndex = info.characterInfo[num].materialReferenceIndex;
        newVertexColors = info.meshInfo[materialIndex].colors32;

        int vertexIndex = info.characterInfo[num].vertexIndex;

        c0.a = alpha;

        newVertexColors[vertexIndex + 0] = c0;
        newVertexColors[vertexIndex + 1] = c0;
        newVertexColors[vertexIndex + 2] = c0;
        newVertexColors[vertexIndex + 3] = c0;

        _text.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }
}
