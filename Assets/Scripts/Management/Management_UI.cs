using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Collections.Generic;

using UIState = FsmUIState;


public enum FsmUIState
{
    START_SCREEN,
    IN_GAME,
    PAUSED,
    IN_SETTINGS,
    GAME_OVER,
    GAME_WIN,
}

/*
 *  NOTE: UI panel names are required to be exactly as detailed in the 
 *  ChangeUIState method. Name them appropraitely in the inspector. 
 */

public class Management_UI : MonoBehaviour
{
    // Singleton because management script 
    public static Management_UI Instance { get; private set; }
    public UIState UIState { get; private set; }

    public List<GameObject> PanelsUI;
    private Dictionary<string, GameObject> uiCache = new();

    private void Awake()
    {
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
                SetUIElement("");
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

    }
}
