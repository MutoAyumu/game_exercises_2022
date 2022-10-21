using TMPro;
using UnityEngine;

public class MessagePrinter : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _textUi = default;

    [SerializeField]
    private string _message = "";

    [SerializeField]
    private float _speed = 1.0F;

    private float _elapsed = 0; // ������\�����Ă���̌o�ߎ���
    private float _interval; // �������̑҂�����

    // _message �t�B�[���h����\�����錻�݂̕����C���f�b�N�X�B
    // �����w���Ă��Ȃ��ꍇ�� -1 �Ƃ���B
    private int _currentIndex = -1;

    /// <summary>
    /// �����o�͒����ǂ����B
    /// </summary>
    public bool IsPrinting
    {
        get
        {
            // TODO: �����ɃR�[�h������
            if (_currentIndex == _message.Length - 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    private void Start()
    {
        ShowMessage(_message);
    }

    private void Update()
    {
        if (_textUi is null || _message is null || _currentIndex + 1 >= _message.Length) { return; }

        _elapsed += Time.deltaTime;
        if (_elapsed > _interval)
        {
            _elapsed = 0;
            _currentIndex++;
            _textUi.text += _message[_currentIndex];
        }

        ColorChange();
    }

    /// <summary>
    /// �w��̃��b�Z�[�W��\������B
    /// </summary>
    /// <param name="message">�e�L�X�g�Ƃ��ĕ\�����郁�b�Z�[�W�B</param>
    public void ShowMessage(string message)
    {
        // TODO: �����ɃR�[�h������
        _textUi.text = "";
        _interval = _speed / message.Length;
        _currentIndex = -1;
        _message = message;
    }

    /// <summary>
    /// ���ݍĐ����̕����o�͂��ȗ�����B
    /// </summary>
    public void Skip()
    {
        // TODO: �����ɃR�[�h������
        _textUi.text = _message;
        _currentIndex = _message.Length - 1;
    }

    void ColorChange()
    {
        _textUi.ForceMeshUpdate();

        TMP_TextInfo textInfo = _textUi.textInfo;

        Color32[] newVertexColors;
        Color32 c = _textUi.color;

        for (int i = 0; i < _textUi.text.Length; i++)
        {
            int characterCount = textInfo.characterCount;

            if (characterCount == 0)
            {
                return;
            }

            int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;

            newVertexColors = textInfo.meshInfo[materialIndex].colors32;

            int vertexIndex = textInfo.characterInfo[i].vertexIndex;

            for (int j = 0; j < 4; j++)
            {
                var alpha = newVertexColors[vertexIndex + j].a;
                alpha++;
                c = new Color32(c.r, c.g, c.b, alpha);
                newVertexColors[vertexIndex + j] = c;
            }

            _textUi.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

            //i = (i + 1) % characterCount;
        }
    }
}