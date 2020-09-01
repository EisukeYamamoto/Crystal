using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//ボタンを使用するためUIとSceneManagerを使用ためSceneManagementを追加
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
  public AudioClip sound1;
  AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
      //Componentを取得
      audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // ボタンをクリックするとBattleSceneに移動します
    public void ButtonClicked () {
        audioSource.PlayOneShot(sound1);
        //SceneManager.LoadSceneAsync("MainScene");
        //FadeManager.FadeOut(1);
	}
}
