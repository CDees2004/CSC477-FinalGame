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
    GAME_OVER,
    GAME_WIN,
}

public class Management_UI : MonoBehaviour
{
    // Singleton because management script 
    public static Management_UI Instance { get; private set; }
    public UIState UIState { get; private set; }

    public List<GameObject> PanelsUI;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {

    }

    public void ChangeUIState(UIState newState)
    {
        var state = newState switch
        {
            UIState.START_SCREEN => "test",
        };
    }
}
