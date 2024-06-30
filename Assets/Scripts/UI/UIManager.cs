using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;
public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameManager;
    [Header("***UI资源***")]
    [SerializeField] private Image[] imageSelect; //选中的图标按钮
    [SerializeField] private GameObject[] panels;
    [SerializeField] private TextMeshProUGUI txt_Money;
    private int mainChoice;

    [Header("***按钮***")]
    [SerializeField] Button btn_start;
    [SerializeField] Button btn_reset;
    [SerializeField] Button btn_quitGame;

    [Header("***设置界面***")]
    [SerializeField] private Image img_joyStick;
    [SerializeField] private Image img_volumn;
    [SerializeField] private Sprite muteSprite;
    [SerializeField] private Sprite volSprite;

    [Header("***武器界面***")]
    [SerializeField] private Image[] imageSelecteWeapon;
    [SerializeField] private TextMeshProUGUI[] txt_weaponSelect;
    private Button[] btn_weapons;

    private int[] weaponPrice;

    [Header("***选关***")]
    [SerializeField] private Image[] imageSelectedLevel;
    private Button[] btn_selected;
    [SerializeField] private TextMeshProUGUI[] txt_selectLevel;

    [Header("***音频***")]
    [SerializeField] private AudioClip[] clickClip;
    // Start is called before the first frame update
    private void Awake()
    {
        if (gameManager==null)
        {
            Instantiate(gameManager);
        }
    }
    void Start()
    {
        weaponPrice = new int[] { 0, 100, 200, 350, 400, 850 };
        //选择武器按钮组件的获取
        btn_weapons = new Button[imageSelecteWeapon.Length];
        txt_weaponSelect = new TextMeshProUGUI[imageSelecteWeapon.Length];
        for (int i = 0; i < imageSelecteWeapon.Length; i++)
        {
            int index = i+1;
            btn_weapons[i] = imageSelecteWeapon[i].GetComponent<Button>();
            btn_weapons[i].onClick.AddListener(delegate { SelectGun(index); });

            txt_weaponSelect[i] = imageSelecteWeapon[i].transform.GetChild(0).
                GetComponent<TextMeshProUGUI>();
        }
        
        //选关按钮组件的获取,并注册事件
        btn_selected = new Button[imageSelectedLevel.Length];
        txt_selectLevel = new TextMeshProUGUI[imageSelectedLevel.Length];

        for (int i = 0; i < imageSelectedLevel.Length; i++)
        {
            int index = i+1;
            btn_selected[i] = imageSelectedLevel[i].GetComponent<Button>();
            btn_selected[i].onClick.AddListener(delegate { SelectLevel(index); });

            txt_selectLevel[i] = imageSelectedLevel[i].transform.GetChild(0).
                GetComponent<TextMeshProUGUI>();
        }
        
        //按钮注册事件
        //公共面板按钮
        for (int i = 0; i < imageSelect.Length; i++)
        {
            int index = i;
            imageSelect[i].GetComponent<Button>().onClick.AddListener(() =>
            {
                AudioSourceManager.instance.PlaySound(clickClip[1]);
                ShowPanel(index);
            });
        }

        btn_start.onClick.AddListener(StartGame);
        //设置页面按钮
        img_joyStick.GetComponent<Button>().onClick.AddListener(SetJoyStick);
        img_volumn.GetComponent<Button>().onClick.AddListener(SetVolumn);
        btn_reset.onClick.AddListener(ResetData);
        btn_quitGame.onClick.AddListener(ExitGame);
        //初始化
        UpdateMoneyText();  
        InitUI();
    }
    #region 初始化UI
    private void InitUI()
    {
        InitCommon();
        InitSetting();
        InitWeapons();
        InitLevels();
    }
    private void InitCommon()
    {
        mainChoice = 3;
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }

        for (int i = 0; i < imageSelect.Length; i++)
        {
            imageSelect[i].color = new Color(0, 1, 0, 0.3f);
        }
        imageSelect[mainChoice - 1].color = new Color(0, 1, 0, 1);
        panels[mainChoice - 1].SetActive(true);
    }
    private void InitSetting()
    {
        img_joyStick.rectTransform.localScale = new Vector2(GlobalValue.JoyStickSize * 2,
            GlobalValue.JoyStickSize * 2);
        //音量图标初始化
        if (GlobalValue.Volumn == 0)
        {
            img_volumn.sprite = muteSprite;
            img_volumn.color = new Color(1, 0, 0, 0.3f);
        }
        else
        {
            img_volumn.sprite = volSprite;
            img_volumn.color = new Color(0, 1, 0, GlobalValue.Volumn);
        }
    }
    private void InitWeapons()
    {
        Debug.Log("当前的金钱是"+GlobalValue.Money);
        for (int i = 0; i < imageSelecteWeapon.Length; i++)
        {
            if (GlobalValue.Money >= weaponPrice[i])
            {
                Debug.Log("初始化武器");
                imageSelecteWeapon[i].color = new Color(0, 1, 0, 0.3f);
                btn_weapons[i].interactable = true;
                txt_weaponSelect[i].color = new Color(0, 1, 0, 0.3f);
            }
            else
            {
                
                imageSelecteWeapon[i].color = new Color(1, 0, 0, 0.3f);
                btn_weapons[i].interactable = false;
                txt_weaponSelect[i].color = new Color(1, 0, 0, 0.3f);
            }
        }
        imageSelecteWeapon[GlobalValue.GunLevel-1].color = new Color(0, 1, 0, 1);
        txt_weaponSelect[GlobalValue.GunLevel-1].color = new Color(0, 1, 0, 1);
    }
    private void InitLevels()
    {
        for (int i = 0; i < imageSelectedLevel.Length; i++)
        {
            if (GlobalValue.UnLockLevel >= i + 1)
            {
                
                imageSelectedLevel[i].color = new Color(0, 1, 0, 0.3f);
                btn_selected[i].interactable = true;
                txt_selectLevel[i].color = new Color(0, 1, 0, 0.3f);
            }
            else
            {
                imageSelectedLevel[i].color = new Color(1, 0, 0, 0.3f);
                btn_selected[i].interactable = false;
                txt_selectLevel[i].color = new Color(1, 0, 0, 0.3f);
            }
        }
        Debug.Log("有解锁关卡" + GlobalValue.SelectLevel);
        imageSelectedLevel[GlobalValue.SelectLevel-1].color = new Color(0,1,0,1);
        txt_selectLevel[GlobalValue.SelectLevel-1].color = new Color(0,1,0,1);
    }
    #endregion

    #region 公共面板
    private void UpdateMoneyText()
    {
        txt_Money.text = GlobalValue.Money.ToString();
    }
    private void ShowPanel(int choice)
    {
        mainChoice = choice;
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
            imageSelect[i].color = new Color(0,1,0,0.3f);
        }
        panels[choice].SetActive(true);
        imageSelect[choice].color = new Color(0,1,0,1);
    } 
    public void StartGame()
    {
        SceneManager.LoadScene(7);
    }
    #endregion

    #region 设置面板
    private void SetJoyStick()
    {
        AudioSourceManager.instance.PlaySound(clickClip[0]);
        GlobalValue.JoyStickSize += 0.05f;
        if (GlobalValue.JoyStickSize > 0.5f)
        {
            GlobalValue.JoyStickSize = 0.3f;
        }
        img_joyStick.rectTransform.localScale = new Vector2(GlobalValue.JoyStickSize * 2,
            GlobalValue.JoyStickSize * 2);
    }
    private void SetVolumn()
    {
        AudioSourceManager.instance.PlaySound(clickClip[0]);
        GlobalValue.Volumn += 0.05f;
        if (GlobalValue.Volumn > 1)
        {
            GlobalValue.Volumn = 0;
            img_volumn.sprite = muteSprite;
            img_volumn.color = new Color(1, 0, 0, 0.3f);
        }
        else
        {
            img_volumn.sprite = volSprite;
            img_volumn.color = new Color(0, 1, 0, GlobalValue.Volumn);
        }
    }
    private void ResetData()
    {
        AudioSourceManager.instance.PlaySound(clickClip[0]);
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(0);
    }
    private void ExitGame()
    {
        AudioSourceManager.instance.PlaySound(clickClip[0]);
        Application.Quit();
    }
    #endregion

    #region 武器面板
    private void SelectGun(int gunlevel)
    {
        Debug.Log("选抢了");
        AudioSourceManager.instance.PlaySound(clickClip[0]);
        GlobalValue.GunLevel = gunlevel;
        InitWeapons();
        //imageSelecteWeapon[GlobalValue.GunLevel - 1].color = new Color(0, 1, 0, 1);
        //txt_weaponSelect[GlobalValue.GunLevel - 1].color = new Color(0, 1, 0, 1);
    }

    public void SelectLevel(int selectLevel)
    {
        Debug.Log("选关了"+ selectLevel);
        AudioSourceManager.instance.PlaySound(clickClip[0]);
        GlobalValue.SelectLevel = selectLevel;
        InitLevels();
    }
    #endregion
    // Update is called once per frame
    void Update()
    {

    }
}
