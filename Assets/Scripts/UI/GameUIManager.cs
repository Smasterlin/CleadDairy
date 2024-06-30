using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameUIManager : MonoBehaviour
{
    private PlayerShooting playerShooting;
    [Header("***血量和子弹的UI显示***")]
    [SerializeField] private Slider hp_slider;
    [SerializeField] private Slider bullet_slider;

    private CanvasGroup canvasGroup;
    [Header("***Pc端武器的显示***")]
    [SerializeField] private GameObject joyStick;
    [SerializeField] private Image img_weapon;
    [SerializeField] private Sprite[] sprite_weapon;

    [Header("***右下角ui显示***")]
    [SerializeField] private Image img_attackCD;
    [SerializeField] private Image img_reloadCD;
    [SerializeField] private Image img_leftMine;
    [SerializeField] private Image img_turret;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.gameUIManager = this;
        canvasGroup = GetComponent<CanvasGroup>();
        playerShooting = GameManager.instance.player.playerShooting;
        //按钮注册事件
        img_attackCD.transform.parent.GetComponent<Button>().onClick.AddListener(OnClickAttackButton);
        img_reloadCD.transform.parent.GetComponent<Button>().onClick.AddListener(OnClickReloadButton);
        img_leftMine.transform.parent.GetComponent<Button>().onClick.AddListener(OnClickMineButton);
        img_turret.transform.parent.GetComponent<Button>().onClick.AddListener(OnclickTurretButton);
        //各平台差异
#if UNITY_STANDALONE_WIN
        canvasGroup.interactable = true;
        img_weapon.gameObject.SetActive(true);
        img_weapon.sprite = sprite_weapon[GlobalValue.GunLevel - 1];
        joyStick.SetActive(true);

#elif UNITY_ANDROID
        canvasGroup.interactable = true;
        img_weapon.gameObject.SetActive(false);
        joyStick.SetActive(true);
#endif
      
    }
    #region UI的更新显示
    /// <summary>
    /// 更新血量UI显示
    /// </summary>
    /// <param name="value"></param>
    public void UpdateHp(float value)
    {
        hp_slider.value = value;
    }
    /// <summary>
    /// 更新子弹UI显示
    /// </summary>
    /// <param name="value"></param>
    public void UpdateBullet(float value)
    {
        bullet_slider.value = value;
    }
    /// <summary>
    /// 地雷UI的显示
    /// </summary>
    /// <param name="value"></param>
    public void UpdateLeafMineUI(float value)
    {
        img_leftMine.fillAmount = value;
    }
    /// <summary>
    /// 炮塔UI的显示
    /// </summary>
    /// <param name="hasTurret"></param>
    public void UpdateTurretUI(bool hasTurret)
    {
        if (hasTurret == true)
        {
            img_turret.fillAmount = 1;
        }
        else
        {
            img_turret.fillAmount = 0;
        }
    }
    /// <summary>
    /// 攻击的时间间隔UI的显示
    /// </summary>
    /// <param name="value"></param>
    public void UpdateAttackCDUI(float value)
    {
        img_attackCD.fillAmount = value;
    }
    /// <summary>
    /// 装弹UI的显示
    /// </summary>
    /// <param name="value"></param>
    public void UpdateReloadAndMagazineBullets(float value)
    {
        img_reloadCD.fillAmount = value;
    }
    #endregion

    #region 按钮事件
    public void OnClickAttackButton()
    {
        Debug.Log("发射子弹了");
        playerShooting.Shoot();
    }
    private void OnClickMineButton()
    {
        playerShooting.SetMine();
    }
    private void OnclickTurretButton()
    {
        playerShooting.SetTurret();
    }
    private void OnClickReloadButton()
    {
        playerShooting.Reload();
    } 
    #endregion
    // Update is called once per frame
    void Update()
    {

    }
}
