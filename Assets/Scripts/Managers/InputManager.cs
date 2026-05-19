using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    #region Events
    public static event Action<float> OnPlayer1Move;
    public static event Action<float> OnPlayer2Move;

    public static event Action OnPlayer1PreviousAbilitySlot;
    public static event Action OnPlayer1NextAbilitySlot;
    public static event Action OnPlayer1UseSelectedAbility;

    public static event Action OnPlayer2PreviousAbilitySlot;
    public static event Action OnPlayer2NextAbilitySlot;
    public static event Action OnPlayer2UseSelectedAbility;

    public static event Action OnPause;
    #endregion

    #region Unity Methods
    private void Update()
    {
        GetPlayer1Input();
        GetPlayer2Input();

        GetPlayer1AbilitySlotInput();
        GetPlayer2AbilitySlotInput();

        GetPauseInput();
    }
    #endregion

    #region Movement Input
    private void GetPlayer1Input()
    {
        float Player1Input = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            Player1Input = -1f;
        }

        else if (Input.GetKey(KeyCode.D))
        {
            Player1Input = 1f;
        }

        OnPlayer1Move?.Invoke(Player1Input);
    }

    private void GetPlayer2Input()
    {
        float Player2Input = 0f;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Player2Input = -1f;
        }

        else if (Input.GetKey(KeyCode.RightArrow))
        {
            Player2Input = 1f;
        }

        OnPlayer2Move?.Invoke(Player2Input);
    }
    #endregion

    #region Ability Input
    private void GetPlayer1AbilitySlotInput()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            OnPlayer1PreviousAbilitySlot?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            OnPlayer1NextAbilitySlot?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            OnPlayer1UseSelectedAbility?.Invoke();
        }
    }

    private void GetPlayer2AbilitySlotInput()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            OnPlayer2PreviousAbilitySlot?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            OnPlayer2NextAbilitySlot?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            OnPlayer2UseSelectedAbility?.Invoke();
        }
    }
    #endregion

    #region Pause Input
    private void GetPauseInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnPause?.Invoke();
        }
    }
    #endregion
}