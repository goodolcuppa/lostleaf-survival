using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction moveAction;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        
    }

    void Update()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        transform.position += new Vector3(input.x, input.y, 0f) * Time.deltaTime * 3f;
    }
}
