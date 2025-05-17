using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private InputSystem_Actions inputActions;
    private GameObject selectedObject; // Mevcut kýrmýzý objeyi tutar

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Gameplay.Tap.performed += OnTapPerformed;
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void OnTapPerformed(InputAction.CallbackContext context)
    {
        Vector2 screenPos = GetInputScreenPosition();
        Ray ray = Camera.main.ScreenPointToRay(screenPos);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GameManager.Instance.HandleClick(hit);
        }
    }

    private Vector2 GetInputScreenPosition()
    {
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
            return Touchscreen.current.primaryTouch.position.ReadValue();
        else if (Mouse.current != null && Mouse.current.leftButton.isPressed)
            return Mouse.current.position.ReadValue();

        return Vector2.zero;
    }


}
