using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Task3 : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] int _row = 5;
    [SerializeField]int _column = 5;

    int _setupcells = 6;

    [SerializeField] GameObject _gameClearPanel;
    [SerializeField] Text _changeCountText;
    [SerializeField] Text _timeText;

    Image[,] _cells;
    int _ChangeCount = 0;
    float _timer;
    bool _isClear;
    private void Awake()
    {
        _cells = new Image[_row, _column];

        var grid = GetComponent<GridLayoutGroup>();
        grid.constraintCount = _column;
    }
    private void Start()
    {
        SetUp();
    }

    public void SetUp()
    {
        _ChangeCount = 0;
        _timer = 0;

        if (_changeCountText)
        {
            _changeCountText.text = "回数 : " + _ChangeCount.ToString("d5");
        }

        var random = Random.Range(_setupcells, _row * _column);
        int count = _row * _column;

        _gameClearPanel.SetActive(false);

        for (var r = 0; r < _row; r++)
        {
            for (var c = 0; c < _column; c++)
            {
                
                if(_cells[r,c] != null)
                Destroy(_cells[r,c].gameObject);

                count--;

                var cell = new GameObject($"Cell({r}, {c})");
                cell.transform.parent = transform;
                var image = cell.AddComponent<Image>();

                if(count < random)
                {
                    random--;
                    image.color = Color.black;
                }
                else if (Random.Range(0, 2) == 0 && random >= 0)
                {
                    random--;
                    image.color = Color.black;
                }

                _cells[r, c] = image;
            }
        }

        _isClear = false;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        var hitCell = eventData.pointerCurrentRaycast.gameObject;

        int row = 0;
        int column = 0;

        for (var r = 0; r < _row; r++)
        {
            for (var c = 0; c < _column; c++)
            {
                if(hitCell.transform == _cells[r, c].transform)
                {
                    row = r;
                    column = c;
                    Debug.Log(hitCell.name);
                    break;
                }
            }
        }
        //クリックしたセル
        _cells[row, column].color = _cells[row,column].color == Color.black? Color.white : Color.black;
        //右
        if(column + 1 < _column)
        _cells[row, column + 1].color = _cells[row, column + 1].color == Color.black ? Color.white : Color.black;
        //左
        if(column - 1 >= 0)
        _cells[row, column - 1].color = _cells[row, column - 1].color == Color.black ? Color.white : Color.black;
        //上
        if(row + 1 < _row)
        _cells[row + 1, column].color = _cells[row + 1, column].color == Color.black ? Color.white : Color.black;
        //下
        if(row - 1 >= 0)
        _cells[row - 1, column].color = _cells[row - 1, column].color == Color.black ? Color.white : Color.black;

        _ChangeCount++;

        if(_changeCountText)
        {
            _changeCountText.text = "回数 : " + _ChangeCount.ToString("d5");
        }

        Judge();
    }
    void Judge()
    {
        for(int r = 0; r < _row; r++)
        {
            for(int c = 0; c < _column; c++)
            {
                if(_cells[r,c].color != Color.white)
                {
                    return;
                }
            }
        }

        Debug.Log("クリア");
        _isClear = true;

        if(_gameClearPanel)
        _gameClearPanel.SetActive(true);
    }

    private void Update()
    {
        if(_timeText && !_isClear)
        {
            _timer += Time.deltaTime;
            _timeText.text = "時間 : " + ((int)_timer).ToString("d5");
        }
    }
}
