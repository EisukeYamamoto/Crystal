using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCollider_Enemy : MonoBehaviour
{
  HitController colliderTriggerParent;
  // Start is called before the first frame update
  void Start()
  {
      GameObject objColliderTriggerParent = gameObject.transform.root.gameObject;
      colliderTriggerParent = objColliderTriggerParent.GetComponent<HitController>();
  }

  void OnTriggerEnter(Collider collider){
      colliderTriggerParent.RelayOnTriggerEnter(collider);
  }

  // Update is called once per frame
  void Update()
  {

  }
}
