using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Minesweeper
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] Text _view = default;
        [SerializeField] CellState _cellState = CellState.None;
        [SerializeField] OpenState _openState = OpenState.Close;
        [SerializeField] Image _image;

        Minesweeper _minesweeper;

        public CellState CellState
        {
            get => _cellState;
            set
            {
                _cellState = value;
                OnCellStateChanged();
            }
        }
        public OpenState OpenState
        {
            get => _openState;
            set
            {
                _openState = value;
                OnOpenStatechanged();
            }
        }

        private void Awake()
        {
            _minesweeper = FindObjectOfType<Minesweeper>();
        }

        private void Start()
        {
            OnCellStateChanged();
        }

        private void OnValidate()
        {
            OnCellStateChanged();
            OnOpenStatechanged();
        }

        private void OnCellStateChanged()
        {
            if (!_view) return;

            if (_openState == OpenState.Close || _openState == OpenState.flag) return;

            if (_cellState == CellState.Mine)
            {
                _view.text = "X";
                _view.color = Color.red;
            }
            else if (_cellState == CellState.None)
            {
                _view.text = "";
            }
            else
            {
                _view.text = ((int)_cellState).ToString();
                _view.color = Color.blue;
            }
        }
        private void OnOpenStatechanged()
        {
            if (!_image || !_view) return;

            if (_openState == OpenState.Close)
            {
                _image.color = Color.gray;
                _view.text = "";
            }
            else if (_openState == OpenState.flag)
            {
                _image.color = Color.white;
                _view.color = Color.red;
                _view.text = "F";
            }
            else
            {
                _image.color = Color.white;
                OnCellStateChanged();
            }
        }
        public void OpenCells()
        {
            if (_openState == OpenState.Open) return;

            OpenState = OpenState.Open;

            if (_cellState != CellState.None) return;

            if (_minesweeper.GetCell(this.gameObject) is { } data)
            {
                var row = data.Row;
                var column = data.Column;
                var cells = _minesweeper.GetCellArray();

                for (int r = row - 1; r <= row + 1; r++)
                {
                    if (r < 0 || r >= cells.GetLength(0)) continue;

                    for (int c = column - 1; c <= column + 1; c++)
                    {
                        if (c < 0 || c >= cells.GetLength(1)) continue;

                        var cell = cells[r, c];

                        cell.OpenCells();
                    }
                }
            }
        }
    }
    public enum CellState
    {
        None = 0, // ‹óƒZƒ‹

        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,

        Mine = -1, // ’n—‹
    }
    public enum OpenState
    {
        Open = 1,
        Close = 2,

        flag = 3,   //Šø
    }
}