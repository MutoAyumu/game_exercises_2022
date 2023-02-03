using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorTransitioner : MonoBehaviour
{
    //背景画像
    [SerializeField] Image _fadeImage;
    [SerializeField] Image _backImage;
    //遷移後の色
    [SerializeField] Color _toColor;
    //遷移する時間(s)
    [SerializeField] float _duration = 1;

    Color _fromColor;
    Sprite _toBackSprite;
    float _elapsed = 0;

    public bool IsCompleted => _fadeImage is null? false : _fadeImage.color != _toColor;

    private void Start()
    {
        if (_fadeImage is null) return;

        _fromColor = _fadeImage.color;
    }
    private void Update()
    {
        _elapsed += Time.deltaTime;

        if (_elapsed < _duration)
        {
            _fadeImage.color = Color.Lerp(_fromColor, _toColor, _elapsed / _duration);
        }
        else
        {
            _fadeImage.color = _toColor;

            if (_toBackSprite is null) return;

            _backImage.sprite = _toBackSprite;
            _toBackSprite = null;
        }
    }

    /// <summary>
    /// フェード処理を開始する
    /// </summary>
    /// <param name="c"></param>
    public void Play(Color c, Sprite nextSprite)
    {
        if(_fadeImage is null) return;

        _fromColor = _fadeImage.color;
        _toColor = c;
        _toBackSprite = nextSprite;
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
