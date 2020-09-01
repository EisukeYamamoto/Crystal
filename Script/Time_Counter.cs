using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Time_Counter : MonoBehaviour
{

    [SerializeField] public float countdown = 180.0f;

    [SerializeField] public Text timeText = default;

    private int minites;
    private int seconds;
    private int mseconds;
    GameManager gamemanager;

    // Start is called before the first frame update
    void Start()
    {
      gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {
       if(gamemanager.game_stop_flg == false){
          countdown -= Time.deltaTime;

          if(countdown <= 0.0f){
              countdown = 0.0f;
              gamemanager.GameOver();
          }

          minites = Mathf.FloorToInt(countdown / 60F);

          seconds = Mathf.FloorToInt(countdown - minites * 60);

          mseconds = Mathf.FloorToInt((countdown - minites * 60 - seconds) * 100);
        }

          //Debug.Log(minites);

          timeText.text = "Time: " + string.Format("{0:00}:{1:00}:{2:00}", minites, seconds, mseconds);
    }
}
