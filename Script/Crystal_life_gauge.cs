using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Crystal_life_gauge : MonoBehaviour
{
    GameObject image;
    // Start is called before the first frame update
    void Start()
    {
        image = GameObject.Find("Crystal_Gauge_Image");

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HPDown(float current, int max){
        image.GetComponent<Image>().fillAmount = current / max;
    }
}
