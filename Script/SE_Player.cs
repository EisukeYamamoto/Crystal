using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SE_Player : MonoBehaviour
{
    public AudioClip sound1;
    AudioSource audioSource;
    private bool isAudioStart = false;

    // Start is called before the first frame update
    void Start()
    {
      //Componentを取得
      audioSource = GetComponent<AudioSource>();
      audioSource.PlayOneShot(sound1);
    }

    // Update is called once per frame
    void Update()
    {
      if (!audioSource.isPlaying && isAudioStart)
      //曲が再生されていない、尚且つ曲の再生が開始されている時
      {
          Destroy(gameObject);//オブジェクトを消す
      }
    }
}
