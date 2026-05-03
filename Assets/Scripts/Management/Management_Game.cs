using HighScore;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UIState = FsmUIState;


public enum FsmUIState
{
    START_SCREEN,
    IN_GAME,
    PAUSED,
    IN_SETTINGS,
    SELECTING_UPGRADE,
    GAME_OVER,
    GAME_WIN,
}

/*
 *  NOTE: UI panel names are required to be exactly as detailed in the 
 *  ChangeUIState method. Name them appropraitely in the inspector. 
 */

public class Management_Game : MonoBehaviour
{
    // Singleton because management script 
    public static Management_Game Instance { get; private set; }
    public UIState UIState { get; private set; }

    public List<GameObject> PanelsUI;
    private Dictionary<string, GameObject> uiCache = new();

    private int runSeed; 

    private void Awake()
    {
        // setup high score
        HS.Init(this, "Team7");

        Instance = this;

        // storing the panels set in inspector into a map to load from 
        foreach (GameObject panel in PanelsUI)
        {
            uiCache.Add(panel.name, panel);
        }

        // Setting the initial state to the START_SCREEN
        // to force the other UI elements to turn off if they were
        // left on 
        SetUIElement("StartScreen");
    }

    // Takes in UI element as arg, set it and only it active
    // called when the state is changed by other scripts 
    private void SetUIElement(string requestedPanelName)
    {
        foreach (GameObject panel in PanelsUI)
        {
            if (requestedPanelName == panel.name)
            {
                panel.SetActive(true);
            }
            else
            {
                panel.SetActive(false);
            }
        }
    }

    // Handles state changing for the UI FSM
    public void ChangeUIState(UIState newState)
    {
        // Check if it is a redudant requested state swap and reject it
        if (newState == UIState) return;

        switch (newState)
        {
            case UIState.START_SCREEN:
                SetUIElement("StartScreen");
                break;

            // If they are in the game, we use a 
            // panel name that doesn't exit to turn them all off 
            case UIState.IN_GAME:
                SetUIElement("ScreenFade");
                break;

            case UIState.PAUSED:
                SetUIElement("PauseScreen");
                break;

            // Has different behavior due to being a NESTED UI panel
            case UIState.IN_SETTINGS:
                OpenSettings();
                break;

            case UIState.GAME_OVER:
                SetUIElement("GameOverScreen");
                break;

            case UIState.GAME_WIN:
                SetUIElement("GameWinScreen");
                break;
        }
    }


    public void CheckWinCondition()
    {
        if (Management_Rooms.Instance.clearedRooms >= 8) UIState = UIState.GAME_WIN;
    }

    private void StartRun()
    {

    }

    private void EndRun()
    {
        StartRun(); // restarting 
    }

    // Wrapper methods added for button inspector use 
    // --- these methods need to be PUBLIC to show up in the inspector
    public void StartGame()
    {
        ChangeUIState(UIState.IN_GAME);
    }

    public void GameOver()
    {
        ChangeUIState(UIState.GAME_OVER);
    }

    // Additional methods for use with the OnClick() 
    // in the Button's inspector widget 
    public void QuitGame()
    {
        print("Quit game called");
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
    Application.Quit(); 
#endif 
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OpenSettings()
    {
        // Additional popup nested inside of Settings 
        // Handle this popup's logic through the use of a Coroutine.
    }
}
