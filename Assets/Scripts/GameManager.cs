using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private float timeAlreadyPlayed;

    public static GameManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this);
    }

    void Start()
    {
        LoadGameData();
    }

    void Update()
    {
        
    }

    private void SaveGameData()
    {
        SaveTimeGamePlayed();
    }

    private void LoadGameData()
    {
        timeAlreadyPlayed = PlayerPrefs.GetFloat("time_played");

        Debug.Log("Loading Game Data");
    }

    public void SaveTimeGamePlayed()
    {
        float timePlayedThisSession = Timer.instance.GetTimePlayedThisSession();

        if (PlayerPrefs.HasKey("time_played"))
        {
            float combinedTimePlayed = timePlayedThisSession + timeAlreadyPlayed;

            PlayerPrefs.SetFloat("time_played", combinedTimePlayed);
        }
        else
        {
            PlayerPrefs.SetFloat("time_played", timePlayedThisSession);
        }

        Debug.Log("Saving Time Played");
    }

    private void StartGamePlayTimer()
    {
        Timer.instance.StartSessionTimer();

        Debug.Log("Starting timer");
    }

    public void Pause()
    {
        Time.timeScale = 0f;
    }

    public void UnPause()
    {
        Time.timeScale = 1f;
    }

    public void ExitGame()
    {
        Debug.Log("Quiting Game...");

        Application.Quit();
    }
}
