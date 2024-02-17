using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct CutInfo
{
    public Pizza.Direction Start;
    public Pizza.Direction End;
    public CutInfo(Pizza.Direction start, Pizza.Direction end)
    {
        Start = start;
        End = end;
    }
}

public class Pizza : MonoBehaviour
{

    public enum Direction
    {
        Top = 0,
        TopRight = 1,
        Right = 2,
        BottomRight = 3,
        Bottom = 4,
        BottomLeft = 5,
        Left = 6,
        TopLeft = 7
    }


    [SerializeField]
    Slice _slicePrefab;
    Slice[] _slices;
    private int _sliceCount = 8;
    [SerializeField] private float _startingAngle = 22.5f;

    [SerializeField]
    List<CutInfo> _cuts;
    // Start is called before the first frame update
    void Start()
    {
        _cuts = new List<CutInfo>();
        _slices = new Slice[_sliceCount];

        float angle = _startingAngle;
        for(int i = 0; i < _sliceCount; i++)
        {
            Slice _spawnedSlice = Instantiate(_slicePrefab, transform);
            _spawnedSlice.transform.localPosition = Vector2.zero;
            _spawnedSlice.transform.rotation = Quaternion.Euler(0, 0, angle);
            angle += 360f / _sliceCount;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MakeSlice(Vector2[] dirs)
    {
        _cuts.Add(new CutInfo(VectorToSliceDirection(dirs[0]), VectorToSliceDirection(dirs[1])));
    }

    private Direction VectorToSliceDirection(Vector2 dir)
    {
        if(dir.y > 0)
        {
            if(dir.x == 0)
            {
                return Direction.Top;
            }
            if (dir.x < 0)
            {
                return Direction.TopLeft;
            }
            else
            {
                return Direction.TopRight;
            }
        }
        else if(dir.y < 0)
        {
            if (dir.x == 0)
            {
                return Direction.Bottom;
            }
            if (dir.x < 0)
            {
                return Direction.BottomLeft;
            }
            else
            {
                return Direction.BottomRight;
            }
        }
        else
        {
            if(dir.x < 0)
            {
                return Direction.Left;
            }
            else
            {
                return Direction.Right;
            }
        }
    }
}
