using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static event Action<float> OnMove;
    public static event Action OnPause;

    private float HorizontalInput;

    private void Update()
    {
        GetMoveInput();
        GetPauseInput();
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

    private void GetPauseInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnPause?.Invoke();
        }
    }
}