using UnityEngine;
using UnityEngine.InputSystem;

public class FollowMouse : MonoBehaviour
{
    void Awake() {
        Cursor.visible = false;
    }

    void Update() {
        transform.position = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }
}
