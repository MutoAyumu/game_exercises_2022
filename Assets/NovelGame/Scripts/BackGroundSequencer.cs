using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundSequencer : MonoBehaviour
{
    [SerializeField] ColorTransitioner _transitioner;

    [SerializeField] Color[] _colors;
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
        if (_colors is null or { Length: 0 }) { return; }

        if (_currentIndex + 1 < _colors.Length)
        {
            _currentIndex++;
            _transitioner?.Play(_colors[_currentIndex]);
        }
    }
}
