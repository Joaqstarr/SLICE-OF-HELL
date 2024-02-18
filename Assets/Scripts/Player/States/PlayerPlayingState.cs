using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlayingState : PlayerBaseState
{
    
    private enum SlicingState
    {
        Lineup,
        Aim,
        Slice, 
        Waiting
    }
    private SlicingState _currentState;

    private Vector2 _aimPosition;
    private Vector2[] _directions = new Vector2[2];
    PlayerManager _player;
    private Vector2 _targetPos;
    private bool _hasMoved = false;
    private bool _half = false;
    public override void OnEnterState(PlayerManager player)
    {
        _aimPosition = player.Pizza.GetComponent<Pizza>()._basePosition;
        player.Pizza.GetComponent<Pizza>().PizzaSpawn += () => { if (_currentState == SlicingState.Waiting) { _currentState = SlicingState.Lineup; } };
        _currentState = SlicingState.Waiting;
        _player = player;
        _hasMoved = true;
       // player.Line.transform.localScale = new Vector2(player.transform.localScale.x, player.Radius);
       // player.Line.localPosition = new Vector2(0, player.Radius / 2);  
    }

    public override void OnExitState(PlayerManager player)
    {
    }

    public override void OnUpdateState(PlayerManager player)
    {
        switch (_currentState){
            case SlicingState.Lineup:
                // if(player.Controls.CurrentDirection != Vector2.zero)
                if (_targetPos != _aimPosition + player.Controls.CurrentDirection * player.Radius)
                {
                    OnMoveCutter(_aimPosition + player.Controls.CurrentDirection * player.Radius);
                }
                _targetPos = _aimPosition + player.Controls.CurrentDirection * player.Radius;
                
                if (player.Controls.IsHoldingFinish)
                {
                    _currentState = SlicingState.Waiting;
                    _hasMoved = false;
                    break;
                }
                if (player.Controls.IsHoldingSlice && player.Controls.CurrentDirection != Vector2.zero)
                {
                    _directions[0] = player.Controls.CurrentDirection;
                    _currentState = SlicingState.Aim;

                }
                break;
            case SlicingState.Aim:
                Aim(player);

                if (!player.Controls.IsHoldingSlice && player.Controls.CurrentDirection.magnitude > 0)
                {
                    _directions[1] = player.Controls.CurrentDirection;
                    _hasMoved = false;
                    _currentState = SlicingState.Slice;
                    break;
                }


                break;
            case SlicingState.Slice:

                if (_hasMoved == false)
                {
                    _hasMoved = true;
                    _player.transform.DOMove(_aimPosition, _player.FirstMoveTween.duration).SetEase(_player.FirstMoveTween.ease).onComplete += HalfWayTween;
                    _half = false;
                    _player._swipeSource.Play();

                }
                player.Line.SetPosition(0, player.transform.position);
                if(_half)
                    player.Line.SetPosition(1, player.transform.position);
                break;
            case SlicingState.Waiting:
                player.transform.position = _aimPosition + player.Controls.CurrentDirection * player.Radius;
                if (!_hasMoved)
                {
                    _hasMoved = true;
                    _player.Pizza.SendMessage("FinishPizza");
                }
                break;
        }

    }

    private void HalfWayTween()
    {
        _half = true;
        _player.Pizza.SendMessage("MakeSlice", _directions);
        _player.transform.DOMove(_player.Line.GetPosition(2), _player.SecondMoveTween.duration).SetEase(_player.SecondMoveTween.ease).onComplete += FinishedTween;

    }
    private void FinishedTween()
    {
        _currentState = SlicingState.Lineup;

    }
    private void Aim(PlayerManager player)
    {
        Vector3[] _positions = new Vector3[3];
        _positions[0] = player.transform.position;
        _positions[1] = _aimPosition;
        _targetPos = (Vector2)_aimPosition + player.Controls.CurrentDirection * player.Radius;
        if ((Vector2)player.Line.GetPosition(2) != _targetPos && _targetPos.magnitude > 0)
        {
            _player._aimSource.Play();
        }

         _positions[2] = _targetPos;
         player.Line.SetPositions(_positions);
    }

    private void OnMoveCutter(Vector2 pos)
    {
        _player._angleSource.Play();
        _player.transform.DOMove(pos, _player.SetAngleTween.duration).SetEase(_player.SetAngleTween.ease);
    }
}
