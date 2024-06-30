using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//�������ԵĽṹ��
public struct WEAPONPROPERTIES
{
    //��׼
    public double minInaccuracy;
    public double maxInaccuracy;

    public double destabilization;
    public double recoilForce;
    public double aimingDeSpeed;
    public double weight;
    public double repulsion;//�ӵ��Ļ����� 

    //������ʱ����
    public double attackCD;
    public double reload;//װ��ʱ�� 

    //��ҩ
    public int magazine;
    public int totalBullects;
}
//����
public enum WEAPONTYPE
{
    PISTOl,
    AK,
    MG,
    SNIPE,
    SHOTGUN,
    CROSSBOW
}
/// <summary>
/// �������ű�
/// </summary>
public class PlayerShooting : MonoBehaviour
{
    [Header("***�ӵ�����������Դ***")]
    [SerializeField] private GameObject[] bullets;
    [SerializeField] private GameObject[] turretGos;
    [SerializeField] private AudioClip[] shootClips;
    [SerializeField] private AudioClip reloadClip;

    [Header("***�����Լ���׼����Դ***")]
    [SerializeField] private GameObject[] weapons;
    [SerializeField] private GameObject mineGo;
    [SerializeField] private GameObject leftLine;
    [SerializeField] private GameObject rightLine;
    [SerializeField] ParticleSystem effect_shoot;

    private Player player;
    [SerializeField]private WEAPONTYPE weaponType;
    private int turretLevel;

    #region ��ǰ�ȼ�ǹ������
    //��׼
    public float minInaccuracy;
    public float maxInaccuracy;

    public float destabilization;
    public float recoilForce;
    public float aimingDeSpeed;
    public float weight;
    public float repulsion;//�ӵ��Ļ����� 

    //������ʱ����
    public float attackCD;
    public float reload;//װ��ʱ�� 

    //��ҩ
    public int magazine;
    public int totalBullects;
    #endregion

    #region ��ǰǹ������(����Ϸ�����б仯������)
    private float curInaccuracy;
    private float curMinInaccuracy;
    private float curMaxInaccuracy;

    private float curDestabilization;
    private float curRecoilForce;
    private float curAimingSpeed;
    private float curWeight;
    private float curRepulsion;

    private float curAttakCD;
    private float curReload;

    private int curMagzine;
    [HideInInspector]
    public int curBullects;
    [HideInInspector]
    public bool hasTurret;
    [HideInInspector]
    public int curMine;
    [HideInInspector]
    public int maxMine;

    #endregion

    private WEAPONPROPERTIES weaponPropertiesList;
    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.player;
        weaponPropertiesList = GameManager.instance.weaponProtiesList[GameManager.instance.gunLevel];
        switch (GameManager.instance.gunLevel)
        {
            case 0:
                weaponType = WEAPONTYPE.PISTOl;
                break;
            case 1:
                weaponType = WEAPONTYPE.AK;
                break;
            case 2:
                weaponType = WEAPONTYPE.MG;
                break;
            case 3:
                weaponType = WEAPONTYPE.SNIPE;
                break;
            case 4:
                weaponType = WEAPONTYPE.SHOTGUN;
                break;
            case 5:
                weaponType = WEAPONTYPE.CROSSBOW;
                break;
        }
        weapons[GameManager.instance.gunLevel].SetActive(true);

        hasTurret = true;
        turretLevel = GameManager.instance.gunLevel / 2;
        if (turretLevel==0)
        {
            turretLevel = 1;
        }
        curMine = maxMine = 3;

        //���ǹ������ֵ
        minInaccuracy = (float)weaponPropertiesList.minInaccuracy;
        maxInaccuracy = (float)weaponPropertiesList.maxInaccuracy;

        destabilization = (float)weaponPropertiesList.destabilization;
        recoilForce = (float)weaponPropertiesList.recoilForce;
        aimingDeSpeed = (float)weaponPropertiesList.aimingDeSpeed;
        weight = (float)weaponPropertiesList.weight;
        repulsion = (float)weaponPropertiesList.repulsion;

        attackCD = (float)weaponPropertiesList.attackCD;
        reload = (float)weaponPropertiesList.reload;

        magazine = weaponPropertiesList.magazine;
        totalBullects = weaponPropertiesList.totalBullects;

        //��ǰǹ��仯������
        curAttakCD = curReload = 0;
        curMagzine = magazine;
        curBullects = totalBullects;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateDestabilization();
        CalculateInaccuracy();
        CalculateCD();
        MonitorInput();
    }

    private void CalculateCD()
    {
        if (curAttakCD>0)
        {
            curAttakCD -= Time.deltaTime;
        }
        GameManager.instance.gameUIManager.UpdateAttackCDUI((attackCD-curAttakCD)/attackCD);
        if (curReload>0)
        {
            curReload -= Time.deltaTime;
            GameManager.instance.gameUIManager.UpdateReloadAndMagazineBullets((reload-curReload)/reload);
        }
        else
        {
            if (curMagzine<=0)
            {
                curMagzine = magazine;
            }
        }
    }

    private void MonitorInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            SetTurret();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            SetMine();
        }
    }
    public void SetTurret()
    {
        if (hasTurret == true)
        {
            //��������
            Instantiate(turretGos[turretLevel], transform.position, Quaternion.identity);
            hasTurret = false;
            GameManager.instance.gameUIManager.UpdateTurretUI(hasTurret);
        }
    }
    public void SetMine()
    {
        if (curMine > 0)
        {
            Instantiate(mineGo, transform.position, Quaternion.identity);
            //���õ���
            curMine -= 1;
            GameManager.instance.gameUIManager.UpdateLeafMineUI((float)curMine / maxMine);
        }
    }
    /// <summary>
    /// ���
    /// </summary>
    public void Shoot()
    {
        if (curBullects > 0 && curMagzine > 0 && curAttakCD <= 0)
        {
            switch (weaponType)
            {
                case WEAPONTYPE.PISTOl:
                case WEAPONTYPE.AK:
                case WEAPONTYPE.MG:
                    CreatBullet(0);
                    break;
                case WEAPONTYPE.SNIPE:
                    CreatBullet(1);
                    break;
                case WEAPONTYPE.SHOTGUN:
                    CreatBullet(2);
                    break;
                case WEAPONTYPE.CROSSBOW:
                    CreatBullet(3);
                    break;
            }


            if (weaponType!=WEAPONTYPE.PISTOl||weaponType!=WEAPONTYPE.CROSSBOW)
            {
                effect_shoot.Play();
            }
            //�ӵ���������
            curMagzine -= 1;
            GameManager.instance.gameUIManager.UpdateReloadAndMagazineBullets((float)curMagzine/magazine);
            curBullects -= 1;
            LimitBullet();
            //����CD����
            curAttakCD = attackCD;
            //����ȷ�ȵ���
            curInaccuracy += recoilForce;
            if (curInaccuracy>=maxInaccuracy)
            {
                curInaccuracy = maxInaccuracy;
            }
            //�Զ���װ�ӵ�
            if (curReload<=0&&curMagzine<=0)
            {
                Reload();
            }
        }
    }
    /// <summary>
    /// �����ӵ�
    /// </summary>
    /// <param name="bulletIndex"></param>
    private void CreatBullet(int bulletIndex)
    {
        AudioSourceManager.instance.PlaySound(shootClips[GameManager.instance.gunLevel]);
        if (weaponType==WEAPONTYPE.SHOTGUN)
        {
            float index=0;
            for (int i = 0; i < 5; i++)
            {
                Instantiate(bullets[bulletIndex], transform.position, Quaternion.Euler(
                0, 0, transform.rotation.eulerAngles.z + (-curInaccuracy+curInaccuracy*index)));
                index+=0.5f;
            }
        }
        else
        {
            Instantiate(bullets[bulletIndex], transform.position, Quaternion.Euler(
                0, 0, transform.rotation.eulerAngles.z + UnityEngine.Random.Range(-curInaccuracy, curInaccuracy)));
        }
        
    }
    /// <summary>
    /// ��װ�ӵ�
    /// </summary>
    public void Reload()
    {
        if (curMagzine<magazine)
        {
            AudioSourceManager.instance.PlaySound(reloadClip);
            curReload = reload;
            GameManager.instance.gameUIManager.UpdateReloadAndMagazineBullets((float)curMagzine / magazine);
            curMagzine = 0;
        }
    }
    private void CalculateInaccuracy()
    {
        if (curInaccuracy >= minInaccuracy + curDestabilization)
        {
            curInaccuracy -= Time.deltaTime * aimingDeSpeed;
        }
        else
        {
            curInaccuracy = minInaccuracy + curDestabilization;
        }
        leftLine.transform.localRotation = Quaternion.AngleAxis(curInaccuracy, Vector3.forward);
        rightLine.transform.localRotation = Quaternion.AngleAxis(-curInaccuracy, Vector3.forward);
    }
    /// <summary>
    /// ���㲻��ȷ��
    /// </summary>
    private void CalculateDestabilization()
    {
        //Debug.Log(player.isMoving);
        if (player.isMoving == true)
        {
            if (curDestabilization < destabilization)
            {
                curDestabilization += Time.deltaTime * 1.2f;
            }
            else
            {
                curDestabilization = destabilization;
            }
        }
        else
        {
            curDestabilization = 0;
        }
    }

    public void LimitBullet()
    {
        if (curBullects>=totalBullects)
        {
            curBullects = totalBullects;
        }
        else if (curBullects<=0)
        {
            curBullects = 0;
        }
        GameManager.instance.gameUIManager.UpdateBullet((float)curBullects / totalBullects);
    }
}
