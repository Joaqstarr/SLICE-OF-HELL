using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pizza : MonoBehaviour
{
    [SerializeField]
    GameObject _slicePrefab;

    private int _sliceCount = 8;
    [SerializeField] private float _startingAngle = 22.5f;
    // Start is called before the first frame update
    void Start()
    {
        float angle = _startingAngle;
        for(int i = 0; i < _sliceCount; i++)
        {
            GameObject _spawnedSlice = Instantiate(_slicePrefab, transform);
            _spawnedSlice.transform.localPosition = Vector2.zero;
            _spawnedSlice.transform.rotation = Quaternion.Euler(0, 0, angle);
            angle += 360f / _sliceCount;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
