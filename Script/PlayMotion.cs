//プレイヤーの動き

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayMotion : MonoBehaviour
{

  [SerializeField] private Vector3 velocity;
  [SerializeField] public int player_life = 5;
  [SerializeField] private float moveSpeed = 10.0f;        // 移動速度
  [SerializeField] private float applySpeed = 0.2f;       // 振り向きの適用速度

  [SerializeField] private PlayerFollowCamera refCamera = default;
  [SerializeField] private LifeGauge lifeGauge = default;
  private Rigidbody _rigidBody;
  public Vector3 velocity_copy;

  private Vector3 left = new Vector3(1, 0, -1);
  private Vector3 right = new Vector3(-1, 0, -1);

  ///    ジャンプ入力フラグ
  ///    ジャンプ入力が一度でもあったらON、着地したらOFF
  private bool _jumpInput = false;

  ///    ジャンプ処理中フラグ
  ///    ジャンプ処理が開始されたらON、着地したらOFF
  private bool _isJumping = false;


  ///    接地してから何フレーム経過したか
  ///    接地してない間は常にゼロとする
  private int _isGround = 0;

  ///    接地してない間、何フレーム経過したか
  ///    接地している間は常にゼロとする
  private int _notGround = 0;

  ///    このフレーム数分接地していたらor接地していなかったら
  ///    状態が変わったと認識する（ジャンプ開始したor着地した）
  ///    接地してからキャラの状態が安定するまでに数フレーム用するため、
  ///    キャラが安定する前に再ジャンプ入力を受け付けてしまうとバグる（ジャンプ出来なくなる）
  ///    筆者PCでは 3 で安定するが、安全をとって今回は 5 とした
  private const int _isGroundStateChange = 5;

  ///    プレイヤーと地面の間の距離
  ///    IsGround()が呼ばれるたびに更新される
  [SerializeField] private float _groundDistance = 0f;
  [SerializeField] private float _floorDistance = 0f;

  private float _frontDistance = 0f;
  private float _leftDistance = 0f;
  private float _rightDistance = 0f;

  ///    _groundDistanceがこの値以下の場合接地していると判定する
  private const float _groundDistanceLimit = 0.8f;

  ///    _floorDistanceがこの値以下の場合接地していると判定する
  private const float _floorDistanceLimit = 2.5f;

  ///    _floorDistanceがこの値以下の場合接地していると判定する
  private const float _floorDistanceLimit_alarm = 3.5f;

  ///    判定元の原点が地面に極端に近いとrayがヒットしない場合があるので、
  ///    オフセットを設けて確実にヒットするようにする
  private Vector3 _raycastOffset  = new Vector3(0f, 0.05f, 0f);

  ///    判定元の原点が地面に極端に近いとrayがヒットしない場合があるので、
  ///    オフセットを設けて確実にヒットするようにする
  private Vector3 _raycastOffset2  = new Vector3(0f, 0f, 0f);

  private Vector3 _raycastOffset_left  = new Vector3(0f, 0f, 0f);

  private Vector3 _raycastOffset_right  = new Vector3(0f, 0f, 0f);

  ///    プレイヤーキャラから下向きに地面判定のrayを飛ばす時の上限距離
  ///    ゲーム中でプレイヤーキャラと地面が最も離れると考えられる場面の距離に、
  ///    マージンを多少付けた値にしておくのが良
  ///    Mathf.Infinityを指定すれば無制限も可能だが重くなる可能性があるかも？
  private const float _raycastSearchDistance = 100f;

  /// 前向きの距離
  private const float _raycastSearchDistance2 = 200f;

  private bool floorHitflg = false;
  public bool Get_Materia_flg = false;
  public bool Crystal_near_flg = false;

  GameManager gamemanager;

    // Start is called before the first frame update
    void Start()
    {
      //transform.position = new Vector3(0.0f, 1.0f, 0.0f);
      _rigidBody = GetComponent<Rigidbody>();
      velocity = Vector3.zero;
      velocity_copy = velocity;
      gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();

      lifeGauge.SetLifeGauge(player_life);

    }

    // Update is called once per frame
    void Update()
    {
      if(gamemanager.game_stop_flg == false){
        //Debug.Log(player_life);
        velocity = Vector3.zero;
        if(Input.GetKey(KeyCode.W)){
          velocity.z += 1;
        }
        if(Input.GetKey(KeyCode.S)){
          velocity.z -= 1;
        }
        if(Input.GetKey(KeyCode.D)){
          velocity.x += 1;
        }
        if(Input.GetKey(KeyCode.A)){
          velocity.x -= 1;
        }

        CheckGroundDistance(() => {
          _jumpInput = false;
          _isJumping = false;
          });

          Debug.DrawRay(
          transform.position + _raycastOffset2,
          transform.TransformDirection(Vector3.back)* 100, Color.red, 0.5f, false);

          Debug.DrawRay(
          transform.position + _raycastOffset_left,
          transform.TransformDirection(left.normalized)* 100, Color.yellow, 0.5f, false);

          Debug.DrawRay(
          transform.position + _raycastOffset_right,
          transform.TransformDirection(right.normalized)* 100, Color.blue, 0.5f, false);




          // 既にジャンプ入力が行われていたら、ジャンプ入力チェックを飛ばす
          if (_jumpInput || JumpInput()) _jumpInput = true;

          //Debug.Log(_notGround);

          // 速度ベクトルの長さを1秒でmoveSpeedだけ進むように調整します
          velocity = velocity.normalized * moveSpeed * Time.deltaTime;
          //Debug.Log(velocity.magnitude);
          velocity_copy = velocity;

          if(velocity.magnitude > 0 && refCamera.rotateflg == false){
            // プレイヤーの回転(transform.rotation)の更新
            // 無回転状態のプレイヤーのZ+方向(後頭部)を、移動の反対方向(-velocity)に回す回転とします
            //transform.rotation = Quaternion.LookRotation(-velocity);
            transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(refCamera.hRotation * -velocity),
            applySpeed);
            CheckFloorDistance();
            if(floorHitflg == true){
              velocity = Vector3.zero;
            }

            transform.position += refCamera.hRotation * velocity;
            //Debug.Log(velocity);
          }

          //Debug.Log(Crystal_near_flg);
      }
      else{

      }


    }

    private void FixedUpdate()
    {
        if (_jumpInput) {
            if (!_isJumping) {
                _isJumping = true;
                DoJump();
            }
        }
    }

    ///    ジャンプ入力チェック
    private bool JumpInput()
    {
        // ジャンプ最速入力のテスト用にGetButton
        //if (Input.GetButton("Jump")) return true;    // ジャンプキー押しっぱなしで連続ジャンプ
        //if (Input.GetButtonDown("Jump")) return true;    // ジャンプキーが押された時だけジャンプにする時はこっち
        if (Input.GetKeyDown(KeyCode.Space)) return true;
        return false;
    }

    ///    ジャンプの強さ
    [SerializeField] private float jumpPower = 100f;
    //private const float jumpPower = 5f;

    ///    ジャンプのための上方向への加圧
    private void DoJump()
    {
        _rigidBody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
    }



    ///    接地判定
    private void CheckGroundDistance(UnityAction landingAction = null, UnityAction takeOffAction = null)
    {
        RaycastHit hit;
        var layerMask = LayerMask.GetMask("Ground");

        // プレイヤーの位置から下向きにRaycast
        // レイヤーマスクでGroundを設定しているので、
        // 地面のGameObjectにGroundのレイヤーを設定しておけば、
        // Groundのレイヤーを持つGameObjectで一番近いものが一つだけヒットする
        var isGroundHit = Physics.Raycast(
                transform.position + _raycastOffset,
                transform.TransformDirection(Vector3.down),
                out hit,
                _raycastSearchDistance,
                layerMask
            );

        if (isGroundHit) {
            _groundDistance = hit.distance;
        } else {
            // ヒットしなかった場合はキャラの下方に地面が存在しないものとして扱う
            _groundDistance = float.MaxValue;
        }
        //Debug.Log(_groundDistance);

        // 地面とキャラの距離は環境によって様々で
        // 完全にゼロにはならない時もあるため、
        // ジャンプしていない時の値に多少のマージンをのせた
        // 一定値以下を接地と判定する
        // 通常あり得ないと思われるが、オーバーフローされると再度アクションが実行されてしまうので、越えたところで止める
        if (_groundDistance < _groundDistanceLimit) {
            if (_isGround <= _isGroundStateChange) {
                _isGround += 1;
                _notGround = 0;
            }
        } else {
            if (_notGround <= _isGroundStateChange) {
                _isGround = 0;
                _notGround += 1;
            }
        }

        // 接地後またはジャンプ後、特定フレーム分状態の変化が無ければ、
        // 状態が安定したものとして接地処理またはジャンプ処理を行う
        if (_isGroundStateChange == _isGround && _notGround == 0) {
            if (landingAction != null){
              landingAction();
              //Debug.Log("landing");
            }
        } else
        if (_isGroundStateChange == _notGround && _isGround == 0) {
            if (takeOffAction != null){
              takeOffAction();
              //Debug.Log("takeOFF");
            }
        }
    }

    ///    接地判定_前向き
    private void CheckFloorDistance(UnityAction landingAction = null, UnityAction takeOffAction = null)
    {
        RaycastHit hit_front_floor;
        RaycastHit hit_left_floor;
        RaycastHit hit_right_floor;

        var layerMask_field = LayerMask.GetMask("Field");

        // プレイヤーの位置から下向きにRaycast
        // レイヤーマスクでGroundを設定しているので、
        // 地面のGameObjectにGroundのレイヤーを設定しておけば、
        // Groundのレイヤーを持つGameObjectで一番近いものが一つだけヒットする
        var isfrontHit = Physics.Raycast(
                transform.position + _raycastOffset2,
                transform.TransformDirection(Vector3.back),
                out hit_front_floor,
                _raycastSearchDistance2,
                layerMask_field
            );

        var isleftHit = Physics.Raycast(
                transform.position + _raycastOffset_left,
                transform.TransformDirection(left.normalized),
                out hit_left_floor,
                _raycastSearchDistance2,
                layerMask_field
            );
        var isrightHit = Physics.Raycast(
                transform.position + _raycastOffset_right,
                transform.TransformDirection(right.normalized),
                out hit_right_floor,
                _raycastSearchDistance2,
                layerMask_field
            );



        if (isfrontHit) {
            _frontDistance = hit_front_floor.distance;
        } else {
            // ヒットしなかった場合はキャラの下方に地面が存在しないものとして扱う
            _frontDistance = float.MaxValue;
        }

        if (isleftHit) {
            _leftDistance = hit_left_floor.distance;
        } else {
            // ヒットしなかった場合はキャラの下方に地面が存在しないものとして扱う
            _leftDistance = float.MaxValue;
        }

        if (isrightHit) {
            _rightDistance = hit_right_floor.distance;
        } else {
            // ヒットしなかった場合はキャラの下方に地面が存在しないものとして扱う
            _rightDistance = float.MaxValue;
        }

        _floorDistance = Min(_frontDistance, _leftDistance, _rightDistance);
        //Debug.Log(_groundDistance);

        // 地面とキャラの距離は環境によって様々で
        // 完全にゼロにはならない時もあるため、
        // ジャンプしていない時の値に多少のマージンをのせた
        // 一定値以下を接地と判定する
        // 通常あり得ないと思われるが、オーバーフローされると再度アクションが実行されてしまうので、越えたところで止める
        if (_floorDistance < _floorDistanceLimit_alarm){
            velocity /= 2;
            if (_floorDistance < _floorDistanceLimit) {
              velocity = Vector3.zero;
            }
        }
    }

    //当たり判定メソッド
    public void RelayOnTriggerEnter(Collider collision)
    {
        //衝突したオブジェクトがBullet(大砲の弾)だったとき
        if(collision.gameObject.CompareTag("Bullet"))
        {
            //Debug.Log("敵と弾が衝突しました！！！");
            //敵(スクリプトがアタッチされているオブジェクト自身)を削除
            player_life -= 1;
            if(player_life < 0){
              player_life = 0;
            }

            if(player_life >= 0){
              lifeGauge.SetLifeGauge(player_life);
            }

            if(player_life == 0){
              gamemanager.GameOver();
            }
            //弾(引数オブジェクト)を削除
            //Destroy(collision.gameObject);
        }

        // if(collision.gameObject.CompareTag("Materia"))
        // {
        //     if(Get_Materia_flg == false){
        //         //Debug.Log("Materia");
        //         //Get_Materia_flg = true;
        //     }
        //     else{
        //
        //     }
        // }
    }

    public float Min(params float[] nums)
    {
        // 引数が渡されない場合
        if(nums.Length == 0) return 999f;

        float min = nums[0];
        for(int i = 1; i < nums.Length; i++)
        {
            min = min < nums[i] ? min : nums[i];
        }
        return min;
    }
}
