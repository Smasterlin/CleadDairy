using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class LoadGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txt_load;
    private AsyncOperation ao;

    [SerializeField] private GameObject panel_diary;
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.instance.showEnd==true)
        {
            ao = SceneManager.LoadSceneAsync(0);
        }
        else
        {
            ao = SceneManager.LoadSceneAsync(GlobalValue.SelectLevel);
        }
        ao.allowSceneActivation = false;
        panel_diary.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(ao.progress);
        if (ao.progress>=0.9f)
        {
            txt_load.text = "INPUT ANYKEY TO CONTINUE";
            //Debug.Log("Ìæ»»ÎÄ±¾ÁË");
            if (Input.anyKeyDown)
            {
                if (GameManager.instance.firstEnterLevels[GlobalValue.SelectLevel]==true)
                {
                    GameManager.instance.firstEnterLevels[GlobalValue.SelectLevel] = false;
                    GameManager.instance.SetBoolArray("FirstEnter",GameManager.instance.firstEnterLevels);
                    panel_diary.SetActive(true);
                    gameObject.SetActive(false);
                    
                }
                else
                {
                    LoadNextScene();
                }
                
            }
        }
    }
    public void LoadNextScene()
    {
        ao.allowSceneActivation = true;
    }
}
