using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slice : MonoBehaviour
{
    private int _sliceNum;
    public void Init(int num)
    {
        _sliceNum = num;
    }

    public int GetSliceNum()
    {
        return _sliceNum;
    }

}
