using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Othello
{
    /// <summary>
    /// オセロのセルスクリプト(盤)
    /// </summary>
    public class Cell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] CellType _cellType;
        [SerializeField] Material _none, _selected;

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
            _mesh = GetComponent<MeshRenderer>();
        }

        void ChangeType()
        {
            switch(_cellType)
            {
                case CellType.None:
                    if(_none && _mesh)
                        _mesh.material = _none;
                    Debug.Log($"Noneに切り替わった");
                    break;
                case CellType.Selected:
                    if (_selected && _mesh)
                        _mesh.material = _selected;
                    Debug.Log($"Selectedに切り替わった");
                    break;
                case CellType.okareteiru:
                    Debug.Log($"何か置かれている");
                    break;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            
        }
    }
    public enum CellType
    {
        None = 0,   //何も置かれていない
        Selected = 1,   //選択されている状態
        okareteiru = 2, //何かしら置かれている
    }
}
