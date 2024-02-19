using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{

    public Vector2 CurrentDirection;
    public bool IsHoldingSlice = false;
    public bool IsHoldingFinish = false;

    // Start is called before the first frame update
    void Start()
    {
        PlayerInput input = GetComponent<PlayerInput>();
        var actions = input.actions;
        actions.devices = new[]{Keyboard.current};
        input.actions = actions;
        
    }

    public void OnDirection(InputValue Value)
    {
        CurrentDirection = Value.Get<Vector2>();
    }

    public void OnSlice(InputValue Value)
    {
        IsHoldingSlice = Value.isPressed;

    }
    public void OnFinish(InputValue Value)
    {
        IsHoldingFinish = Value.isPressed;

    }

    public void OnQuit()
    {
        Application.Quit();
    }

    public void OnP1(InputValue Value)
    {
        if (!GameManager.Instance.GameStarted)
        {
            GameManager.Instance.StartGame1P();
        }
    }

    public void OnP2(InputValue Value)
    {
        if (!GameManager.Instance.GameStarted)
        {
            GameManager.Instance.StartGame2P();
        }
    }
}
