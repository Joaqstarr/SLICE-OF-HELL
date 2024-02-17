using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[Serializable]
public struct CutInfo
{
    public Pizza.Direction Start;
    public Pizza.Direction End;
    public CutInfo(Pizza.Direction start, Pizza.Direction end)
    {
        if((int)start < (int)end)
        {
            Pizza.Direction hold = end;
            end = start;
            start = hold;
        }
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
    Transform _sliceHolder;


    [SerializeField]
    Slice _slicePrefab;
    Slice[] _slices;
    private int _sliceCount = 8;
    [SerializeField] private float _startingAngle = 0;

    [SerializeField]
    List<CutInfo> _cuts;

    private List<SliceHolder> _sliceHolders;
    // Start is called before the first frame update
    void Start()
    { 

        _sliceHolders = new List<SliceHolder>();

        _cuts = new List<CutInfo>();
        _slices = new Slice[_sliceCount];

        float angle = _startingAngle;
        for(int i = 0; i < _sliceCount; i++)
        {
            Slice _spawnedSlice = Instantiate(_slicePrefab, transform.GetChild(0));

            _spawnedSlice.Init(i);
            _spawnedSlice.transform.localPosition = Vector2.zero;
            _spawnedSlice.transform.rotation = Quaternion.Euler(0, 0, angle);
            angle -= 360f / _sliceCount;
            _slices[i] = _spawnedSlice;
        }
        _sliceHolders.Add(transform.GetChild(0).GetComponent<SliceHolder>());
        UpdateAllSliceHolders(false);



    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MakeSlice(Vector2[] dirs)
    {
        CutInfo thisCutInfo = new CutInfo(VectorToSliceDirection(dirs[0]), VectorToSliceDirection(dirs[1]));
        _cuts.Add(thisCutInfo);
        int holdersCount = _sliceHolders.Count;
        for (int j = 0; j < holdersCount; j++)
        {

            CutFromHolder(thisCutInfo, _sliceHolders[j]);
        }

        UpdateAllSliceHolders(true);
        for (int i = 0; i < _sliceHolders.Count; i++)
        {
            MakeSliceValid(_sliceHolders[i]);
        }
        UpdateAllSliceHolders(true, true);

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

    private void CutFromHolder(CutInfo cut, SliceHolder holder)
    {
        int endPoint = (int)cut.End;

        if (holder.HeldSlices.Length > 1)
        {
            Transform newSliceHolder = null;

            int i = (int)cut.Start;
            while (i != endPoint)
            {
                if (holder.SliceMap.ContainsKey(i))
                {
                    if (newSliceHolder == null)
                        newSliceHolder = Instantiate(_sliceHolder, transform);

                    holder.SliceMap[i].transform.parent = newSliceHolder;
                    holder.SliceMap[i].transform.localPosition = Vector2.zero;
                }

                i = (i + 1) % _slices.Length;
            }
            if (newSliceHolder != null)
                _sliceHolders.Add(newSliceHolder.GetComponent<SliceHolder>());
        }
    }

    private void MakeSliceValid(SliceHolder sliceToCheck)
    {

        for (int i = 0; i < sliceToCheck.HeldIndices.Length -1; i++)
        {

            if (MathF.Abs( sliceToCheck.HeldIndices[i] - sliceToCheck.HeldIndices[(i+1)%sliceToCheck.HeldIndices.Length])> 1.1f && !(sliceToCheck.HeldIndices[i] == 7 && sliceToCheck.HeldIndices[(i + 1) % sliceToCheck.HeldIndices.Length] == 0))
            {
                CutInfo cutToMake = new CutInfo((Direction)sliceToCheck.HeldIndices[0], (Direction)sliceToCheck.HeldIndices[i + 1]);
                CutFromHolder(cutToMake, sliceToCheck);
                return;
            }
            
        }
    }
    public void UpdateAllSliceHolders(bool deleteIfEmpty = false, bool move = false)
    {
        for(int i = 0; i < _sliceHolders.Count; i++)
        {
            _sliceHolders[i].UpdateInfo(deleteIfEmpty, move);
        }
    }
}
