using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBullet : MonoBehaviour
{
  [SerializeField] private GameObject effectObject_ground = default;
  [SerializeField] private GameObject effectObject = default;
  [SerializeField] private GameObject Audio_Object = default;
  [SerializeField] private float deleteTime = 1.0f;
  [SerializeField] private float bullet_time = 1.0f;
  [SerializeField] private float offset = default;
  private float bullet_time_now;

  Rigidbody rb;
  Vector3 velocity;
  Vector3 angularVelocity;

  GameManager gamemanager;


  void Start(){

      rb = GetComponent<Rigidbody>();
      gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
      rb.isKinematic = false;
      bullet_time_now = 0f;
  }

  void Update(){
      if (gamemanager.game_stop_flg == true){
          if(!rb.isKinematic){
              velocity = rb.velocity;
              angularVelocity = rb.angularVelocity;
              rb.isKinematic = true;
          }
          // Debug.Log("false");
          // Debug.Log(rb.velocity);
      }
      else{
          bullet_time_now += Time.deltaTime;
          if(rb.isKinematic){
              rb.velocity = velocity;
              rb.angularVelocity = angularVelocity;
              rb.isKinematic = false;
          }
          if(bullet_time_now >= bullet_time){
              Destroy(gameObject);
          }
      }
      //Debug.Log(rb.velocity);
  }
  //当たり判定メソッド
  private void OnCollisionEnter(Collision collision)
  {

    if(collision.gameObject.CompareTag("Ground"))
    {
      Instantiate(Audio_Object, transform.position, transform.rotation);
      Destroy(gameObject);
      var instantiateEffect = GameObject.Instantiate(effectObject_ground, transform.position + new Vector3(0f,-0.45f,0f), Quaternion.identity) as GameObject;
      Destroy(instantiateEffect, deleteTime);
    }
    else{
      Instantiate(Audio_Object, transform.position, transform.rotation);
      var instantiateEffect = GameObject.Instantiate(effectObject, transform.position + new Vector3(0f,offset,0f), Quaternion.identity) as GameObject;
      Destroy(instantiateEffect, deleteTime);
    }

  }
}
