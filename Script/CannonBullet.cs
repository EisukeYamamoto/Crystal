using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBullet : MonoBehaviour
{
    //bulletプレハブ
    public GameObject bullet;
    public GameObject materia;
    //弾が生成されるポジションを保有するゲームオブジェクト
    public GameObject bulletPos;
    //弾丸のスピード
    public float speed = 1500f;

    private GameObject createMateria;
    [SerializeField] private GameObject effectObject = default;
    [SerializeField] private GameObject Audio_Object_materia = default;
    [SerializeField] private GameObject Audio_Object_bullet = default;
    [SerializeField] private float deleteTime = 1.0f;
    [SerializeField] private float offset = default;

    private GameObject player;
    private PlayMotion playmotion;
    GameManager gamemanager;

    private bool createMateria_flg;

    // Start is called before the first frame update
    void Start()
    {
      player = gameObject.transform.root.gameObject;
      playmotion = player.GetComponent<PlayMotion>();
      createMateria_flg = true;
      gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
      if(gamemanager.game_stop_flg == false){
        if(playmotion.Get_Materia_flg == false)
        {
          //スペースが押されたとき
          createMateria_flg = true;
          if (Input.GetMouseButtonDown(0))
          {
            //ballをインスタンス化して発射
            GameObject createdBullet = Instantiate(bullet) as GameObject;
            createdBullet.transform.position = bulletPos.transform.position;
            Instantiate(Audio_Object_bullet, createdBullet.transform.position, createdBullet.transform.rotation);

            //発射ベクトル
            Vector3 force;
            //発射の向きと速度を決定
            force = bulletPos.transform.forward * speed;
            // Rigidbodyに力を加えて発射
            createdBullet.GetComponent<Rigidbody>().AddForce(force);
          }
        }

        else
        {
          if(createMateria_flg == true){
            createMateria = Instantiate(materia) as GameObject;
            createMateria_flg = false;
          }

          createMateria.transform.position = bulletPos.transform.position;

          if(playmotion.Crystal_near_flg == true)
          {
            if(Input.GetMouseButtonDown(0))
            {
              Instantiate(Audio_Object_materia, createMateria.transform.position, createMateria.transform.rotation);
              Destroy(createMateria);
              var instantiateEffect = GameObject.Instantiate(effectObject, createMateria.transform.position + new Vector3(0f,offset,0f), Quaternion.identity) as GameObject;
              Destroy(instantiateEffect, deleteTime);
              playmotion.Get_Materia_flg = false;
            }
          }
        }
      }
      else{
        
      }
    }
}
