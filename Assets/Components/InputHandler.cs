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
        Vector2 screenPosition = GetInputScreenPosition();

        // Ekrandan bir ray çýkar
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.collider.gameObject;

            // Eski objeyi temizle
            if (selectedObject != null && selectedObject != hitObject)
            {
                ResetColor(selectedObject);
            }

            // Yeni objeyi seç ve kýrmýzý yap
            selectedObject = hitObject;
            SetRed(selectedObject);
        }
    }

    private Vector2 GetInputScreenPosition()
    {
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            return Touchscreen.current.primaryTouch.position.ReadValue();
        }
        else if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            return Mouse.current.position.ReadValue();
        }

        return Vector2.zero;
    }

    private void SetRed(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.red;
        }
    }

    private void ResetColor(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.white;
        }
    }
}
