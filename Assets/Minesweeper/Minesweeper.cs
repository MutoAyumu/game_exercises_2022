using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(GridLayoutGroup))]
public class Minesweeper : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Cell _cellPrefab = default;
    [SerializeField] GridLayoutGroup _grid = default;
    [SerializeField] int _rows = 10;
    [SerializeField] int _columus = 10;
    [SerializeField] int _mineCount = 5;

    [SerializeField] Text _timeText;
    float _timer;
    public bool _isGameSet;

    Cell[,] _cells;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_isGameSet) return;

        var hit = eventData.pointerCurrentRaycast.gameObject;
        Cell cell = null;

        if (GetCell(hit) is { } cellDate)
        {
            cell = cellDate.Cell;

            if (cell.CellState == CellState.None)
            {
                Test(cellDate.Raw, cellDate.Column);
        }
        }

        if (cell)
        {
            if (eventData.pointerId == -1)  //右クリック
            {
                if (cell.OpenState != OpenState.flag)
                {
                    cell.OpenState = OpenState.Open;
                }

                if (cell.CellState == CellState.Mine)
                {
                    _isGameSet = true;
                }
            }
            else if (eventData.pointerId == -2) //左クリック
            {
                if (cell.OpenState == OpenState.Open) return;

                if (cell.OpenState != OpenState.flag)
                {
                    cell.OpenState = OpenState.flag;
                }

                else if (cell.OpenState == OpenState.flag)
                {
                    cell.OpenState = OpenState.Close;
                }
            }
        }
    }
    void Test(int raw, int column)
    {
        if (_cells[raw, column].CellState != CellState.None) return;

        if (_cells[raw, column].OpenState == OpenState.Open) return;

        for(int r = raw - 1; r <= raw + 1; r++)
        {
            if (r < 0 || r >= _rows) continue;

            for (int c = column - 1; c <= column + 1; c++)
            {
                if (c < 0 || c >= _columus) continue;

                var cell = _cells[r, c];

                if (cell.CellState == CellState.None)
                {
                    Test(r, c);
                    cell.OpenState = OpenState.Open;
                    //return;
                }
                else if(cell.CellState != CellState.Mine)
                {
                    cell.OpenState = OpenState.Open;
                }
            }
        }
    }

    private void Start()
    {
        if (!_grid)
        {
            _grid = GetComponent<GridLayoutGroup>();
        }

        SetupCells();
        SetupMine();
    }

    private void SetupCells()
    {
        _grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _grid.constraintCount = _columus;

        _cells = new Cell[_rows, _columus];

        for (int r = 0; r < _rows; r++)
        {
            for (int c = 0; c < _columus; c++)
            {
                var cell = Instantiate(_cellPrefab, _grid.transform);
                _cells[r, c] = cell;
            }
        }
    }

    private void SetupMine()
    {
        //地雷を設置
        for (int i = 0; i < _mineCount; i++)
        {
            var r = Random.Range(0, _rows);
            var c = Random.Range(0, _columus);

            if (_cells[r, c].CellState != CellState.Mine)
            {
                _cells[r, c].CellState = CellState.Mine;

                for (var j = r - 1; j <= r + 1; j++)
                {
                    if (j < 0 || j >= _rows) continue;

                    for (var k = c - 1; k <= c + 1; k++)
                    {
                        if (k < 0 || k >= _columus) continue;

                        if (_cells[j, k].CellState != CellState.Mine)
                        {
                            var num = (int)_cells[j, k].CellState;
                            num++;
                            _cells[j, k].CellState = (CellState)num;
                        }
                    }
                }
            }
            else
            {
                i--;
            }
        }
    }

    void Update()
    {
        if(_timeText && !_isGameSet)
        {
            _timer += Time.deltaTime;
            _timeText.text = _timer.ToString("0000");
        }
    }

    (Cell Cell, int Raw, int Column)? GetCell(GameObject cell)
    {
        for (int r = 0; r < _rows; r++)
        {
            for (int c = 0; c < _columus; c++)
            {
                if (cell == _cells[r, c].gameObject)
                {
                    return (_cells[r, c], r, c);
                }
            }
        }
        return null;
    }
    void CheckedCells()
    {

    }
}
