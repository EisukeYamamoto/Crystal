using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitController : MonoBehaviour
{
    //当たり判定メソッド
    public void RelayOnTriggerEnter(Collider collision)
    {
        //衝突したオブジェクトがBullet(大砲の弾)だったとき
        if(collision.gameObject.CompareTag("Bullet"))
        {
            //Debug.Log("敵と弾が衝突しました！！！");
            //敵(スクリプトがアタッチされているオブジェクト自身)を削除
            Destroy(gameObject);
            //弾(引数オブジェクト)を削除
            Destroy(collision.gameObject);
        }
    }



}
