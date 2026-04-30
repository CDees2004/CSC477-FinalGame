using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Collections.Generic;


using UIState = FsmUIState;
using UnityEngine.UIElements;

public enum FsmUIState
{
    START_SCREEN,
    IN_GAME,
    PAUSED,
    GAME_OVER,
    GAME_WIN,
}

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
    }

    // Takes in UI element as arg, set it and only it active
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

    // handles state changing for the UI FSM
    public void ChangeUIState(UIState newState)
    {
        switch (newState)
        {
            case UIState.START_SCREEN:
                SetUIElement("StartScreen");
                break;

            // if they are in the game, we use a 
            // panel name that doesn't exit to turn them all off 
            case UIState.IN_GAME:
                SetUIElement("");
                break;

            case UIState.PAUSED:
                SetUIElement("PauseScreen");
                break;

            case UIState.GAME_OVER:
                SetUIElement("GameOverScreen");
                break;

            case UIState.GAME_WIN:
                SetUIElement("GameWinScreen");
                break;
        }
    }
}
