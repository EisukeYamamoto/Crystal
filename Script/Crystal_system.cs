using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_system : MonoBehaviour
{
  [SerializeField] public int crystal_life = 5;
  [SerializeField] public float crystal_life_now = 5;

  GameObject lifesystem;
  GameManager gamemanager;
    // Start is called before the first frame update
    void Start()
    {
        lifesystem = GameObject.Find("life");
        gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {
       if(gamemanager.game_stop_flg == false){
         lifesystem.GetComponent<Crystal_life_gauge>().HPDown(crystal_life_now, crystal_life);
         if(crystal_life_now == 0){
             gamemanager.GameOver();
         }
       }
       else{

       }
      //Debug.Log(crystal_life_now);
    }

    //当たり判定メソッド
    public void RelayOnTriggerEnter(Collider collision)
    {
        //衝突したオブジェクトがBullet(大砲の弾)だったとき
        if(collision.gameObject.CompareTag("Bullet"))
        {
            //Debug.Log("クリスタル衝突");
            //Debug.Log("敵と弾が衝突しました！！！");
            //敵(スクリプトがアタッチされているオブジェクト自身)を削除
            crystal_life_now -= 1;
            if(crystal_life_now < 0){
              crystal_life_now = 0;
            }
            //弾(引数オブジェクト)を削除
            Destroy(collision.gameObject);
        }
    }
}
