using System;
using UnityEngine;
using UnityEngine.InputSystem;
using ScriptLibrary.InputManager;

public class InputHandler : InputReader
{
    [SerializeField] private InputActionReference clickInputReference;
    [SerializeField] private InputActionReference pointerLocationInputReference;
    
    
    [NonSerialized] public float ClickActionValue;
    [NonSerialized] public Vector2 PointerLocationValue;
    
    private void OnEnable()
    {
        RegisterAction(clickInputReference, ctx => ClickActionValue = ctx.ReadValue<float>(), ctx => ClickActionValue = 0f);
        RegisterAction(pointerLocationInputReference, ctx => PointerLocationValue = ctx.ReadValue<Vector2>(),
            ctx => { });
    }

}
