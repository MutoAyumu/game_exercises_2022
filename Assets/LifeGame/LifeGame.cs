using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace LifeGame
{
    public class LifeGame : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] int _raws = 5;
        [SerializeField] int _columus = 5;

        [SerializeField] RectTransform _parent;

        [SerializeField] float _setTime = 1f;

        Cell[,] _cells;
        GridLayoutGroup _grid;
        GameState _gameState = GameState.Stop;
        float _timer;

        /* MEMO
         * 生きているセルの座標を記録している配列を作りLoopで回す
         */

        private void Awake()
        {
            //初期設定
            _cells = new Cell[_raws, _columus];

            if (!_parent)
            {
                _parent = GetComponent<RectTransform>();
            }

            _grid = _parent.GetComponent<GridLayoutGroup>();
            _grid.constraintCount = _columus;
        }
        private void Start()
        {
            OnStart();
        }

        /// <summary>
        /// ゲームの初期化
        /// </summary>
        public void OnStart()
        {
            for (int r = 0; r < _raws; r++)
            {
                for (int c = 0; c < _columus; c++)
                {
                    if (_cells[r, c])
                    {
                        Destroy(_cells[r, c].gameObject);
                        _cells[r, c] = null;
                    }

                    var go = new GameObject();
                    go.AddComponent<Image>();
                    go.name = $"Cell[{r},{c}]";
                    go.transform.SetParent(_parent);

                    var cell = go.AddComponent<Cell>();

                    _cells[r, c] = cell;
                }
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var go = eventData.pointerCurrentRaycast.gameObject;

            if (GetCell(go) is { } data)
            {
                var cell = data.cell;
                cell.CellStateChanged();
            }
        }

        /// <summary>
        /// セルの情報を取得
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        (Cell cell, int raw, int columu)? GetCell(GameObject go)
        {
            for (int r = 0; r < _raws; r++)
            {
                for (int c = 0; c < _columus; c++)
                {
                    if (_cells[r, c].gameObject == go)
                        return (_cells[r, c], r, c);
                }
            }

            return null;
        }

        /// <summary>
        /// Button等で呼ぶ関数
        /// </summary>
        /// <param name="state"></param>
        public void GameStateChanged(int num)
        {
            _gameState = (GameState)num;
            Debug.Log(_gameState);
        }

        private void Update()
        {
            switch (_gameState)
            {
                case GameState.Stop:
                    break;
                case GameState.Update:
                    Run();
                    break;
                case GameState.Skip:
                    Skip();
                    GameStateChanged(0);
                    break;
            }
        }

        void Run()
        {
            //連続で実行する

            if (_timer >= 0)
            {
                _timer -= Time.deltaTime;
                return;
            }

            Move();

            _timer = _setTime;
        }
        void Skip()
        {
            //１回動かす

            Move();
        }
        void Move()
        {
            int[,] data = new int[_raws, _columus];

            for (int r = 0; r < _raws; r++)
            {
                for (int c = 0; c < _columus; c++)
                {
                    var num = SearchCells(r, c);
                    data[r, c] = num;
                    //Debug.Log($"{num}:{r},{c}");
                }
            }

            for (int r = 0; r < _raws; r++)
            {
                for (int c = 0; c < _columus; c++)
                {
                    var num = data[r, c];

                    if (num == 3)
                    {
                        if (_cells[r, c].CellState == CellState.Death)
                        {
                            //誕生
                            var cell = _cells[r, c];
                            cell.CellStateChanged();
                        }
                    }
                    else if (num == 0)
                    {
                        //周囲に生きたセルがいない
                    }

                    if (_cells[r, c].CellState == CellState.Life)
                    {
                        if (num == 2 || num == 3)
                        {
                            //生存
                        }
                        else if (num <= 1 || num >= 4)
                        {
                            //死
                            var cell = _cells[r, c];
                            cell.CellStateChanged();
                        }
                    }
                }
            }
        }
        int SearchCells(int raw, int columu)
        {
            //隣接する生きたセルの数を返す

            int count = 0;

            for (int r = raw - 1; r <= raw + 1; r++)
            {
                if (r < 0 || r >= _raws)
                    continue;

                for (int c = columu - 1; c <= columu + 1; c++)
                {
                    if (c < 0 || c >= _columus)
                        continue;

                    if (c == columu && r == raw) //自分を除く
                        continue;

                    if (_cells[r, c].CellState == CellState.Life)
                    {
                        count++;
                    }
                }
            }

            return count;
        }
    }

    public enum GameState
    {
        Stop = 0,
        Update = 1,
        Skip = 2
    }
}
