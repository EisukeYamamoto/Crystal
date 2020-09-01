using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitMateria : MonoBehaviour
{

    [SerializeField] private GameObject effectObject = default;
    [SerializeField] private float deleteTime = 1.0f;
    [SerializeField] private GameObject Audio_Object = default;
    [SerializeField] private float offset = default;
    private GameObject player;
    PlayMotion playermotion;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playermotion = player.GetComponent<PlayMotion>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision);
        if (collision.gameObject.CompareTag("Player") && playermotion.Get_Materia_flg == false){
            Instantiate(Audio_Object, transform.position, transform.rotation);
            Destroy(gameObject);
            var instantiateEffect = GameObject.Instantiate(effectObject, transform.position + new Vector3(0f,offset,0f), Quaternion.identity) as GameObject;
            Destroy(instantiateEffect, deleteTime);
            playermotion.Get_Materia_flg = true;
        }

    }
}
