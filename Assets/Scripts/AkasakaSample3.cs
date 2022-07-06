using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AkasakaSample3 : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private int _rows = 5;

    [SerializeField]
    private int _columns = 5;

    private GameObject[,] _cells; // 見た目
    private bool[,] _lights; // 内部データ

    private void Start()
    {
        // FixedColumnCount 前提
        var layout = GetComponent<GridLayoutGroup>();
        layout.constraintCount = _columns;

        _cells = CreateCells(_rows, _columns);
        _lights = CreateLights(_rows, _columns);

        for (var r = 0; r < _lights.GetLength(0); r++)
        {
            for (var c = 0; c < _lights.GetLength(1); c++)
            {
                var next = ToggleLights(_lights, r, c);
                if (All(next, false))
                {
                    Debug.Log("1手で終わるまずいデータ");
                }
            }
        }

        UpdateCells();
    }

    /// <summary>
    /// 2次元照明データの生成。
    /// </summary>
    /// <param name="rows">行数。</param>
    /// <param name="columns">列数。</param>
    /// <returns>照明データ。</returns>
    private bool[,] CreateLights(int rows, int columns)
    {
        // デバッグ用の1手で終わるデータ（5x5固定なので注意）
        //return new[,]
        //{
        //    { false, false, false, false,false },
        //    { false, false, true, false,false },
        //    { false, true, true, true,false },
        //    { false, false, true, false,false },
        //    { false, false, false, false,false },
        //};

        var lights = new bool[rows, columns];

        // ただのランダム配置
        //for (var r = 0; r < lights.GetLength(0); r++)
        //{
        //    for (var c = 0; c < lights.GetLength(1); c++)
        //    {
        //        lights[r, c] = Random.value < 0.5;
        //    }
        //}

        // 適当にボタン押しまくったようなランダム
        for (var i = 0; i < 3; i++)
        {
            var r = Random.Range(0, rows);
            var c = Random.Range(0, columns);
            lights = ToggleLights(lights, r, c);
            Debug.Log($"{r}, {c}"); // これの逆順で必ずクリアできる
        }
        return lights;
    }


    /// <summary>
    /// 2次元照明ビュー（GameObject）の生成。
    /// </summary>
    /// <param name="rows">行数。</param>
    /// <param name="columns">列数。</param>
    /// <returns>照明ビュー。</returns>
    private GameObject[,] CreateCells(int rows, int columns)
    {
        var cells = new GameObject[rows, columns];
        for (var r = 0; r < cells.GetLength(0); r++)
        {
            for (var c = 0; c < cells.GetLength(1); c++)
            {
                var cell = new GameObject($"Cell({r}, {c})");
                cell.transform.parent = transform;
                cell.AddComponent<Image>();
                cells[r, c] = cell;
            }
        }
        return cells;
    }

    /// <summary>
    /// 指定した照明データがすべて点灯または消灯しているかどうか。
    /// </summary>
    /// <param name="lights">照明データ。</param>
    /// <param name="isOn">
    /// すべてが点灯しているかどうかを調べるなら true。
    /// すべてが消灯しているかどうかを調べるなら false。</param>
    /// <returns>照明データがすべて同じ状態なら true。そうでなければ false。</returns>
    private bool All(bool[,] lights, bool isOn)
    {
        for (var r = 0; r < lights.GetLength(0); r++)
        {
            for (var c = 0; c < lights.GetLength(1); c++)
            {
                var light = lights[r, c];
                if (light != isOn) { return false; }
            }
        }

        return true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var cell = eventData.pointerCurrentRaycast.gameObject;

        // クリックされたセルの行番号と列番号取得
        if (GetCellPosition(cell) is { } pt)
        {
            Debug.Log($"Selected: {pt.Row}, {pt.Column}");
            _lights = ToggleLights(_lights, pt.Row, pt.Column);
            UpdateCells();
        }
        if (All(_lights, false))
        {
            Debug.Log("全部消灯");
        }
    }

    /// <summary>
    /// 指定したセル番号の照明とその周囲4マスの状態を反転させたデータを生成する。
    /// </summary>
    /// <param name="lights">元照明データ。</param>
    /// <param name="row">行番号。</param>
    /// <param name="column">列番号。</param>
    /// <returns>指定行番号・列番号のセルとその周囲4マスの状態を反転させたデータ。</returns>
    private bool[,] ToggleLights(bool[,] lights, int row, int column)
    {
        // 配列のコピーを取る
        var rows = lights.GetLength(0);
        var columns = lights.GetLength(1);
        var result = new bool[rows, columns];
        for (var r = 0; r < lights.GetLength(0); r++)
        {
            for (var c = 0; c < lights.GetLength(1); c++)
            {
                result[r, c] = lights[r, c];
            }
        }

        result[row, column] = !result[row, column];
        if (row - 1 >= 0)
        {
            result[row - 1, column] = !result[row - 1, column];
        }
        if (row + 1 < rows)
        {
            result[row + 1, column] = !result[row + 1, column];
        }
        if (column - 1 >= 0)
        {
            result[row, column - 1] = !result[row, column - 1];
        }
        if (column + 1 < columns)
        {
            result[row, column + 1] = !result[row, column + 1];
        }

        return result;
    }

    /// <summary>
    /// 内部の照明データと見た目のセルを一致させる。
    /// </summary>
    private void UpdateCells()
    {
        for (var r = 0; r < _lights.GetLength(0); r++)
        {
            for (var c = 0; c < _lights.GetLength(1); c++)
            {
                var light = _lights[r, c];
                var cell = _cells[r, c];
                var image = cell.GetComponent<Image>();
                image.color = light ? Color.white : Color.black;
            }
        }
    }

    /// <summary>
    /// セルの行番号と列番号を取得する。
    /// </summary>
    /// <param name="cell">調べたいセル。</param>
    /// <returns>セルの行番号と列番号。見つからなければ null。</returns>
    private (int Row, int Column)? GetCellPosition(GameObject cell)
    {
        for (var r = 0; r < _cells.GetLength(0); r++)
        {
            for (var c = 0; c < _cells.GetLength(1); c++)
            {
                if (cell == _cells[r, c])
                {
                    return (r, c);
                }
            }
        }
        return null;
    }
}
