using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState
{
    public abstract void OnEnterState(PlayerManager player);
    public abstract void OnUpdateState(PlayerManager player);

    public abstract void OnExitState(PlayerManager player);

}
