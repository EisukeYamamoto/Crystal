using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    //カーソルに使用するテクスチャ
    [SerializeField]
    private Texture2D cursor = default;
    GameManager gamemanager;

    // Start is called before the first frame update
    void Start()
    {
      gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {
      if(gamemanager.game_stop_flg == false){
        //カーソルを自前のカーソルに変更
        Cursor.SetCursor(cursor, new Vector2(cursor.width / 2, cursor.height / 2),
        CursorMode.Auto);
      }
      else{
        //カーソルを自前のカーソルに変更
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

      }
    }
}
