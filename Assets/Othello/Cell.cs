using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Othello
{
    /// <summary>
    /// �I�Z���̃Z���X�N���v�g(��)
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
                    Debug.Log($"None�ɐ؂�ւ����");
                    break;
                case CellType.Selected:
                    if (_selected && _mesh)
                        _mesh.material = _selected;
                    Debug.Log($"Selected�ɐ؂�ւ����");
                    break;
                case CellType.Placed:
                    Debug.Log($"�����u����Ă���");
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
        None = 0,   //�����u����Ă��Ȃ�
        Selected = 1,   //�I������Ă�����
        Placed = 2, //��������u����Ă���
        CanPlaced = 3 //�z�u�ł�����
    }
}
