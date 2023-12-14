using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private PlayerInput input;

    [SerializeField] private InputActionReference MoveReference;
    [SerializeField] private InputActionReference LookReference;

    void Awake()
    {
        input = GetComponent<PlayerInput>();

       
        input.actions[MoveReference.action.name].performed += OnMove;
        input.actions[LookReference.action.name].performed += OnLook;

       
        InputSystem.onDeviceChange += DeviceCheck;
    }

    void OnDestroy()
    {
        
        input.actions[MoveReference.action.name].performed -= OnMove;
        input.actions[LookReference.action.name].performed -= OnLook;
        InputSystem.onDeviceChange -= DeviceCheck;
    }

    private void DeviceCheck(InputDevice device, InputDeviceChange change)
    {
        
        if (device is Gamepad)
        {
            if (change == InputDeviceChange.Added)
            {
                Debug.Log("Gamepad verbonden!");
                
            }
            else if (change == InputDeviceChange.Removed)
            {
                Debug.Log("Gamepad losgekoppeld!");
                
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log($"OnMove ({context.control}); {context.ReadValue<Vector2>()}");
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        Debug.Log($"Look ({context.control}); {context.ReadValue<Vector2>()}");
    }
}
