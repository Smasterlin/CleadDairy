using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameUIManager : MonoBehaviour
{
    private PlayerShooting playerShooting;
    [Header("***Ѫ�����ӵ���UI��ʾ***")]
    [SerializeField] private Slider hp_slider;
    [SerializeField] private Slider bullet_slider;

    private CanvasGroup canvasGroup;
    [Header("***Pc����������ʾ***")]
    [SerializeField] private GameObject joyStick;
    [SerializeField] private Image img_weapon;
    [SerializeField] private Sprite[] sprite_weapon;

    [Header("***���½�ui��ʾ***")]
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
        //��ťע���¼�
        img_attackCD.transform.parent.GetComponent<Button>().onClick.AddListener(OnClickAttackButton);
        img_reloadCD.transform.parent.GetComponent<Button>().onClick.AddListener(OnClickReloadButton);
        img_leftMine.transform.parent.GetComponent<Button>().onClick.AddListener(OnClickMineButton);
        img_turret.transform.parent.GetComponent<Button>().onClick.AddListener(OnclickTurretButton);
        //��ƽ̨����
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
    #region UI�ĸ�����ʾ
    /// <summary>
    /// ����Ѫ��UI��ʾ
    /// </summary>
    /// <param name="value"></param>
    public void UpdateHp(float value)
    {
        hp_slider.value = value;
    }
    /// <summary>
    /// �����ӵ�UI��ʾ
    /// </summary>
    /// <param name="value"></param>
    public void UpdateBullet(float value)
    {
        bullet_slider.value = value;
    }
    /// <summary>
    /// ����UI����ʾ
    /// </summary>
    /// <param name="value"></param>
    public void UpdateLeafMineUI(float value)
    {
        img_leftMine.fillAmount = value;
    }
    /// <summary>
    /// ����UI����ʾ
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
    /// ������ʱ����UI����ʾ
    /// </summary>
    /// <param name="value"></param>
    public void UpdateAttackCDUI(float value)
    {
        img_attackCD.fillAmount = value;
    }
    /// <summary>
    /// װ��UI����ʾ
    /// </summary>
    /// <param name="value"></param>
    public void UpdateReloadAndMagazineBullets(float value)
    {
        img_reloadCD.fillAmount = value;
    }
    #endregion

    #region ��ť�¼�
    public void OnClickAttackButton()
    {
        Debug.Log("�����ӵ���");
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
