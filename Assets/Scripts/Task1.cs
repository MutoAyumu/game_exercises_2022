using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Task1 : MonoBehaviour
{
    [SerializeField, Tooltip("タイルの最大数")] int _maxCellCount = 5;

    [SerializeField]Image[] _cells;
    [Tooltip("選択されているタイルの要素番号")] int _selectCell;
    [Tooltip("残っているタイルの数")] int _cellCount;
    private void Start()
    {
        _cellCount = _maxCellCount;
        _cells = new Image[_cellCount];

        for (var i = 0; i < _cellCount; i++)
        {
            var obj = new GameObject($"Cell{i}");
            obj.transform.parent = transform;

            var image = obj.AddComponent<Image>();
            if (i == 0) 
            { 
                image.color = Color.red;
                _selectCell = i;
            }
            else { image.color = Color.white; }

            _cells[i] = image;
        }
    }

    private void Update()
    {
        if (_cellCount > 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) // 左キーを押した
            {
                _selectCell--;

                if (_selectCell < 0)
                {
                    _selectCell = _cellCount - 1;
                    _cells[0].color = Color.white;
                }
                else
                {
                    _cells[_selectCell + 1].color = Color.white;
                }

                _cells[_selectCell].color = Color.red;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow)) // 右キーを押した
            {
                _selectCell++;

                if (_selectCell >= _cellCount)
                {
                    _selectCell = 0;
                    _cells[_cellCount - 1].color = Color.white;
                }
                else
                {
                    _cells[_selectCell - 1].color = Color.white;
                }

                _cells[_selectCell].color = Color.red;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Linqを使った方

                Destroy(_cells[_selectCell].gameObject);
                _cells = _cells.Where((_cells, index) => index != _selectCell).ToArray();   //選択されていたタイル以外を配列にする
                _cellCount--;   //カウントを減らす

                if (_cellCount > 0)
                {
                    _selectCell = 0;
                    _cells[_selectCell].color = Color.red;
                }

                //配列

                //for (int i = 0; i < _count; i++)
                //{
                //    Destroy(_array[i].gameObject);
                //}

                //_count--;
                //_array = new Image[_count];

                //for (var i = 0; i < _count; i++)
                //{
                //    var obj = new GameObject($"Cell{i}");
                //    obj.transform.parent = transform;

                //    var image = obj.AddComponent<Image>();

                //    if (i == 0)
                //    {
                //        image.color = Color.red;
                //        _currentNum = i;
                //    }
                //    else 
                //    { 
                //        image.color = Color.white; 
                //    }

                //    _array[i] = image;
                //}

                //if (_count > 0)
                //{
                //    _currentNum = 0;
                //    _array[_currentNum].color = Color.red;
                //}
            }
        }
    }
}