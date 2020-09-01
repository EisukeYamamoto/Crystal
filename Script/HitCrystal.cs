using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCrystal : MonoBehaviour
{

    Crystal_system colliderTriggerParent;
    // Start is called before the first frame update
    void Start()
    {
      GameObject objColliderTriggerParent = gameObject.transform.root.gameObject;
      colliderTriggerParent = objColliderTriggerParent.GetComponent<Crystal_system>();

    }

    void OnTriggerEnter(Collider collider){
        //Debug.Log(collider);
        colliderTriggerParent.RelayOnTriggerEnter(collider);
    }

    // Update is called once per frame
    void Update()
    {


    }


}
