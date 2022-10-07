using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Othello
{
    public class Othello : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] int _rows = 8;
        [SerializeField] int _coulum = 8;

        [SerializeField] Cell _cellPrefab;
        [SerializeField] Stone _stonePrefab;

        [SerializeField] int _selectR, _selectC;
        Cell _currentSelectCell;

        Cell[,] _cellData;
        Stone[,] _stoneData;
        Turn _currentTurn = Turn.Black;

        List<Stone> _changeStone = new List<Stone>();

        private void Awake()
        {

        }

        private void Start()
        {
            Setup();
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        void Setup()
        {
            _cellData = new Cell[_rows, _coulum];

            var CellParent = new GameObject("CellParent");

            for(int r = 0; r < _rows; r++)
            {
                for(int c = 0; c < _coulum; c++)
                {
                    var half = _cellPrefab.transform.localScale.x;
                    var cell = Instantiate(_cellPrefab, new Vector3((c - half) * 1.1f, (-r + half) * 1.1f, 0), Quaternion.identity);
                    cell.name = $"Cell[{r},{c}]";
                    _cellData[r, c] = cell;
                    cell.transform.SetParent(CellParent.transform);
                }
            }

            _stoneData = new Stone[_rows,_coulum];

            var StoneParent = new GameObject("StoneParent");

            for (int r = 0; r < _rows; r++)
            {
                for (int c = 0; c < _coulum; c++)
                {
                    var pos = _cellData[r, c].transform.position;
                    pos.z -= 0.35f;
                    var stone = Instantiate(_stonePrefab, pos, Quaternion.identity);
                    stone.name = $"Stone[{r},{c}]";
                    _stoneData[r, c] = stone;
                    stone.transform.SetParent(StoneParent.transform);
                    stone.gameObject.SetActive(false);
                }
            }

            _stoneData[3, 3].StoneType = StoneType.White;
            _stoneData[3, 4].StoneType = StoneType.Black;
            _stoneData[4, 3].StoneType = StoneType.Black;
            _stoneData[4, 4].StoneType = StoneType.White;

            _currentSelectCell = _cellData[0, 0];

            SetPrediction();
        }

        /// <summary>
        /// Cellのデータを返す
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        (Cell cell, int r, int c)? GetCell(Cell cell)
        {
            for(int r = 0; r < _rows; r++)
            {
                for(int c = 0; c < _rows; c++)
                {
                    if (cell == _cellData[r, c])
                        return (_cellData[r, c], r, c);
                }
            }

            Debug.Log($"該当のCellDataが見つかりませんでした");
            return (null, 0, 0);
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            var go = eventData.pointerCurrentRaycast.gameObject;

            if(TryGetComponent<Cell>(out var cell))
            {
                if(GetCell(cell) is { } data)
                {
                    
                }
            }    
        }
        void CellCheck(Cell cell)
        {

        }
        void StoneCheck(Stone stone)
        {
            //石の切り替え・探索
            var type = StoneType.Black;

            switch (_currentTurn)
            {
                case Turn.Black:
                    type = StoneType.Black;
                    break;
                case Turn.White:
                    type = StoneType.White;
                    break;
            }

            stone.StoneType = type;
        }

        private void Update()
        {
            keyInput();
        }

        void keyInput()
        {
            if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (_selectR - 1 < 0) return;

                _selectR--;
                _currentSelectCell.OnSelected(false);
                _currentSelectCell = _cellData[_selectR, _selectC];
                _currentSelectCell.OnSelected(true);
            }
            else if(Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (_selectR + 1 >= _rows) return;

                _selectR++;
                _currentSelectCell.OnSelected(false);
                _currentSelectCell = _cellData[_selectR, _selectC];
                _currentSelectCell.OnSelected(true);
            }       
            else if(Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (_selectC + 1 >= _coulum) return;

                _selectC++;
                _currentSelectCell.OnSelected(false);
                _currentSelectCell = _cellData[_selectR, _selectC];
                _currentSelectCell.OnSelected(true);
            }   
            else if(Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (_selectC - 1 < 0) return;

                _selectC--;
                _currentSelectCell.OnSelected(false);
                _currentSelectCell = _cellData[_selectR, _selectC];
                _currentSelectCell.OnSelected(true);
            }
            else if(Input.GetKeyDown(KeyCode.Space))
            {
                PutStone(_selectR, _selectC);
            }
        }

        void SetPrediction()
        {
            for(int r = 0; r < _rows; r++)
            {
                for(int c = 0; c < _coulum; c++)
                {
                    var type = _cellData[r, c].CellType;

                    if ((type == CellType.None || type == CellType.CanPlaced) && GetAroundStone(r,c))
                    {
                        _cellData[r, c].CellType = CellType.CanPlaced;
                        _changeStone.Clear();
                    }
                    else if((_cellData[r,c].CellType == CellType.None || _cellData[r,c].CellType == CellType.CanPlaced) && !GetAroundStone(r,c))
                    {
                        _cellData[r, c].CellType = CellType.None;
                    }
                }
            }
        }

        /// <summary>
        /// 石を置く
        /// </summary>
        /// <param name="r"></param>
        /// <param name="c"></param>
        void PutStone(int r, int c)
        {
            if (_cellData[r, c].CellType != CellType.CanPlaced) return;

            if(GetAroundStone(r,c))
            {
                _stoneData[r, c].StoneType = _currentTurn == Turn.White ? StoneType.White : StoneType.Black;

                foreach(var stone in _changeStone)
                {
                    stone.StoneType = _currentTurn == Turn.White ? StoneType.White : StoneType.Black;
                }

                _currentTurn = _currentTurn == Turn.White ? Turn.Black : Turn.White;
            }

            _changeStone.Clear(); 
        }

        //各方向で返せる石があるか調べる
        bool GetAroundStone(int r, int c)
        {
            var stoneFlag = false;

            //裏返す石のタイプ
            var stoneType = _currentTurn == Turn.White ? StoneType.Black : StoneType.White;

            //上
            if(r - 1 >= 0) //例外除去
            {
                if(_cellData[r - 1, c].CellType == CellType.Placed && _stoneData[r - 1, c].StoneType == stoneType)
                {
                    var count = 1;

                    while(r - count >= 0)
                    {
                        count++;
                        
                        if(_cellData[r - count, c].CellType == CellType.Placed)
                        {
                            if(!stoneFlag)
                            {
                                stoneFlag = true;
                            }

                            if (_stoneData[r - count, c].StoneType == stoneType)
                            {
                                _changeStone.Add(_stoneData[r - count, c]);
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

            return stoneFlag;
        }
    }
}
public enum Turn
{
    Black,
    White
}
