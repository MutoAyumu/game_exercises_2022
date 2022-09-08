using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Othello
{
    public class Othello : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] int _raws = 8;
        [SerializeField] int _coulum = 8;

        [SerializeField] GridLayoutGroup _grid;
        [SerializeField] RectTransform _parent;

        [SerializeField] Cell _cellPrefab;
        [SerializeField] Stone _stonePrefab;

        Cell[,] _cellData;
        Turn _currentTurn = Turn.Black;

        private void Awake()
        {
            if (!_parent)
            {
                _parent = this.transform as RectTransform;
            }
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

        }

        /// <summary>
        /// Cellのデータを返す
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        (Cell cell, int r, int c)? GetCell(Cell cell)
        {
            for(int r = 0; r < _raws; r++)
            {
                for(int c = 0; c < _raws; c++)
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
    }
}
public enum Turn
{
    Black,
    White
}
