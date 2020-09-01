using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Alarm_Manager : MonoBehaviour
{

  private GameObject player;
  private PlayMotion playmotion;

  [SerializeField] GameObject materiaPanel = default;
  [SerializeField] GameObject materiaPanel2 = default;

    // Start is called before the first frame update
    void Start()
    {
      player = GameObject.Find("Player");
      playmotion = player.GetComponent<PlayMotion>();
      materiaPanel.SetActive(false);
      materiaPanel2.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if(playmotion.Get_Materia_flg == true && playmotion.Crystal_near_flg == false){
          materiaPanel.SetActive(true);
          materiaPanel2.SetActive(false);
        }
        else if(playmotion.Get_Materia_flg == true && playmotion.Crystal_near_flg == true){
          materiaPanel.SetActive(false);
          materiaPanel2.SetActive(true);
        }
        else{
          materiaPanel.SetActive(false);
          materiaPanel2.SetActive(false);
        }

    }
}
