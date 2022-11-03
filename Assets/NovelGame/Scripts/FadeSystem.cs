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

        Debug.Log($"Fade�J�n");

        //�J�n�O�������o�^����Ă�������s
        _onBefore?.Invoke();

        //�J�n�O�������I���܂őҋ@������
        yield return new WaitUntil(() => _onBefore == null);

        ImageColorFade(_fadeOutImage, 1);

        if (_fadeInImage)
            ImageColorFade(_fadeInImage, 0);

        while (true)
        {
            yield return new WaitForSeconds(interval);

            value += alpha;

            //�C���[�W�̓����x��ύX
            ImageColorFade(_fadeOutImage, 1 - value);

            if(_fadeInImage)
                ImageColorFade(_fadeInImage, value);

            //�X�L�b�v
            if (Skip())
            {
                break;
            }

            //�I��
            if (_fadeOutImage.color.a <= 0)
            {
                break;
            }
        }

        Debug.Log($"Fade�I��");

        ImageColorFade(_fadeOutImage, 0);

        if (_fadeInImage)
            ImageColorFade(_fadeInImage, 1);

        //�I�����������o�^����Ă�������s
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
    /// �X�L�b�v����
    /// </summary>
    public bool Skip()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            return true;
        }

        return false;
    }

    #region �e�X�g

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
