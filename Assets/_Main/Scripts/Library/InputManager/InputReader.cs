using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System;

namespace ScriptLibrary.InputManager
{
    public abstract class InputReader : MonoBehaviour
    {
        public List<InputAction> PlayerMovementAction;

        /// <summary>
        /// To be used in a Monobehaviour Object and called by other objects for accessing variables
        /// </summary>
        /// <param name="reference"> Input key to be monitored </param>
        /// <param name="callbackContextFucntion"> function thats called during input </param>
        /// <example>
        /// [SerializeField] private InputActionReference move;
        /// public Vector2 MovementInput;
        /// RegisterAction(reference: move, callbackContextFucntion: ctx => Move = ctx.ReadValue<Vector2>());
        /// </example>
        protected virtual void RegisterAction(InputActionReference reference, Action<InputAction.CallbackContext> callbackContextFucntion)
        {
            InputAction action = reference.action; // get the action from the action reference

            //enable and add the fun
            action.Enable();
            action.performed += callbackContextFucntion;
            action.canceled += callbackContextFucntion;

            //add to the list for disabling later
            PlayerMovementAction.Add(reference.action);
        }

        protected virtual void OnDisable()
        {
            //loop through function and disable all the 
            for (int i=0; i<PlayerMovementAction.Count; i++)
            {
                PlayerMovementAction[i].Disable();
            }
        }


    }
}
