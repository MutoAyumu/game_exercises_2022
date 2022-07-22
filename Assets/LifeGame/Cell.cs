using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LifeGame
{
    /// <summary>
    /// LifeGameのセルスクリプト
    /// </summary>
    public class Cell : MonoBehaviour
    {
        [SerializeField] CellState _cellState = CellState.Death;
        Image _image;

        public CellState CellState 
        { 
            get => _cellState;
            set 
            {
                _cellState = value;
                CellStateChanged();
            }
        }

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        /// <summary>
        /// セルの切り替えを行う
        /// </summary>
        public void CellStateChanged()
        {
            var color = Color.white;

            if(_cellState == CellState.Death)
            {
                _cellState = CellState.Life;
                color = Color.black;
            }
            else
            {
                _cellState = CellState.Death;
            }

            _image.color = color;
        }
    }
    public enum CellState
    {
        Death = 0,
        Life = 1,
    }
}
