using UnityEngine;
using UnityEngine.UI;

public class Task2 : MonoBehaviour
{
    [SerializeField] int _row = 5;
    [SerializeField] int _column = 5;
    int _selectCellX;
    int _selectCellY;
    Image[,] _cells;

    private void Start()
    {
        _cells = new Image[_column, _row];

        var grid = GetComponent<GridLayoutGroup>();
        grid.constraintCount = _row;

        for (var r = 0; r < _column; r++)
        {
            for (var c = 0; c < _row; c++)
            {
                var obj = new GameObject($"Cell({r}, {c})");
                obj.transform.parent = transform;

                var image = obj.AddComponent<Image>();

                _cells[r,c] = image;

                if (r == 0 && c == 0) { image.color = Color.red; }
                else { image.color = Color.white; }
            }
        }
    }

    private void Update()
    {
        var h = (Input.GetKeyDown(KeyCode.LeftArrow) ? -1 : 0) +
                (Input.GetKeyDown(KeyCode.RightArrow) ? 1 : 0);

        var v = (Input.GetKeyDown(KeyCode.DownArrow) ? 1 : 0) +
                (Input.GetKeyDown(KeyCode.UpArrow) ? -1 : 0);

        if (h != 0 || v != 0)
        {
            _selectCellY += v;
            _selectCellX += h;

            OnChangeCells();
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            var cell = _cells[_selectCellY, _selectCellX];

            Destroy(cell);
            OnChangeCells();
        }
    }
    void OnChangeCells()
    {
        for (var i = 0; i < _cells.GetLength(0); i++)
        {
            for (int j = 0; j < _cells.GetLength(1); j++)
            {
                var image = _cells[i, j];
                if (!image) { continue; } // Destory Ï‚È‚ç–³Ž‹


                _selectCellY = Mathf.Clamp(_selectCellY, 0, _column - 1);
                _selectCellX = Mathf.Clamp(_selectCellX, 0, _row - 1);

                if (i == _selectCellY && j == _selectCellX)
                {
                    image.color = Color.red;
                }
                else
                {
                    image.color = Color.white;
                }
            }
        }
    }
}