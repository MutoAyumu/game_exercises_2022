using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    /*MEMO
    （未完成）
    文字が全て表示されるまで点滅した感じになる
    Updateでぶん回してるから適当に条件を付けて止めるようにする
    Listとかで無理やりやってる感があるから何とかしたい
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
        //テキスト オブジェクトに含まれるすべての要素を取得
        TMP_TextInfo textInfo = _textUi.textInfo;

        //文字が無ければ戻る
        if (textInfo.characterCount == 0) return;

        _textUi.ForceMeshUpdate();

        //頂点の色を入れておくもの
        Color32[] newVertexColors;
        Color32 c = _textUi.color;

        //文字数が更新されたらアルファ値を入れておくリストを追加
        if (textInfo.characterCount != _characterCount)
        {
            _characterCount = textInfo.characterCount;
            _alphaList.Add(0);
        }

        //文字数分ループ
        for (int i = 0; i < _alphaList.Count; i++)
        {
            //TextInfo使っているマテリアルのi番目のインデックスを取得
            int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
            //メッシュの頂点の色を取得
            newVertexColors = textInfo.meshInfo[materialIndex].colors32;
            //TextInfoで使われる最初の頂点のインデックスを取得
            int vertexIndex = textInfo.characterInfo[i].vertexIndex;

            //アルファ値を増やす
            var alpha = _alphaList[i];
            alpha++;

            //Byteの最大値以上になったら255に丸める
            if (_alphaList[i] >= 255)
            {
                alpha = 255;
            }

            //メッシュの各頂点の色データを設定
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
