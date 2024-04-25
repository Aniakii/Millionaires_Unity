using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    public float timer = 300f;
    public Text timerText;
    public bool isGameStarted = false;

    void Start()
    {
        
    }

    void Update()
    {
        if (isGameStarted)
        {
            if (timer > 0)
                    {
                        timer -= Time.deltaTime;
                        int minutes = Mathf.FloorToInt(timer / 60);
                        int seconds = Mathf.FloorToInt(timer % 60);
                        timerText.text = $"{minutes}min {seconds}s";
                    }
             else
                    {
                        timerText.text = "Koniec czasu!";
                    }
        }
        

    }

    public void restartTimer()
    {
        timer = 300f;
        isGameStarted = true;
    }
}
