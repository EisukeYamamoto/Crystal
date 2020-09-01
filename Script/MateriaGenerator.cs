using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MateriaGenerator : MonoBehaviour
{

  [Header("Set Materia Prefab")]
  //敵プレハブ
  public GameObject MateriaPrefab;
  //[Header("Set Interval Min and Max")]
  // //時間間隔の最小値
  // [Range(1f,3f)]
  // public float minTime = 2f;
  // //時間間隔の最大値
  // [Range(5f,10f)]
  // public float maxTime = 5f;
  //////1パターン////////////////////////////////////////////
  [Header("Set X1 Position Min and Max")]
  //X座標の最小値
  [Range(-100f,-30f)]
  public float xMinPosition1 = -100f;
  //X座標の最大値
  [Range(-30f,-10f)]
  public float xMaxPosition1 = -30f;
  [Header("Set Z1 Position Min and Max")]
  //Z座標の最小値
  [Range(-100f,0f)]
  public float zMinPosition1 = -100f;
  //Z座標の最大値
  [Range(0f, 100f)]
  public float zMaxPosition1 = 100f;
  //////2パターン////////////////////////////////////////////
  [Header("Set X2 Position Min and Max")]
  //X座標の最小値
  [Range(10f,30f)]
  public float xMinPosition2 = 10f;
  //X座標の最大値
  [Range(30f,100f)]
  public float xMaxPosition2 = 100f;
  [Header("Set Z2 Position Min and Max")]
  //Z座標の最小値
  [Range(-100f,0f)]
  public float zMinPosition2 = -100f;
  //Z座標の最大値
  [Range(0f, 100f)]
  public float zMaxPosition2 = 100f;
  //////3パターン////////////////////////////////////////////
  [Header("Set X3 Position Min and Max")]
  //X座標の最小値
  [Range(-100f,0f)]
  public float xMinPosition3 = -100f;
  //X座標の最大値
  [Range(0f,100f)]
  public float xMaxPosition3 = 100f;
  [Header("Set Z3 Position Min and Max")]
  //Z座標の最小値
  [Range(-100f,-30f)]
  public float zMinPosition3 = -100f;
  //Z座標の最大値
  [Range(-30f, -10f)]
  public float zMaxPosition3 = -80f;
  //////4パターン////////////////////////////////////////////
  [Header("Set X4 Position Min and Max")]
  //X座標の最小値
  [Range(-100f,0f)]
  public float xMinPosition4 = -100f;
  //X座標の最大値
  [Range(0f,100f)]
  public float xMaxPosition4 = 90f;
  [Header("Set Z4 Position Min and Max")]
  //Z座標の最小値
  [Range(10f,30f)]
  public float zMinPosition4 = 10f;
  //Z座標の最大値
  [Range(30f, 100f)]
  public float zMaxPosition4 = 100f;

  //敵生成時間間隔
  private float interval;
  //経過時間
  //private float time = 0f;

  [SerializeField] private int materianum_limit = default;
  public int materianum;
  private float materia_zero_time = 0f;
  GameObject[] materiaObject;
  GameObject materia;
  GameManager gamemanager;

  [SerializeField] public Text materiaText = default;


    // Start is called before the first frame update
    void Start()
    {
        //materiaをインスタンス化する(生成する)
        for(int i = 0; i < materianum_limit; i++){
          materia = Instantiate(MateriaPrefab);
          //生成した敵の位置をランダムに設定する
          materia.transform.position = GetRandomPosition();
        }
        gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
       if(gamemanager.game_stop_flg == false){
          materianum = materianum_limit - Check("Materia");
          //Debug.Log(materianum);
          materiaText.text = "Materia: " + materianum.ToString() + "/" + materianum_limit.ToString();
          if(materianum == materianum_limit){
              materia_zero_time += Time.deltaTime;
              if(materia_zero_time >= 0.5f){

                 gamemanager.GameClear();
               }
          }
          else{
            materia_zero_time = 0f;
          }
        }



    }

    //ランダムな位置を生成する関数
    private Vector3 GetRandomPosition()
    {
        int r1 = Random.Range(1, 5);
        float y = 3.0f;
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

        if((x < 3 && x > -3) && (z < 53 && z > 47)){
          x = Random.Range(xMinPosition1, xMaxPosition1);
          z = Random.Range(zMinPosition1, zMaxPosition1);
        }

        //Vector3型のPositionを返す
        return new Vector3(x,y,z);
    }

    public int Check(string tagname){
        materiaObject = GameObject.FindGameObjectsWithTag(tagname);
        return materiaObject.Length;
    }
}
