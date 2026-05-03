using UnityEditor;
using UnityEngine;

/*
 *  Handles the cheat codes used across the game as a 
 *  form of universal input to change game state 
 *  without requiring play for debugging purposes.
 */

public class Management_CheatCodes : MonoBehaviour
{
    private PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions(); 
    }

    private void OnEnable()
    {
        inputActions.UI.Enable();
    }

    private void OnDisable()
    {
        inputActions.UI.Disable();
    }

    private void Update()
    {
        if (inputActions.UI.CheatCode_GameWin.WasPressedThisFrame())
        {
            print("Numpad 1 pressed");
            Management_Game.Instance.ChangeUIState(FsmUIState.GAME_WIN);
        }

        if (inputActions.UI.CheatCode_GameLose.WasPressedThisFrame()) Management_Game.Instance.ChangeUIState(FsmUIState.GAME_OVER);
    }
}
