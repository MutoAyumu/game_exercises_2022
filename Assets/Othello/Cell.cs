using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Othello
{
    /// <summary>
    /// オセロのセルスクリプト(盤)
    /// </summary>
    public class Cell : MonoBehaviour
    {
        [SerializeField] CellType _cellType;
        [SerializeField] Material _none, _selected, _canPlaced;

        MeshRenderer _mesh;

        public CellType CellType
        {
            get => _cellType;
            set
            {
                _cellType = value;
                ChangeType();
            }
        }

        private void OnValidate()
        {
            ChangeType();
        }

        private void Awake()
        {
            _mesh = this.transform.GetChild(0).GetComponent<MeshRenderer>();
        }

        void ChangeType()
        {
            switch (_cellType)
            {
                case CellType.None:
                    if (_none && _mesh)
                        _mesh.material = _none;
                    Debug.Log($"Noneに切り替わった");
                    break;
                case CellType.Selected:
                    if (_selected && _mesh)
                        _mesh.material = _selected;
                    Debug.Log($"Selectedに切り替わった");
                    break;
                case CellType.Placed:
                    Debug.Log($"何か置かれている");
                    break;
            }
        }
        public void OnSelected(bool isSelect)
        {
            _mesh.material = isSelect ? _selected : _cellType == CellType.CanPlaced ? _canPlaced : _none;
        }
    }
    public enum CellType
    {
        None = 0,   //何も置かれていない
        Selected = 1,   //選択されている状態
        Placed = 2, //何かしら置かれている
        CanPlaced = 3 //配置できる状態
    }
}
