using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{

    public Vector2 CurrentDirection;
    public bool IsHoldingSlice = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void OnDirection(InputValue Value)
    {
        CurrentDirection = Value.Get<Vector2>();
    }

    public void OnSlice(InputValue Value)
    {
        IsHoldingSlice = Value.isPressed;

    }
}
