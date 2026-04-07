using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static event Action<float> OnMove;

    private float HorizontalInput;

    private void Update()
    {
        GetMoveInput();
    }

    private void GetMoveInput()
    {
        if (Input.GetKey(KeyCode.A))
        {
            HorizontalInput = -1f;
        }

        else if (Input.GetKey(KeyCode.D))
        {
            HorizontalInput = 1f;
        }

        else
        {
            HorizontalInput = 0f;
        }

        OnMove?.Invoke(HorizontalInput);
    }
}