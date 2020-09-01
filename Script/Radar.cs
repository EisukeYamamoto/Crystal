
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
  private float speed = 0.1f;
  public AttackEnemyMove move;
  private bool Crystalflg;

  void Start(){
      Crystalflg = false;

  }
  // 「OnTriggerStay」はトリガーが他のコライダーに触れている間中実行されるメソッド（ポイント）
  private void OnTriggerStay(Collider other)
  {

      //Debug.Log(other);
      if (other.CompareTag("crystal"))
      {
          move.AttackEnemyFlag = true;
          Crystalflg = true;
          //Debug.Log("クリスタルを見つけました");
      }

      //もしも他のオブジェクトに「Player」というTag（タグ）が付いていたならば（条件）
      else if (other.CompareTag("Player") && Crystalflg == false)
      {
          GameObject target = GameObject.FindWithTag("Player");
          //Debug.Log(move.AttackEnemyFlag);
          move.AttackEnemyFlag = true;
          // Debug.Log("敵に見つかりました");
          // 「root」を使うと「親（最上位の親）」の情報を取得することができる（ポイント）
          // LookAt()メソッドは指定した方向にオブジェクトの向きを回転させることができる（ポイント）
          //transform.root.LookAt(target);
          // 以下，https://www.sejuku.net/blog/69635 参照
          Vector3 relatePos = target.transform.position - this.transform.root.position;

          Quaternion rotation = Quaternion.LookRotation(relatePos);

          transform.root.rotation = Quaternion.Slerp(this.transform.root.rotation,
                                                     rotation, speed);


      }

  }
}
