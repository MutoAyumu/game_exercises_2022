using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    /*MEMO
    �i�������j
    �������S�ĕ\�������܂œ_�ł��������ɂȂ�
    Update�łԂ�񂵂Ă邩��K���ɏ�����t���Ď~�߂�悤�ɂ���
    List�Ƃ��Ŗ���������Ă銴�����邩�牽�Ƃ�������
     */

    [SerializeField] private TMP_Text _textUi = default;
    [SerializeField] bool _isChange = true;

    List<byte> _alphaList = new List<byte>();
    int _characterCount = 0;

    private void Awake()
    {
        if (!_textUi) return;

        var color = Color.white;

        if (_isChange)
        {
            color.a = 0;
        }

        _textUi.color = color;
    }

    private void Update()
    {
        if (!_isChange) return;

        ChangeColor();
    }

    public void ClearList()
    {
        _characterCount = 0;
        _alphaList.Clear();
    }

    void ChangeColor()
    {
        //�e�L�X�g �I�u�W�F�N�g�Ɋ܂܂�邷�ׂĂ̗v�f���擾
        TMP_TextInfo textInfo = _textUi.textInfo;

        //������������Ζ߂�
        if (textInfo.characterCount == 0) return;

        _textUi.ForceMeshUpdate();

        //���_�̐F�����Ă�������
        Color32[] newVertexColors;
        Color32 c = _textUi.color;

        //���������X�V���ꂽ��A���t�@�l�����Ă������X�g��ǉ�
        if (textInfo.characterCount != _characterCount)
        {
            _characterCount = textInfo.characterCount;
            _alphaList.Add(0);
        }

        //�����������[�v
        for (int i = 0; i < _alphaList.Count; i++)
        {
            //TextInfo�g���Ă���}�e���A����i�Ԗڂ̃C���f�b�N�X���擾
            int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
            //���b�V���̒��_�̐F���擾
            newVertexColors = textInfo.meshInfo[materialIndex].colors32;
            //TextInfo�Ŏg����ŏ��̒��_�̃C���f�b�N�X���擾
            int vertexIndex = textInfo.characterInfo[i].vertexIndex;

            //�A���t�@�l�𑝂₷
            var alpha = _alphaList[i];
            alpha++;

            //Byte�̍ő�l�ȏ�ɂȂ�����255�Ɋۂ߂�
            if (_alphaList[i] >= 255)
            {
                alpha = 255;
            }

            //���b�V���̊e���_�̐F�f�[�^��ݒ�
            for (int j = 0; j < 4; j++)
            {
                _alphaList[i] = alpha;
                c = new Color32(c.r, c.g, c.b, alpha);
                newVertexColors[vertexIndex + j] = c;
            }
        }

        _textUi.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }
}
