using UnityEngine;

/*
 * The Game Manager handles tracking the overall game state
 * determining if the win or lose condition has ever been reached. 
 * 
 * Some functionality is as simple as triggering events in the 
 * UI manager. This is done to maintain clarity. 
 */

public enum FSMGameState
{
    PLAYING, 
    SELECTING_UPGRADE, 
    WIN, 
    LOSE, 
}

public class Management_Game : MonoBehaviour
{


    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}
