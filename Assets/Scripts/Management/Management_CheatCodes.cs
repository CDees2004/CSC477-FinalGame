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
        // Win
        if (inputActions.UI.CheatCode_GameWin.WasPressedThisFrame())
        {
            print("Numpad 1 pressed");
            Management_Game.Instance.ChangeUIState(FsmUIState.GAME_WIN);
        }

        // Lose
        if (inputActions.UI.CheatCode_GameLose.WasPressedThisFrame()) Management_Game.Instance.ChangeUIState(FsmUIState.GAME_OVER);

        // Pause - Not really a CheatCode but it's simpler to put it here
        // Only allow pausing when the state is playing
        if (inputActions.UI.PauseGame.WasPressedThisFrame() && Management_Game.Instance.UIState == FsmUIState.IN_GAME)
        {
            print("Pause button pressed. State: IN_GAME");
            Management_Game.Instance.ChangeUIState(FsmUIState.PAUSED);
        }
        // Unpausing
        else if (inputActions.UI.PauseGame.WasPressedThisFrame() && Management_Game.Instance.UIState == FsmUIState.PAUSED)
        {
            Management_Game.Instance.ChangeUIState(FsmUIState.IN_GAME);
        }

        // Clear current room
        if (inputActions.UI.CheatCode_ClearRoom.WasPressedThisFrame())
        {
            if (Management_Rooms.Instance.CurrentRoom != null)
            {
                Management_Rooms.Instance.CurrentRoom.ForceClearRoom();
            }
        }
    }
}
