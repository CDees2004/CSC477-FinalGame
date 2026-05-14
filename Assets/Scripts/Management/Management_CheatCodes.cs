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
            Management_Rooms.clearedEnemyRooms = 8;
            Management_Game.Instance.CheckWinCondition();
        }

        // Lose
        if (inputActions.UI.CheatCode_GameLose.WasPressedThisFrame()) Management_Game.Instance.ChangeUIState(FsmUIState.GAME_OVER);

        // Pause - Not really a CheatCode but it's simpler to put it here
        // Only allow pausing when the state is playing
        if (inputActions.UI.PauseGame.WasPressedThisFrame() && Management_Game.Instance.UIState == FsmUIState.IN_GAME)
        {
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
                //player.TakeDamage(20.0f);
            }
        }

        if (inputActions.UI.CheatCode_Heal.WasPressedThisFrame())
        {
            if (player != null)
            {
                player.HealPlayer(20.0f);
            }
        }

        // Kill one enemy in the current room
        if (inputActions.UI.CheatCode_KillEnemy.WasPressedThisFrame())
        {
            if (Management_Rooms.Instance.CurrentRoom != null)
            {
               // Random.Range(Management_Rooms.Instance.CurrentRoom.spawnedEnemies).KillEnemy();
            }
        }

        if (inputActions.UI.CheatCode_Points.WasPressedThisFrame())
        {
            // Adding 10,000 points to players score
            Score.Instance.UpdateScore(10_000);
        }
    }
}
