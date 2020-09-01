using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [Header("Set Enemy Prefab")]
    //敵プレハブ
    public GameObject enemyPrefab;
    [Header("Set Interval Min and Max")]
    //時間間隔の最小値
    [Range(1f,3f)]
    public float minTime = 2f;
    //時間間隔の最大値
    [Range(5f,10f)]
    public float maxTime = 5f;
    [Header("Set Y Position Min and Max")]
    //Y座標の最小値
    [Range(5f,25f)]
    public float yMinPosition = 5f;
    //Y座標の最大値
    [Range(25f,45f)]
    public float yMaxPosition = 40f;
    //////1パターン////////////////////////////////////////////
    [Header("Set X1 Position Min and Max")]
    //X座標の最小値
    [Range(-150f,-130f)]
    public float xMinPosition1 = -100f;
    //X座標の最大値
    [Range(-130f,-110f)]
    public float xMaxPosition1 = -80f;
    [Header("Set Z1 Position Min and Max")]
    //Z座標の最小値
    [Range(-130f,0f)]
    public float zMinPosition1 = -100f;
    //Z座標の最大値
    [Range(0f, 130f)]
    public float zMaxPosition1 = 100f;
    //////2パターン////////////////////////////////////////////
    [Header("Set X2 Position Min and Max")]
    //X座標の最小値
    [Range(110f,130f)]
    public float xMinPosition2 = 80f;
    //X座標の最大値
    [Range(130f,150f)]
    public float xMaxPosition2 = 100f;
    [Header("Set Z2 Position Min and Max")]
    //Z座標の最小値
    [Range(-130f,0f)]
    public float zMinPosition2 = -100f;
    //Z座標の最大値
    [Range(0f, 130f)]
    public float zMaxPosition2 = 100f;
    //////3パターン////////////////////////////////////////////
    [Header("Set X3 Position Min and Max")]
    //X座標の最小値
    [Range(-130f,0f)]
    public float xMinPosition3 = -100f;
    //X座標の最大値
    [Range(0f,130f)]
    public float xMaxPosition3 = 100f;
    [Header("Set Z3 Position Min and Max")]
    //Z座標の最小値
    [Range(-150f,-130f)]
    public float zMinPosition3 = -100f;
    //Z座標の最大値
    [Range(-130f, -110f)]
    public float zMaxPosition3 = -80f;
    //////4パターン////////////////////////////////////////////
    [Header("Set X4 Position Min and Max")]
    //X座標の最小値
    [Range(-130f,0f)]
    public float xMinPosition4 = -100f;
    //X座標の最大値
    [Range(0f,130f)]
    public float xMaxPosition4 = 90f;
    [Header("Set Z4 Position Min and Max")]
    //Z座標の最小値
    [Range(110f,130f)]
    public float zMinPosition4 = 80f;
    //Z座標の最大値
    [Range(130f, 150f)]
    public float zMaxPosition4 = 100f;
    //敵生成時間間隔
    private float interval;
    //経過時間
    private float time = 0f;

    [SerializeField] private int enemynum_limit = default;
    private int enemynum;
    GameObject[] enemyObject;

    GameManager gamemanager;

    // Start is called before the first frame update
    void Start()
    {
      //時間間隔を決定する
      interval = GetRandomTime();

      gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
      if(gamemanager.game_stop_flg == false){
        enemyObject = GameObject.FindGameObjectsWithTag("Enemy");
        enemynum = enemyObject.Length;
        //Debug.Log(enemynum);

        if(enemynum < enemynum_limit){
          //時間計測
          time += Time.deltaTime;
        }

        //経過時間が生成時間になったとき(生成時間より大きくなったとき)
        if(time > interval && enemynum <= enemynum_limit)
        {
          //enemyをインスタンス化する(生成する)
          GameObject enemy = Instantiate(enemyPrefab);
          //生成した敵の位置をランダムに設定する
          enemy.transform.position = GetRandomPosition();
          //Debug.Log(enemy.transform.position);
          //経過時間を初期化して再度時間計測を始める
          time = 0f;
          //次に発生する時間間隔を決定する
          interval = GetRandomTime();
        }
      }
    }
    //ランダムな時間を生成する関数
    private float GetRandomTime()
    {
        return Random.Range(minTime, maxTime);
    }

    //ランダムな位置を生成する関数
    private Vector3 GetRandomPosition()
    {
        int r1 = Random.Range(1, 5);
        float y = Random.Range(yMinPosition, yMaxPosition);
        float x, z;
        switch(r1)
        {
           //それぞれの座標をランダムに生成する
           case 1:
               x = Random.Range(xMinPosition1, xMaxPosition1);
               z = Random.Range(zMinPosition1, zMaxPosition1);
               break;
           case 2:
               x = Random.Range(xMinPosition2, xMaxPosition2);
               z = Random.Range(zMinPosition2, zMaxPosition2);
               break;
           case 3:
               x = Random.Range(xMinPosition3, xMaxPosition3);
               z = Random.Range(zMinPosition3, zMaxPosition3);
               break;
           case 4:
               x = Random.Range(xMinPosition4, xMaxPosition4);
               z = Random.Range(zMinPosition4, zMaxPosition4);
               break;
            default:
               x = Random.Range(xMinPosition1, xMaxPosition1);
               z = Random.Range(zMinPosition1, zMaxPosition1);
               //Debug.Log(r1);
               break;
        }

        //Vector3型のPositionを返す
        return new Vector3(x,y,z);
    }
}
