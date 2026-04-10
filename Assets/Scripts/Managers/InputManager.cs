using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static event Action<float> OnPlayer1Move;
    public static event Action<float> OnPlayer2Move;
    public static event Action OnPause;

    private float Player1Input;
    private float Player2Input;

    private void Update()
    {
        GetPlayer1Input();
        GetPlayer2Input();
        GetPauseInput();
    }

    private void GetPlayer1Input()
    {
        if (Input.GetKey(KeyCode.A))
        {
            Player1Input = -1f;
        }

        else if (Input.GetKey(KeyCode.D))
        {
            Player1Input = 1f;
        }

        else
        {
            Player1Input = 0f;
        }

        OnPlayer1Move?.Invoke(Player1Input);
    }

    private void GetPlayer2Input()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Player2Input = -1f;
        }

        else if (Input.GetKey(KeyCode.RightArrow))
        {
            Player2Input = 1f;
        }

        else
        {
            Player2Input = 0f;
        }

        OnPlayer2Move?.Invoke(Player2Input);
    }


    private void GetPauseInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnPause?.Invoke();
        }
    }
}