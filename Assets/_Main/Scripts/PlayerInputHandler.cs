using UnityEngine;
using UnityEngine.InputSystem;
using ScriptLibrary.InputManager;
using System;

public class PlayerInputHandler : InputReader
{
    [SerializeField] private InputActionReference clickInputReference;
    [SerializeField] private InputActionReference dragInputReference;
    
    [NonSerialized] public Action OnRightClickDown;
    [NonSerialized] public Action OnRightClickUp;
    [NonSerialized] public Vector2 MousePointerLocation;

    void OnEnable()
    {
        RegisterAction(clickInputReference, ctx =>
            {
                if (ctx.control.IsPressed()){
                    OnRightClickDown?.Invoke();
                }
                else{
                    OnRightClickUp?.Invoke();
                }
            });
        RegisterAction(dragInputReference, ctx => MousePointerLocation = ctx.ReadValue<Vector2>());
    }
}
