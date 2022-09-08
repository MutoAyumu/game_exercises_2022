using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Othello
{
    /// <summary>
    /// �I�Z���̃Z���X�N���v�g(��)
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
                    Debug.Log($"None�ɐ؂�ւ����");
                    break;
                case CellType.Selected:
                    if (_selected && _mesh)
                        _mesh.material = _selected;
                    Debug.Log($"Selected�ɐ؂�ւ����");
                    break;
                case CellType.okareteiru:
                    Debug.Log($"�����u����Ă���");
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
        None = 0,   //�����u����Ă��Ȃ�
        Selected = 1,   //�I������Ă�����
        okareteiru = 2, //��������u����Ă���
    }
}
