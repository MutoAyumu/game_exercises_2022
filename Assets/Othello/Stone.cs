using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Othello
{
    /// <summary>
    /// オセロの駒
    /// </summary>
    public class Stone : MonoBehaviour
    {
        [SerializeField]StoneType _stoneType;

        int _blackRotationX = 0;
        int _whiteRotationX = 180;

        public StoneType StoneType
        {
            get => _stoneType;
            set
            {
                _stoneType = value;
                ChangeType();
            }
        }

        private void OnValidate()
        {
            ChangeType();
        }

        /// <summary>
        /// StoneTypeを変更する
        /// </summary>
        void ChangeType()
        {
            var x = 0;

            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }

            switch (_stoneType)
            {
                case StoneType.Black:
                    x = _blackRotationX;
                    break;
                case StoneType.White:
                    x = _whiteRotationX;
                    break;
            }

            this.transform.rotation = Quaternion.Euler(x,0,0); //回転させる
            //this.transform.Rotate(new Vector3(x, 0, 0));
        }
    }
    public enum StoneType
    {
        White = 0,
        Black = 1
    }
}
