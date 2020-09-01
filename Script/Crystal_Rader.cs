using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Rader : MonoBehaviour
{
    private GameObject player;
    private PlayMotion playmotion;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playmotion = player.GetComponent<PlayMotion>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {

        //Debug.Log(other);
        if (other.CompareTag("Player"))
        {
            //Debug.Log("クリスタルの近くにいます！");
            playmotion.Crystal_near_flg = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {

        //Debug.Log(other);
        if (other.CompareTag("Player"))
        {
            //Debug.Log("クリスタルの近くにいます！");
            playmotion.Crystal_near_flg = false;
        }
    }
}
