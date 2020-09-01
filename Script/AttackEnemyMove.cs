using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnemyMove : MonoBehaviour
{
  //bulletプレハブ
  public GameObject bullet;
  //弾が生成されるポジションを保有するゲームオブジェクト
  public GameObject bulletPos;

  private GameObject main_target;

  private Vector3 velocity_enemy;

  [SerializeField] private float moveSpeed_enemy = 5.0f;        // 移動速度

  [SerializeField] private GameObject Audio_Object_bullet = default;
  //弾丸のスピード
  public float speed = 1500f;

  private int shotInterval;

  public int shotlimit = 60;

  private float myrotatespeed = 0.1f;

  public bool AttackEnemyFlag;

  GameManager gamemanager;
  // Start is called before the first frame update
  void Start()
  {
      AttackEnemyFlag = false;
      main_target = GameObject.FindWithTag("crystal_target");
      velocity_enemy = Vector3.zero;

      gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();

  }

  // Update is called once per frame
  void Update()
  {
    if(gamemanager.game_stop_flg == false){
      shotInterval += 1;
      if(AttackEnemyFlag == true)
      {
        if (shotInterval % shotlimit == 0)
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
      else{
        Vector3 relatePos = main_target.transform.position - this.transform.root.position;

        Quaternion rotation = Quaternion.LookRotation(relatePos);

        transform.root.rotation = Quaternion.Slerp(this.transform.root.rotation,
        rotation, myrotatespeed);

        velocity_enemy.z += 1;

        velocity_enemy = velocity_enemy.normalized * moveSpeed_enemy * Time.deltaTime;

        transform.root.position += transform.root.rotation * velocity_enemy;

      }
      AttackEnemyFlag = false;
    }
    else{
      
    }
  }
}
