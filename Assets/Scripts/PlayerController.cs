using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

# if UNITY_EDITOR
using UnityEditor;
# endif

[RequireComponent(typeof(PlayerInput)), RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    Rigidbody rb;

    InputActionAsset actions;

    public Transform neckBoneYaw;
    public Transform neckBonePitch;

    OnGround onGround;

    public float moveSpeed = 2f;
    public float rotateSpeed = 5f;

    public bool inverseYaw = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        actions = GetComponent<PlayerInput>().actions;

        onGround = GetComponentInChildren<OnGround>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        rb.angularVelocity = Vector3.zero;

        if (onGround.onGround)
        {
            Move();
        }

        Rotate();
    }

    void Move()
    {
        var lastY = rb.velocity.y;

        var actionMove = actions["Move"].ReadValue<Vector2>();
        var move = new Vector3(actionMove.x, 0, actionMove.y);

        rb.velocity = neckBoneYaw.TransformVector(move).normalized * moveSpeed + Vector3.up * lastY;
    }

    void Rotate()
    {
        var actionRotate = actions["Rotate"].ReadValue<Vector2>();

        transform.Rotate(0, actionRotate.x * Time.deltaTime * rotateSpeed, 0);
        neckBonePitch.Rotate(actionRotate.y * (inverseYaw ? -1 : 1) * Time.deltaTime * rotateSpeed, 0, 0);

        float neckX = neckBonePitch.localRotation.eulerAngles.x;
        float neckY = neckBonePitch.localRotation.eulerAngles.y;
        // Debug.Log("neckX: "+ neckBonePitch.localRotation.eulerAngles);
        if (neckY > 90)
        {
            if (neckX > 180)
            {
                neckBonePitch.localRotation = Quaternion.Euler(270, 0, 0);
            }
            else
            {
                neckBonePitch.localRotation = Quaternion.Euler(90, 0, 0);
            }
        }
    }

#if UNITY_EDITOR
    // エディターでのマウス固定
    static void AutoFocusGameViewOnPlay()
    {
        EditorApplication.playModeStateChanged += (PlayModeStateChange state) =>
        {
            // If we're about to run the scene...
            if (state == PlayModeStateChange.EnteredPlayMode)
            {
                var gameWindow = EditorWindow.GetWindow(typeof(EditorWindow).Assembly.GetType("UnityEditor.GameView"));
                gameWindow.Focus();
                gameWindow.SendEvent(new Event
                {
                    button = 0,
                    clickCount = 1,
                    type = EventType.MouseDown,
                    mousePosition = gameWindow.rootVisualElement.contentRect.center
                });

            }
        };
    }
#endif


}
