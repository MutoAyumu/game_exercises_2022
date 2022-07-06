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
    [SerializeField] Image _gameClearPanel;
    [SerializeField] Image _gameOverPanel;
    float _timer;
    [SerializeField] GameState _gameState = GameState.Start;

    Cell[,] _cells;

    public void OnPointerClick(PointerEventData eventData)
    {
        //if (_isGameSet) return;

        var hit = eventData.pointerCurrentRaycast.gameObject;
        var cell = hit.GetComponent<Cell>();

        if (!cell) return;

        if (eventData.pointerId == -1)  //右クリック
        {
            if (cell.OpenState != OpenState.flag)
            {
                if (_gameState == GameState.Start)
                {
                    SetupMine(hit);
                }

                cell.OpenCells();
                CheckedCells();

                if (cell.CellState == CellState.Mine)
                {
                    _gameState = GameState.GameOver;

                    if (_gameOverPanel)
                        _gameOverPanel.gameObject.SetActive(true);
                }
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

    private void Awake()
    {
        if (!_grid)
        {
            _grid = GetComponent<GridLayoutGroup>();
        }

        //_mineCount = Mathf.Min(_mineCount, _rows * _columus / 5);

        SetupCells();
    }
    private void Start()
    {
        Setup();
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
                cell.name = $"Cell[{r},{c}]";
            }
        }
    }

    private void SetupMine(GameObject cell)
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

        if(GetCell(cell) is { } data)
        {
            var row = data.Row;
            var column = data.Column;

            if(_cells[row, column].CellState == CellState.Mine)
            {
                Setup();

                SetupMine(cell);
            }
            else
            {
                _gameState = GameState.Playback;
            }
        }
    }

    public void Setup()
    {
        for (int r = 0; r < _rows; r++)
        {
            for (int c = 0; c < _columus; c++)
            {
                _cells[r, c].CellState = CellState.None;
                _cells[r, c].OpenState = OpenState.Close;
            }
        }

        _gameState = GameState.Start;
        _timer = 0;

        if (_timeText)
            _timeText.text = _timer.ToString("0000");

        if (_gameClearPanel)
            _gameClearPanel.gameObject.SetActive(false);

        if (_gameOverPanel)
            _gameOverPanel.gameObject.SetActive(false);
    }

    void Update()
    {
        if (_timeText && _gameState == GameState.Playback)
        {
            _timer += Time.deltaTime;
            _timeText.text = _timer.ToString("0000");
        }
    }

    public (int Row, int Column)? GetCell(GameObject cell)
    {
        for (int r = 0; r < _rows; r++)
        {
            for (int c = 0; c < _columus; c++)
            {
                if (cell == _cells[r, c].gameObject)
                {
                    return (r, c);
                }
            }
        }
        return null;
    }
    public Cell[,] GetCellArray()
    {
        return _cells;
    }
    void CheckedCells()
    {
        if (_gameState != GameState.Playback) return;

        var normalCells = _rows * _columus - _mineCount;
        var count = 0;

        for(int r = 0; r < _rows; r++)
        {
            for(int c = 0; c < _columus; c++)
            {
                if (_cells[r, c].OpenState != OpenState.Open) continue;
                if (_cells[r, c].CellState == CellState.Mine) continue;

                count++;
            }
        }

        if(count == normalCells)
        {
            _gameState = GameState.GameClear;

            for (int r = 0; r < _rows; r++)
            {
                for (int c = 0; c < _columus; c++)
                {
                    _cells[r, c].OpenState = OpenState.Open;
                }
            }

            if (_gameClearPanel)
                _gameClearPanel.gameObject.SetActive(true);
        }
    }
}
public enum GameState
{
    Start = 1,
    Playback = 2,
    GameOver = 3,
    GameClear = 4
}
