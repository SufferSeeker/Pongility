using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static event Action OnPlayer1PreviousAbilitySlot;
    public static event Action OnPlayer1NextAbilitySlot;
    public static event Action OnPlayer1UseSelectedAbility;

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

        GetPlayer1AbilitySlotInput();
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

    private void GetPlayer1AbilitySlotInput()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            OnPlayer1PreviousAbilitySlot?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            OnPlayer1NextAbilitySlot?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            OnPlayer1UseSelectedAbility?.Invoke();
        }
    }
}