using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeSystem : MonoBehaviour
{
    /// <summary>
    /// �t�F�[�h���n�܂�O�ɋN����f���Q�[�g
    /// </summary>
    public delegate void OnBeforeFade();
    OnBeforeFade _onBefore;

    /// <summary>
    /// �t�F�[�h���I�������ɋN����f���Q�[�g
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
    /// �t�F�[�h���n�܂�O�ɋN���������C�x���g��o�^
    /// </summary>
    /// <param name="d"></param>
    public void SetupDelegate(OnBeforeFade d)
    {
        _onBefore = d;
    }
    /// <summary>
    /// �t�F�[�h���I�������ɋN���������C�x���g��o�^
    /// </summary>
    /// <param name="d"></param>
    public void SetupDelegate(OnAfterFade d)
    {
        _onAfter = d;
    }

    /// <summary>
    /// �P��̃t�F�[�h
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
    /// �N���X�t�F�[�h
    /// </summary>
    /// <param name="fadeOutImage"></param>
    /// <param name="fadeInImage"></param>
    /// <param name="time"></param>
    public void OnFade(Image fadeOutImage, Image fadeInImage, float time)
    {
        if(_isFade) return;

        _fadeOutImage = fadeOutImage;
        _fadeInImage = fadeInImage;
        _fadeTime = time;
        _isFade = true;

        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        float timer = 0;
        float testTimer = 0;
        float interval = _fadeTime / 255;

        Debug.Log($"Fade�J�n : {interval}");

        //�J�n�O�������o�^����Ă�������s
        _onBefore?.Invoke();

        while(true)
        {
            yield return null;

            timer += Time.deltaTime;
            testTimer += Time.deltaTime; 

            if(interval <= timer)
            {
                ImageColorFade(_fadeOutImage, timer);
                timer = 0;
            }

            if(_fadeOutImage.color.a <= 0)
            {
                Debug.Log($"Fade�I�� : {testTimer}");
                break;
            }
        }

        //�I�����������o�^����Ă�������s
        _onAfter?.Invoke();

        _isFade = false;
    }

    void ImageColorFade(Image image, float time)
    {
        var c = image.color;
        var alpha = c.a - time;
        c.a = alpha;
        image.color = c;
    }

    /// <summary>
    /// �X�L�b�v����
    /// </summary>
    public void Skip()
    {

    }

    [SerializeField] Image _testImage;
    [SerializeField] float _testTime = 2f;

    public void Test()
    {
        OnFade(_testImage, _testTime);
    }
}
