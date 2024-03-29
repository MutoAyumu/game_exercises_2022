using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessagePrinter : MonoBehaviour
{
    //自分で追加
    [SerializeField] ColorChanger _colorChanger;

    [SerializeField]
    private TMP_Text _textUi = default;

    [SerializeField]
    private string _message = "";

    [SerializeField]
    private float _speed = 1.0F;

    private float _elapsed = 0; // 文字を表示してからの経過時間
    private float _interval; // 文字毎の待ち時間

    // _message フィールドから表示する現在の文字インデックス。
    // 何も指していない場合は -1 とする。
    private int _currentIndex = -1;

    /// <summary>
    /// 文字出力中かどうか。
    /// </summary>
    public bool IsPrinting
    {
        get
        {
            // TODO: ここにコードを書く
            if (_currentIndex == _message.Length - 1)
            {
                _colorChanger?.ClearList();
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
    }

    /// <summary>
    /// 指定のメッセージを表示する。
    /// </summary>
    /// <param name="message">テキストとして表示するメッセージ。</param>
    public void ShowMessage(string message)
    {
        // TODO: ここにコードを書く
        _textUi.text = "";
        _interval = _speed / message.Length;
        _currentIndex = -1;
        _message = message;
    }

    /// <summary>
    /// 現在再生中の文字出力を省略する。
    /// </summary>
    public void Skip()
    {
        // TODO: ここにコードを書く
        _textUi.text = _message;
        _currentIndex = _message.Length - 1;
    }
}