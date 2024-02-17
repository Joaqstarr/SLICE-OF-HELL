using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlayingState : PlayerBaseState
{
    private enum SlicingState
    {
        Lineup,
        Aim,
        Slice
    }
    private SlicingState _currentState;
    public override void OnEnterState(PlayerManager player)
    {
        _currentState = SlicingState.Lineup;

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
                player.transform.position = (Vector2)player.Pizza.transform.position + player.Controls.CurrentDirection * player.Radius;
                if(player.Controls.IsHoldingSlice)
                    _currentState = SlicingState.Aim;
                break;
            case SlicingState.Aim:
                if (!player.Controls.IsHoldingSlice)
                {
                    _currentState = SlicingState.Slice;
                    break;
                }

                Aim(player);

                break;
            case SlicingState.Slice:
                Debug.Log("SLICE");
                _currentState = SlicingState.Lineup;
                break;
        }

    }

    private void Aim(PlayerManager player)
    {
        Vector3[] _positions = new Vector3[3];
        _positions[0] = player.transform.position;
        _positions[1] = player.Pizza.position;
        _positions[2] = (Vector2)player.Pizza.transform.position + player.Controls.CurrentDirection * player.Radius;
         player.Line.SetPositions(_positions);
    }
}
