using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundSequencer : MonoBehaviour
{
    [SerializeField] ColorTransitioner _transitioner;

    [SerializeField] Color _color;
    [SerializeField] Sprite[] _sprites;

    bool _isFade;
    int _currentIndex = -1;

    private void Start()
    {
        MoveNext();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_transitioner.IsCompleted) { _transitioner.Skip(); }
            else { MoveNext(); }
        }
    }

    private void MoveNext()
    {
        var c = _color;
        Sprite sprite = null;
        if(_currentIndex + 1 < _sprites.Length)
            _currentIndex++;

        if(_sprites[_currentIndex])
        {
            sprite = _sprites[_currentIndex];
        }

        if(_isFade)
        {
            c.a = 0;
            sprite = null;
        }

        _transitioner?.Play(c, sprite);
        _isFade = !_isFade;
    }
}
