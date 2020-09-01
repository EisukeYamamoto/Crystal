using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannnonForward : MonoBehaviour
{
    //Plane plane = new Plane();
    //float distance = 0;
    float camRayLength = 1000f;
    //Rigidbody playerRigidbody;          // Reference to the player's rigidbody.
    int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
    int fieldMask;
    int BodyMask;
    int EnemyMask;
    int CrystalMask;

    private GameObject player;
    private PlayMotion playmotion;

    private GameObject target;
    //private float myrotatespeed = 0.1f;

    GameManager gamemanager;


    // Start is called before the first frame update
    void Start()
    {
      floorMask = LayerMask.GetMask("Ground");
      fieldMask = LayerMask.GetMask("Field");
      BodyMask = LayerMask.GetMask("Body");
      EnemyMask = LayerMask.GetMask("Enemy");
      CrystalMask = LayerMask.GetMask("Crystal");
      //playerRigidbody = GetComponent <Rigidbody> ();

      player = gameObject.transform.root.gameObject;
      playmotion = player.GetComponent<PlayMotion>();

      target = GameObject.FindWithTag("crystal_target");
      gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();


    }

    // Update is called once per frame
    void Update()
    {
      if(gamemanager.game_stop_flg == false){
        // カメラとマウスの位置を元にRayを準備
        Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);

        RaycastHit floorHit;

        if(playmotion.Get_Materia_flg == false)
        {

          if(Physics.Raycast (camRay, out floorHit, camRayLength, BodyMask))
          {
            //Debug.Log("Body");

          }
          else if(Physics.Raycast (camRay, out floorHit, camRayLength, EnemyMask))
          {
            // Create a vector from the player to the point on the floor the raycast from the mouse hit.
            Vector3 playerToMouse = floorHit.point - transform.position;

            // Ensure the vector is entirely along the floor plane.
            //playerToMouse.y = 0f;

            // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
            Quaternion newRotation = Quaternion.LookRotation (playerToMouse);

            // Set the player's rotation to this new rotation.
            //playerRigidbody.MoveRotation (newRotation);
            transform.rotation = newRotation;
            //Debug.Log("Enemy");
          }


          else if(Physics.Raycast (camRay, out floorHit, camRayLength, CrystalMask))
          {
            // Create a vector from the player to the point on the floor the raycast from the mouse hit.
            Vector3 playerToMouse = floorHit.point - transform.position;

            // Ensure the vector is entirely along the floor plane.
            //playerToMouse.y = 0f;

            // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
            Quaternion newRotation = Quaternion.LookRotation (playerToMouse);

            // Set the player's rotation to this new rotation.
            //playerRigidbody.MoveRotation (newRotation);
            transform.rotation = newRotation;
            // Debug.Log("Crystal");
          }

          else if(Physics.Raycast (camRay, out floorHit, camRayLength, floorMask))
          {
            // Create a vector from the player to the point on the floor the raycast from the mouse hit.
            Vector3 playerToMouse = floorHit.point - transform.position;

            // Ensure the vector is entirely along the floor plane.
            //playerToMouse.y = 0f;

            // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
            Quaternion newRotation = Quaternion.LookRotation (playerToMouse);

            // Set the player's rotation to this new rotation.
            //playerRigidbody.MoveRotation (newRotation);
            transform.rotation = newRotation;
            //Debug.Log("Ground");
          }

          else if(Physics.Raycast (camRay, out floorHit, camRayLength, fieldMask))
          {
            // Create a vector from the player to the point on the floor the raycast from the mouse hit.
            Vector3 playerToMouse = floorHit.point - transform.position;

            // Ensure the vector is entirely along the floor plane.
            //playerToMouse.y = 0f;

            // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
            Quaternion newRotation = Quaternion.LookRotation (playerToMouse);

            // Set the player's rotation to this new rotation.
            //playerRigidbody.MoveRotation (newRotation);
            transform.rotation = newRotation;
            //Debug.Log("Ground");
          }

        }

        else{
          Vector3 relatePos = target.transform.position - this.transform.position;

          Quaternion newRotation = Quaternion.LookRotation(relatePos);

          transform.rotation = newRotation;
        }
      }
      else{
        
      }
    }
}
