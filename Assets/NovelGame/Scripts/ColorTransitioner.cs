using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorTransitioner : MonoBehaviour
{
    //背景画像
    [SerializeField] Image _image;
    //遷移後の色
    [SerializeField] Color _toColor;
    //遷移する時間(s)
    [SerializeField] float _duration = 1;

    Color _fromColor;
    float _elapsed = 0;

    public bool IsCompleted => _image is null? false : _image.color == _toColor;

    private void Start()
    {
        if (_image is null) return;

        _fromColor = _image.color;
    }
    private void Update()
    {
        _elapsed += Time.deltaTime;

        if (_elapsed < _duration)
        {
            _image.color = Color.Lerp(_fromColor, _toColor, _elapsed / _duration);
        }
        else
        {
            _image.color = _toColor;
        }
    }

    /// <summary>
    /// フェード処理を開始する
    /// </summary>
    /// <param name="c"></param>
    public void Play(Color c)
    {
        if(_image is null) return;

        _fromColor = _image.color;
        _toColor = c;
        _elapsed = 0;
    }

    /// <summary>
    /// フェード処理の破棄
    /// </summary>
    public void Skip()
    {
        _elapsed = _duration;
    }
}
