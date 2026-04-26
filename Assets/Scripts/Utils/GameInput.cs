using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{

    private InputSystem_Actions inputActions;

    public static GameInput Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
        inputActions = new InputSystem_Actions();
        inputActions.Player.Enable();
    }

    private void OnDestroy()
    {
        if (inputActions != null)
        {
            inputActions.Player.Disable();

            inputActions.Dispose();
        }
    }

    private void OnDisable()
    {
        if (inputActions != null)
        {
            inputActions.Player.Disable();
        }
    }

    /// <summary>
    /// Returns the movement vector based on player input.
    /// </summary>
    /// <returns> 
    //  Vector2 representing the movement direction.
    // </returns>
    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 input = inputActions.Player.Move.ReadValue<Vector2>();
        return input.normalized;
    }

    /// <summary>
    /// Returns the look vector based on player input.
    /// </summary>
    /// <returns>
    /// Vector2 representing the look direction.
    // </returns>
    public Vector2 GetLookVector()
    {
        Vector2 input = inputActions.Player.Look.ReadValue<Vector2>();
        return input;
    }

    /// <summary>
    /// Returns the mouse scroll wheel input.
    /// </summary>
    /// <returns>
    /// Vector2 representing the mouse scroll wheel input. Only the Y-axis is used for zooming in and out.
    /// The X-axis is typically not used for zooming.
    /// If the scroll wheel is not used, it will return (0, 0).
    // </returns>
    public Vector2 GetMouseScrollWheel()
    {
        Vector2 input = inputActions.Player.Zoom.ReadValue<Vector2>();
        return input;
    }

    public bool IsRunning()
    {
        return inputActions.Player.Sprint.IsPressed() && inputActions.Player.Move.IsPressed();
    }

    public bool IsWalking()
    {
        return inputActions.Player.Move.IsPressed() && !inputActions.Player.Sprint.IsPressed();
    }

    public bool IsShooting()
    {
        return inputActions.Player.Attack.IsPressed();
    }

    public bool IsAiming()
    {
        return inputActions.Player.Aim.IsPressed();
    }
}
