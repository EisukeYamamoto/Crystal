using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// http://kdey.cocolog-nifty.com/blog/2011/11/coliderscript-a.html
public class HitCollider_Player : MonoBehaviour
{
    PlayMotion colliderTriggerParent;
    // Start is called before the first frame update
    void Start()
    {
        GameObject objColliderTriggerParent = gameObject.transform.root.gameObject;
        colliderTriggerParent = objColliderTriggerParent.GetComponent<PlayMotion>();
    }

    void OnTriggerEnter(Collider collider){
        //Debug.Log("Player_hit");
        colliderTriggerParent.RelayOnTriggerEnter(collider);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
