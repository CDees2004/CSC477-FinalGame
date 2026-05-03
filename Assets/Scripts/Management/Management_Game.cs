using UnityEngine;

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

    public int roomsCleared { get; private set; }
    public int runSeed; 

    private void Awake()
    {
        Instance = this; 
    }

    public void StartRun()
    {
        runSeed = Random.Range(0, int.MaxValue);
        roomsCleared = 0; 

        // load the starting room 
    }

    public void ClearRoom()
    {
        roomsCleared++; 
    }

    public void EndRun()
    {
        // resetting
        StartRun(); 
    }
}
