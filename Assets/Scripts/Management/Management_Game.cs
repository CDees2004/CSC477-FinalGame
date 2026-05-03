using UnityEngine;

using GameState = FSMGameState; 

public enum FSMGameState
{
    PLAYING, 
    SELECTING_UPGRADE, 
    WIN, 
    LOSE, 
}

public class Management_Game : MonoBehaviour
{
    public static Management_Game Instance;
    public GameState GameState { get; private set; }

    public int roomsCleared { get; private set; }
    public int runSeed; 

    private void Awake()
    {
        Instance = this; 

    }

    public void ChangeGameState(GameState newGameState)
    {
        if (newGameState == GameState) return;

        switch (newGameState)
        {
            case GameState.PLAYING:
                print("playing");
                break;

            case GameState.WIN:
                Management_UI.Instance.ChangeUIState(FsmUIState.GAME_WIN);
                break;
        }
    }

    public void StartRun()
    {
        runSeed = Random.Range(0, int.MaxValue);
        roomsCleared = 0; 

        // load the starting room 
    }

    public void EndRun()
    {
        // resetting
        StartRun(); 
    }

    public void CheckWinCondition()
    {
        if (Management_Rooms.Instance.clearedRooms >= 8) GameState = GameState.WIN; 
    }
}
