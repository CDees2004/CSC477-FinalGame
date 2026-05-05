using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

/*
 *  Handles the cheat codes used across the game as a 
 *  form of universal input to change game state 
 *  without requiring play for debugging purposes.
 */

public class Management_CheatCodes : MonoBehaviour
{
    // Set in inspector
    public Player player;
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
            Management_Rooms.clearedRooms = 8;
            Management_Game.Instance.CheckWinCondition();
        }

        // Lose
        if (inputActions.UI.CheatCode_GameLose.WasPressedThisFrame()) Management_Game.Instance.ChangeUIState(FsmUIState.GAME_OVER);

        // Pause - Not really a CheatCode but it's simpler to put it here
        // Only allow pausing when the state is playing
        if (inputActions.UI.PauseGame.WasPressedThisFrame() && Management_Game.Instance.UIState == FsmUIState.IN_GAME)
        {
            print("Pause button pressed. State: IN_GAME");
            Management_Game.Instance.ChangeUIState(FsmUIState.PAUSED);
            Time.timeScale = 0.0f;
        }
        // Unpausing
        else if (inputActions.UI.PauseGame.WasPressedThisFrame() && Management_Game.Instance.UIState == FsmUIState.PAUSED)
        {
            Management_Game.Instance.ChangeUIState(FsmUIState.IN_GAME);
            Time.timeScale = 1.0f;
        }

        // Clear current room
        if (inputActions.UI.CheatCode_ClearRoom.WasPressedThisFrame())
        {
            if (Management_Rooms.Instance.CurrentRoom != null)
            {
                Management_Rooms.Instance.CurrentRoom.ForceClearRoom();
            }
        }

        // Testing damage and healing
        if (inputActions.UI.CheatCode_TakeDamage.WasPressedThisFrame())
        {
            if (player != null)
            {
                player.TakeDamage(20.0f);
                print("Player took 20 damage via CheatCode");
            }
        }

        if (inputActions.UI.CheatCode_Heal.WasPressedThisFrame())
        {
            if (player != null)
            {
                player.HealPlayer(20.0f);
                print("Player healed 20 health via CheatCode");
            }
        }
    }
}
