//https://unity.moon-bear.com/3d%e3%82%a2%e3%82%af%e3%82%b7%e3%83%a7%e3%83%b3%e3%82%b2%e3%83%bc%e3%83%a0%e3%80%8c%e3%83%a6%e3%83%8b%e3%83%86%e3%82%a3%e3%81%a1%e3%82%83%e3%82%93%e3%83%91%e3%83%ab%e3%82%af%e3%83%bc%e3%83%ab%e3%80%8d/%e3%82%b7%e3%83%bc%e3%83%b3%e3%81%ae%e5%88%87%e3%82%8a%e6%9b%bf%e3%81%88%e5%87%a6%e7%90%86%e3%82%92%e4%bd%9c%e3%82%8b/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
  [System.NonSerialized]
  public int currentStageNum = 0; //現在のステージ番号（0始まり）

  // [SerializeField]
  // string[] stageName; //ステージ名
  [SerializeField]
  GameObject fadeCanvasPrefab = default;
  [SerializeField]
  GameObject gameOverCanvasPrefab = default;
  [SerializeField]
  GameObject gameClearCanvasPrefab = default;
  [SerializeField]
  GameObject PauseCanvasPrefab = default;
  [SerializeField]
  float fadeWaitTime = 1.0f; //フェード時の待ち時間
  [SerializeField]
  GameObject Audio_Positive = default;
  [SerializeField]
  GameObject Audio_Negative = default;

  GameObject fadeCanvasClone;
  FadeManager fadeCanvas;
  GameObject gameOverCanvasClone;
  GameObject gameClearCanvasClone;
  GameObject PauseCanvasClone;
  PlayMotion playmotion;
  CannnonForward cannonforward;
  CannonBullet cannonbullet;
  PlayerFollowCamera Maincamera;
  Target target_image;
  AttackEnemyMove enemy;
  Rigidbody playerRigidbody;

  Button[] buttons;

  public bool game_stop_flg;
  public bool pause_flg;


  //最初の処理
  void Start ()
  {
      //シーンを切り替えてもこのゲームオブジェクトを削除しないようにする
      DontDestroyOnLoad(gameObject);
      game_stop_flg = false;
      pause_flg = false;

      //デリゲートの登録
      SceneManager.sceneLoaded += OnSceneLoaded;

      //ユーザーコントロールとカメラコントロールを取得
      // playmotion = GameObject.Find("Player").GetComponent<PlayMotion>();
      // cannonforward = GameObject.Find("CannonRotation").GetComponent<CannnonForward>();
      // cannonbullet = GameObject.Find("CannonBulletFire").GetComponent<CannonBullet>();
      // camera = GameObject.FindWithTag("MainCamera").GetComponent<PlayerFollowCamera>();
      // enemy = GameObject.Find("CannonBulletFire_Enemy").GetComponent<AttackEnemyMove>();
      // playerRigidbody = GameObject.Find("Player").GetComponent<Rigidbody>();
  }

  //シーンのロード時に実行（最初は実行されない）
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //改めて取得
        LoadComponents();
    }

    //コンポーネントの取得
    void LoadComponents()
    {
        //タイトル画面じゃないなら取得
        if(SceneManager.GetActiveScene().name != "StartScene")
        {
          playmotion = GameObject.Find("Player").GetComponent<PlayMotion>();
          cannonforward = GameObject.Find("CannonRotation").GetComponent<CannnonForward>();
          cannonbullet = GameObject.Find("CannonBulletFire").GetComponent<CannonBullet>();
          Maincamera = GameObject.FindWithTag("MainCamera").GetComponent<PlayerFollowCamera>();
          target_image = GameObject.FindWithTag("MainCamera").GetComponent<Target>();
          //enemy = GameObject.Find("CannonBulletFire_Enemy").GetComponent<AttackEnemyMove>();
          playerRigidbody = GameObject.Find("Player").GetComponent<Rigidbody>();
          game_stop_flg = false;
          pause_flg = false;
          target_image.enabled = true;
        }
    }

  //毎フレームの処理
  void Update ()
  {
    if(Input.GetKeyDown(KeyCode.P)){
      if(pause_flg == true){
        Pause();
      }
    }
    Debug.Log(currentStageNum);

  }

  //次のステージに進む処理
  public void NextStage()
  {
      currentStageNum = 1;
      Instantiate(Audio_Positive, transform.position, transform.rotation);

      //コルーチンを実行
      StartCoroutine(WaitForLoadScene(currentStageNum));
  }

  //次のステージに進む処理
  public void NextTutorialStage()
  {
      currentStageNum = 2;
      Instantiate(Audio_Positive, transform.position, transform.rotation);

      //コルーチンを実行
      StartCoroutine(WaitForLoadScene(currentStageNum));
  }

  //任意のステージに移動する処理
  public void MoveToStage(int stageNum)
  {
      //コルーチンを実行
      StartCoroutine(WaitForLoadScene(stageNum));
      //Debug.Log(stageNum);
  }

  //シーンの読み込みと待機を行うコルーチン
  IEnumerator WaitForLoadScene(int stageNum)
  {
      //character.enabled = false;
      //playerRigidbody.isKinematic = true;


      //フェードオブジェクトを生成
      fadeCanvasClone = Instantiate(fadeCanvasPrefab);

      //コンポーネントを取得
      fadeCanvas = fadeCanvasClone.GetComponent<FadeManager>();

      //フェードインさせる
      fadeCanvas.fadeIn = true;

      yield return new WaitForSeconds(fadeWaitTime);

      //シーンを非同期で読込し、読み込まれるまで待機する
      yield return SceneManager.LoadSceneAsync(stageNum);

      //フェードアウトさせる
      //fadeCanvas.fadeOut = true;
      fadeCanvas.fadeReset = true;
  }

  //ゲームオーバー処理
  public void GameOver()
  {
      //キャラやカメラの移動を停止させる
      // character.enabled = false;
      // playerRigidbody.isKinematic = true;
      // freeLookCam.enabled = false;
      game_stop_flg = true;
      pause_flg = false;
      //Debug.Log("over");

      //ゲームオーバー画面表示
      gameOverCanvasClone = Instantiate(gameOverCanvasPrefab);

      //ボタンを取得
      buttons = gameOverCanvasClone.GetComponentsInChildren<Button>();

      //ボタンにイベント設定
      buttons[0].onClick.AddListener(Retry);
      buttons[1].onClick.AddListener(Return);

  }

  //ゲームクリア処理
  public void GameClear()
  {
      //キャラやカメラの移動を停止させる
      // character.enabled = false;
      // playerRigidbody.isKinematic = true;
      // freeLookCam.enabled = false;
      game_stop_flg = true;
      pause_flg = false;
      //Debug.Log("clear");

      //ゲームオーバー画面表示
      gameClearCanvasClone = Instantiate(gameClearCanvasPrefab);

      //ボタンを取得
      buttons = gameClearCanvasClone.GetComponentsInChildren<Button>();

      //ボタンにイベント設定
      buttons[0].onClick.AddListener(Retry_Clear);
      buttons[1].onClick.AddListener(Return_Clear);

  }

  //ゲームオーバー処理
  public void Pause()
  {
      //キャラやカメラの移動を停止させる
      // character.enabled = false;
      // playerRigidbody.isKinematic = true;
      // freeLookCam.enabled = false;
      game_stop_flg = true;
      pause_flg = false;
      //Debug.Log("over");

      //ゲームオーバー画面表示
      PauseCanvasClone = Instantiate(PauseCanvasPrefab);

      //ボタンを取得
      buttons = PauseCanvasClone.GetComponentsInChildren<Button>();

      //ボタンにイベント設定
      buttons[0].onClick.AddListener(Retry_Pause);
      buttons[1].onClick.AddListener(Return_Pause);

  }

  //リトライ
    public void Retry()
    {
        Destroy(gameOverCanvasClone);

        Instantiate(Audio_Positive, transform.position, transform.rotation);

        if(SceneManager.GetActiveScene().name == "MainScene"){
            MoveToStage(1);
        }
        else{
            MoveToStage(2);
        }
    }

    //最初のシーンに戻る
    public void Return()
    {
        Destroy(gameOverCanvasClone);

        //Debug.Log("return");
        Instantiate(Audio_Negative, transform.position, transform.rotation);

        target_image.enabled = false;

        MoveToStage(0);
    }

    //リトライ
      public void Retry_Clear()
      {
          Destroy(gameClearCanvasClone);

          Instantiate(Audio_Positive, transform.position, transform.rotation);
          if(SceneManager.GetActiveScene().name == "MainScene"){
              MoveToStage(1);
          }
          else{
              MoveToStage(2);
          }
      }

      //最初のシーンに戻る
      public void Return_Clear()
      {
          Destroy(gameClearCanvasClone);

          //Debug.Log("return");
          Instantiate(Audio_Negative, transform.position, transform.rotation);

          target_image.enabled = false;

          MoveToStage(0);
      }

      //リトライ
        public void Retry_Pause()
        {
            Destroy(PauseCanvasClone);

            Instantiate(Audio_Positive, transform.position, transform.rotation);

            //MoveToStage(currentStageNum);
            game_stop_flg = false;
            pause_flg = true;
        }

        //最初のシーンに戻る
        public void Return_Pause()
        {
            Destroy(PauseCanvasClone);

            //Debug.Log("return");
            Instantiate(Audio_Negative, transform.position, transform.rotation);

            target_image.enabled = false;

            MoveToStage(0);
        }

    //ゲーム終了
    public void ExitGame()
    {
        Instantiate(Audio_Negative, transform.position, transform.rotation);
        Application.Quit();
    }
}
