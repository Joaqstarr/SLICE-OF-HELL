using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct TweenSettings
{
    public float duration;
    public float strength;
    public Ease ease;
}
public class PlayerManager : MonoBehaviour
{

    private PlayerBaseState _currentState;

    private PlayerBaseState _playerGameState = new PlayerPlayingState();

    public Transform Pizza;

    public float Radius = 1;
    public PlayerControls Controls;

    public LineRenderer Line;

    public TweenSettings FirstMoveTween;
    public TweenSettings SecondMoveTween;
    public TweenSettings SetAngleTween;

    [Header("Audio")]
    public AudioSource _angleSource;
    public AudioSource _aimSource;
    public AudioSource _swipeSource;
    


    // Start is called before the first frame update
    void Start()
    {
        _currentState = _playerGameState;

        _currentState.OnEnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        _currentState.OnUpdateState(this);

    }

    public void SwitchState(PlayerBaseState newState)
    {
        _currentState.OnExitState(this);
        _currentState = newState;
        _currentState.OnEnterState(this);
    }
}
