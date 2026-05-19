using HighScore;
using System.Collections.Generic;
using TMPro;
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
    // Set in Inspector
    public List<GameObject> PanelsUI;
    public GameObject settingsPanel;
    public TMP_InputField nameInput;
    public GameObject namePromptText;

    private string playerName;
    private Dictionary<string, GameObject> uiCache = new();

    // settings
    public bool ReduceFlashing;

    private void Awake()
    {
        // setup high score
        HS.Init(this, "Last Light");

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
        UIState = FsmUIState.START_SCREEN;


        // Disabling the settings panel at first in case it's left on 
        settingsPanel.SetActive(false);
    }

    private void Start()
    {
        namePromptText.SetActive(false);

        ReduceFlashing = false;
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
        UIState = newState; // Actually changing the state!

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
                // Using the actual high score implementation
                HS.SubmitHighScore(this,playerName, Score.Instance.score);
                print($"End score: {Score.Instance.score}");
                break;

            case UIState.GAME_WIN:
                SetUIElement("GameWinScreen");
                // Using the actual high score implementation
                HS.SubmitHighScore(this, playerName, Score.Instance.score);
                break;
        }
    }

    // If we want to do a win condition instead of endless, call this check somewhere
    public void CheckWinCondition()
    {
        print("Checking win condition.");
        // Checking just == rather than <= to allow for endless mode
        // ONLY check if we are in an enemy room
        if (Management_Rooms.clearedEnemyRooms == 8 && Management_Rooms.Instance.CurrentRoom.roomType == RoomType.ENEMY) ChangeUIState(UIState.GAME_WIN);
    }

    // Wrapper methods added for button inspector use 
    // --- these methods need to be PUBLIC to show up in the inspector
    public void StartGame()
    {
        // Need to check if they entered a name on the main menu screen first
        // If they have not, toggle a text element prompting them to
        ReadPlayerName();

        if (string.IsNullOrWhiteSpace(playerName))
        {
            namePromptText.SetActive(true);
            return;
        }
           
        ChangeUIState(UIState.IN_GAME);
        SoundManager.Play(SoundType.MENU_CLICK);
    }

    public void GameOver()
    {
        ChangeUIState(UIState.GAME_OVER);
    }

    // Additional methods for use with the OnClick() 
    // in the Button's inspector widget 
    public void QuitGame()
    {
        SoundManager.Play(SoundType.MENU_CLICK);
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
    Application.Quit(); 
#endif 
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SoundManager.Play(SoundType.MENU_CLICK);
    }

    public void OpenSettings()
    {
        // Additional popup nested inside of Paused menu
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        SoundManager.Play(SoundType.MENU_CLICK);
    }

    public void ToggleFlashing()
    {
        if (ReduceFlashing) {
            ReduceFlashing = false;
        } else {
            ReduceFlashing = true;
        }
        SoundManager.Play(SoundType.MENU_CLICK);
    }

    // Putting the game state back to IN_GAME
    // Win condition no longer triggers, player keeps going until losing.
    public void ContinueGame()
    {
        ChangeUIState(UIState.IN_GAME);
        SoundManager.Play(SoundType.MENU_CLICK);
    }

    // For getting the players name from the entry field on the main menu
    public void ReadPlayerName()
    {
         playerName = nameInput.text;
    }
}
