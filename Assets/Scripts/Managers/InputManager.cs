using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    #region Events
    public static event Action<float> OnPlayer1Move;
    public static event Action<float> OnPlayer2Move;

    public static event Action<int> OnPlayer1AbilitySlotInput;
    public static event Action<int> OnPlayer2AbilitySlotInput;

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
            OnPlayer1AbilitySlotInput?.Invoke(0);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            OnPlayer1AbilitySlotInput?.Invoke(1);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            OnPlayer1AbilitySlotInput?.Invoke(2);
        }
    }

    private void GetPlayer2AbilitySlotInput()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            OnPlayer2AbilitySlotInput?.Invoke(0);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            OnPlayer2AbilitySlotInput?.Invoke(1);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            OnPlayer2AbilitySlotInput?.Invoke(2);
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