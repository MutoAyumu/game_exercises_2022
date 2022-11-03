using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeSystem : MonoBehaviour
{
    /// <summary>
    /// フェードが始まる前に起きるデリゲート
    /// </summary>
    public delegate void OnBeforeFade();
    OnBeforeFade _onBefore;

    /// <summary>
    /// フェードが終わった後に起きるデリゲート
    /// </summary>
    public delegate void OnAfterFade();
    OnAfterFade _onAfter;

    Image _fadeOutImage;
    Image _fadeInImage;
    float _fadeTime;

    bool _isFade;

    public bool IsFade
    {
        get
        {
            return _isFade;
        }
        set
        {
            _isFade = value;
        }
    }

    /// <summary>
    /// フェードが始まる前に起こしたいイベントを登録
    /// </summary>
    /// <param name="d"></param>
    public void SetupDelegate(OnBeforeFade d)
    {
        _onBefore = d;
    }
    /// <summary>
    /// フェードが終わった後に起こしたいイベントを登録
    /// </summary>
    /// <param name="d"></param>
    public void SetupDelegate(OnAfterFade d)
    {
        _onAfter = d;
    }

    /// <summary>
    /// 単一のフェード
    /// </summary>
    /// <param name="fadeOutImage"></param>
    public void OnFade(Image fadeOutImage, float time)
    {
        if (_isFade) return;

        _fadeOutImage = fadeOutImage;
        _fadeTime = time;
        _isFade = true;

        StartCoroutine(Fade());
    }
    /// <summary>
    /// クロスフェード
    /// </summary>
    /// <param name="fadeOutImage"></param>
    /// <param name="fadeInImage"></param>
    /// <param name="time"></param>
    public void OnFade(Image fadeOutImage, Image fadeInImage, float time)
    {
        if (_isFade) return;

        _fadeOutImage = fadeOutImage;
        _fadeInImage = fadeInImage;
        _fadeTime = time;
        _isFade = true;

        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        float interval = _fadeTime / 255f;
        float alpha = 1f / 255f;
        float value = 0;

        Debug.Log($"Fade開始");

        //開始前処理が登録されていたら実行
        _onBefore?.Invoke();

        //開始前処理が終わるまで待機したい
        yield return new WaitUntil(() => _onBefore == null);

        ImageColorFade(_fadeOutImage, 1);

        if (_fadeInImage)
            ImageColorFade(_fadeInImage, 0);

        while (true)
        {
            yield return new WaitForSeconds(interval);

            value += alpha;

            //イメージの透明度を変更
            ImageColorFade(_fadeOutImage, 1 - value);

            if(_fadeInImage)
                ImageColorFade(_fadeInImage, value);

            //スキップ
            if (Skip())
            {
                break;
            }

            //終了
            if (_fadeOutImage.color.a <= 0)
            {
                break;
            }
        }

        Debug.Log($"Fade終了");

        ImageColorFade(_fadeOutImage, 0);

        if (_fadeInImage)
            ImageColorFade(_fadeInImage, 1);

        //終了時処理が登録されていたら実行
        _onAfter?.Invoke();

        _isFade = false;
    }

    void ImageColorFade(Image image, float alpha)
    {
        var c = image.color;
        c.a = alpha;
        image.color = c;
    }

    /// <summary>
    /// スキップ処理
    /// </summary>
    public bool Skip()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            return true;
        }

        return false;
    }

    #region テスト

    [SerializeField] Image _testImage1;
    [SerializeField] Image _testImage2;
    [SerializeField] float _testTime = 2f;

    public void SingleTest()
    {
        OnFade(_testImage1, _testTime);
    }

    public void CrossTest()
    {
        OnFade(_testImage1, _testImage2, _testTime);
    }
    #endregion
}
