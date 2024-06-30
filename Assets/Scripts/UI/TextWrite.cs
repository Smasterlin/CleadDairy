using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TextWrite : MonoBehaviour
{
    private float charPerSecond;
    private string word;
    private bool startWrite;
    private float timer;
    [SerializeField]private Text text;
    private int currentpos;
    [SerializeField] private LoadGame loadGame;
    [SerializeField] private AudioClip writeClip;
    // Start is called before the first frame update
    void Start()
    {
        charPerSecond = 0.1f;
        startWrite = false;
        timer = 0;

        int storyIndex=GlobalValue.SelectLevel;
        if (storyIndex==5)
        {
            if (GameManager.instance.isAnthonyDead==true)
            {
                storyIndex = 5;
            }
            else
            {
                storyIndex = 7;
            }
            if (GameManager.instance.showEnd==true)
            {
                storyIndex++;
            }
        }

        //文本内容
        word = GameManager.instance.stories[storyIndex];
        text.text = "";
        StartWriteEffect();
    }
    private void StartWriteEffect()
    {
        startWrite = true;
    }
    private void StartWrite()
    {
        if (startWrite==true)
        {
            //Debug.Log("开始书写日记了11111");
            if (AudioSourceManager.instance.audioSource.isPlaying==false)
            {
                AudioSourceManager.instance.PlayMusic(writeClip);
            }
            timer += Time.deltaTime;
            if (timer>=charPerSecond)
            {
                //Debug.Log("开始书写日记了");
                timer = 0;
                currentpos ++;
                text.text = word.Substring(0,currentpos);
                if (currentpos>=word.Length)
                {
                    FinishStartWrite();
                }
            }
        }
    }
    private void FinishStartWrite()
    {
        AudioSourceManager.instance.StopPlayMusic();
        timer = 0;
        currentpos = 0;
        startWrite = false;
        text.text = word;
        Invoke("LoadNextScenes",2);
    }
    private void LoadNextScenes()
    {
        loadGame.LoadNextScene();
    }
    // Update is called once per frame
    void Update()
    {
        StartWrite();
    }
}
