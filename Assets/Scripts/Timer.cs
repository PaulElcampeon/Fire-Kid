using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private float timePlayedThisSession;

    private bool shouldStartTimer;

    public static Timer instance;

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

    void Update()
    {
        if (!shouldStartTimer) return;

        timePlayedThisSession += Time.deltaTime;
    }

    public void StartSessionTimer()
    {
        shouldStartTimer = true;
    }

    //Returns time played this session to the nearest second
    public float GetTimePlayedThisSession()
    {
        return Mathf.Round(timePlayedThisSession);
    }

    public float ConvertToMinutes(float seconds)
    {
        return 0;
    }

    public float ConvertToHours(float seconds)
    {
        return 0;
    }

    public float ConvertToDays(float seconds)
    {
        return 0;
    }

    public float ConvertToWeeks(float seconds)
    {
        return 0;
    }
}
