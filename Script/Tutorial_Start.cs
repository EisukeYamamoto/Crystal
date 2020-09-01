using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_Start : MonoBehaviour
{

  [SerializeField]
  float waitTime = 2.0f;
  [SerializeField] GameObject Tutorial_1 = default;
  [SerializeField] GameObject Tutorial_2 = default;
  [SerializeField] GameObject Tutorial_3 = default;
  [SerializeField] GameObject Tutorial_4 = default;

  GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
      gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
      Tutorial_1.SetActive(false);
      Tutorial_2.SetActive(false);
      Tutorial_3.SetActive(false);
      Tutorial_4.SetActive(false);

      StartCoroutine(Tutorial());

    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Tutorial()
    {
        yield return new WaitForEndOfFrame();

        //プレイヤーを停止させる
        gameManager.game_stop_flg = true;
        gameManager.pause_flg = false;

        yield return new WaitForSeconds(waitTime);

        Tutorial_1.SetActive(true);

        yield return new WaitForSeconds(waitTime);
        yield return new WaitUntil(()=>Input.GetKeyUp(KeyCode.Space));

        Tutorial_1.SetActive(false);
        Tutorial_2.SetActive(true);

        yield return new WaitForSeconds(waitTime);
        yield return new WaitUntil(()=>Input.GetKeyUp(KeyCode.Space));

        Tutorial_2.SetActive(false);
        Tutorial_3.SetActive(true);

        yield return new WaitForSeconds(waitTime);
        yield return new WaitUntil(()=>Input.GetKeyUp(KeyCode.Space));

        Tutorial_3.SetActive(false);
        Tutorial_4.SetActive(true);

        yield return new WaitForSeconds(waitTime);
        yield return new WaitUntil(()=>Input.GetKeyUp(KeyCode.Space));

        Tutorial_4.SetActive(false);

        //プレイヤーを移動可能にさせる
        gameManager.game_stop_flg = false;
        gameManager.pause_flg = true;


        yield return new WaitForSeconds(waitTime);

        //readyGoText.enabled = false;

    }
}
