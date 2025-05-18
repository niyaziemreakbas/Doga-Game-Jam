using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControllerManager : MonoBehaviour
{
    public float moveSpeed = 10f;

    private InputSystem_Actions controls;
    private Vector2 moveInput;
    private bool isTouching = false;
    private Vector2 lastTouchPos;

    void Awake()
    {
        controls = new InputSystem_Actions();

        // Girişleri oku
        controls.Camera.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Camera.Move.canceled += _ => moveInput = Vector2.zero;
    }

    void OnEnable() => controls.Enable();
    void OnDisable() => controls.Disable();

    void Update()
    {
        // 🎮 Klavye veya gamepad hareketi (sabit dünya yönüne göre)
        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y);
        transform.position += move * moveSpeed * Time.deltaTime;

        // 📱 Dokunmatik hareket
        if (Touchscreen.current != null && Touchscreen.current.touches.Count > 0)
        {
            var touch = Touchscreen.current.primaryTouch;

            if (touch.press.isPressed)
            {
                Vector2 currentPos = touch.position.ReadValue();

                if (!isTouching)
                {
                    lastTouchPos = currentPos;
                    isTouching = true;
                }

                Vector2 delta = currentPos - lastTouchPos;

                // Sabit yönlere göre hareket (ekran sola kayarsa kamera sola gider)
                Vector3 touchDir = new Vector3(-delta.x, 0, -delta.y) * 0.01f;
                transform.position += touchDir * moveSpeed * Time.deltaTime;

                lastTouchPos = currentPos;
            }
            else
            {
                isTouching = false;
            }
        }
    }
}
