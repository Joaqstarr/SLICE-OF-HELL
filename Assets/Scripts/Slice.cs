using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slice : MonoBehaviour
{
    private int _sliceNum;
    [SerializeField]
    private Sprite[] art;
    [SerializeField] SpriteRenderer _renderer;
    [SerializeField]
    float _repeatTime = 0.1f;
    private bool Alive = true;
    public void Init(int num)
    {
        _sliceNum = num;
    }

    public int GetSliceNum()
    {
        return _sliceNum;
    }
    private void Start()
    {
        InvokeRepeating("UpdateArt", _repeatTime, _repeatTime);
    }
    private void UpdateArt()
    {
        if (!Alive) return;
        _renderer.sprite = art[Random.Range(0,art.Length)];
    }

    public void Kill()
    {
        Alive = false;
    }
    public void Revive()
    {
        Alive = true;
    }
}
