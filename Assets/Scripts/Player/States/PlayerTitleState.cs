using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTitleState : PlayerBaseState
{
    public override void OnEnterState(PlayerManager player)
    {
        player.transform.GetChild(0).gameObject.SetActive(false);
    }

    public override void OnExitState(PlayerManager player)
    {
        player.transform.GetChild(0).gameObject.SetActive(true);

    }

    public override void OnUpdateState(PlayerManager player)
    {
    }
}
