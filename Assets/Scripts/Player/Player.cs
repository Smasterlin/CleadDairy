using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour
{
    private bool decreaseHp;
    private bool isBossDead;
    [Header("***玩家引用资源***")]
    [SerializeField] private GameObject blood;
    [SerializeField] private GameObject bloodParticle;
    [SerializeField] private AudioClip bonusClip;
    public PlayerShooting playerShooting;

    [SerializeField] private GameObject effect_explode;
    [SerializeField] private AudioClip explodeClip;
    [SerializeField] private GameObject blood_dead;
    public List<Enemy> enemyList;

    public bool isMoving;
    private int moveSpeed;
    public float moveAngle;
    public int maxHP;
    public float currentHp;

    private float regenHpSpeed;
    private float delayRegenHp;
    [HideInInspector]
    public float delayTimer;
    [HideInInspector]
    public bool delayRegen = true;

    private Rigidbody2D playerRig;
    //private AudioSource audioSource;
    [Header("击杀敌人的数量")]
    public int kills;
    public int curKills;
    private void Awake()
    {

    }
    /// <summary>
    /// 测试
    /// </summary>
    private void OnEnable()
    {
        //GameManager.instance.player = this;
        enemyList = new();
        maxHP = 100;
        currentHp = 100;
        regenHpSpeed = delayRegenHp = 1;
        moveSpeed = 2;
        //audioSource = GetComponent<AudioSource>();
        playerRig = GetComponent<Rigidbody2D>();
        if (!GameManager.instance.isAnthonyDead && GlobalValue.SelectLevel == 6)
        {
            decreaseHp = true;
        }
        if (GlobalValue.SelectLevel == 3 || GlobalValue.SelectLevel == 5||GlobalValue.SelectLevel==6)
        {
            isBossDead = true;
        }
    }
    private void Start()
    {

    }
    private void Update()
    {
        PlayerMove();
        if (decreaseHp==true)
        {
            currentHp -= 1 * Time.deltaTime;
            LimitHp();
            PlayerDie();
        }
        else
        {
            RegenHp();
        }
        
    }
    private void PlayerMove()
    {
        isMoving = false;
        //摇杆移动方式
        #region 摇杆移动方式
        playerRig.AddForce(new Vector2(GameManager.instance.inputValue.x * moveSpeed / (1 + 0.1f * GameManager.instance.playerShooting.weight) * 70 * Time.deltaTime,
            GameManager.instance.inputValue.y * moveSpeed / (1 + 0.1f * GameManager.instance.playerShooting.weight) * 70 * Time.deltaTime));
        if (!(GameManager.instance.inputValue == Vector2.zero))
        {
            isMoving = true;
            moveAngle = GameManager.instance.inputAngle;
        }
        else
        {
            isMoving = false;
            moveAngle = 0;
        }
        //isMoving = GameManager.instance.inputValue == Vector2.zero;
        #endregion


        //Pc端移动方式
        #region Pc端移动方式

        if (Input.GetKey(KeyCode.W))
        {
            playerRig.AddForce(new Vector2(0, moveSpeed / (1 + 0.1f * GameManager.instance.playerShooting.weight)) * 70 * Time.deltaTime);
            moveAngle = 0;
            isMoving = true;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            playerRig.AddForce(new Vector2(0, -moveSpeed / (1 + 0.1f * GameManager.instance.playerShooting.weight)) * 70 * Time.deltaTime);
            moveAngle = 180;
            isMoving = true;
        }

        if (Input.GetKey(KeyCode.A))
        {
            playerRig.AddForce(new Vector2(-moveSpeed / (1 + 0.1f * GameManager.instance.playerShooting.weight), 0) * 70 * Time.deltaTime);
            moveAngle = 90;
            isMoving = true;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            playerRig.AddForce(new Vector2(moveSpeed / (1 + 0.1f * GameManager.instance.playerShooting.weight), 0) * 70 * Time.deltaTime);
            moveAngle = -90;
            isMoving = true;
        } 
        #endregion
        //transform.rotation = Quaternion.Euler(0,0,moveAngle);
        //transform.eulerAngles = new Vector3(0, 0, moveAngle);
    }
    /// <summary>
    /// 自动回血
    /// </summary>
    private void RegenHp()
    {
        if (delayTimer <= 0 && currentHp < maxHP &&delayRegen==true)
        {
            currentHp += Time.deltaTime;
            LimitHp();
        }
        else if (delayTimer > 0)
        {
            delayTimer -= Time.deltaTime;
        }
    }
    /// <summary>
    /// 道具功能点
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string colStr = collision.tag;
        switch (colStr)
        {
            case "EnemyBullet"://玩家被僵尸士兵的子弹打中
                Instantiate(bloodParticle,transform.position,Quaternion.Euler(0,0,
                    transform.rotation.eulerAngles.z+180));
                Instantiate(blood,transform.position,Quaternion.Euler(0,0,
                    transform.rotation.eulerAngles.z + 180+ Random.Range(-15, 15)));
                Destroy(collision.gameObject);
                currentHp -= 10;
                LimitHp();

                PlayerDie();
                break;
            case "BulletItem":

                if (playerShooting.curBullects < playerShooting.totalBullects)
                {
                    playerShooting.curBullects += (int)(playerShooting.totalBullects * 0.25f);
                    playerShooting.LimitBullet();
                    DestroyItem(collision);
                    Debug.Log("拿到子弹箱子了");
                }
                break;
            case "HeathItem":
                if (currentHp < maxHP)
                {
                    currentHp += 20;
                    LimitHp();
                    DestroyItem(collision);
                }
                break;
            case "MineItem":
                if (playerShooting.curMine < playerShooting.maxMine)
                {
                    playerShooting.curMine += 1;
                    GameManager.instance.gameUIManager.UpdateLeafMineUI((float)playerShooting.curMine / playerShooting.maxMine);
                    DestroyItem(collision);
                }
                break;
            case "TurretItem":
                if (playerShooting.hasTurret == false)
                {
                    playerShooting.hasTurret = true;
                    GameManager.instance.gameUIManager.UpdateTurretUI(playerShooting.hasTurret);
                    DestroyItem(collision);
                }
                break;
            case "MoneyItem":
                GlobalValue.Money += 5;
                DestroyItem(collision);
                break;
            case "Key":
                collision.GetComponent<Key>().OpenDoor();
                DestroyItem(collision);
                break;
            case "EnemyMine":
                currentHp -= 70;
                LimitHp();
                AudioSourceManager.instance.PlaySound(explodeClip);
                Instantiate(effect_explode, transform.position, transform.rotation);
                Destroy(collision.gameObject);
                PlayerDie();
                break;
        }
    }

    private void DestroyItem(Collider2D collision)
    {
        AudioSourceManager.instance.PlaySound(bonusClip);

        Destroy(collision.gameObject);
    }
    /// <summary>
    /// 补给箱
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {

            //子弹补给箱
            if (collision.gameObject.name.Contains("BulletsBox"))
            {
                if (playerShooting.curBullects < playerShooting.totalBullects)
                {
                    AudioSourceManager.instance.PlaySound(bonusClip);
                    playerShooting.curBullects += (int)(playerShooting.totalBullects * 0.25f);
                    playerShooting.LimitBullet();
                }
            }
            //血量补给箱
            if (collision.gameObject.name.Contains("HeathBox"))
            {
                if (currentHp < maxHP)
                {
                    AudioSourceManager.instance.PlaySound(bonusClip);
                    currentHp += 20;
                    LimitHp();
                }
            }

            //地雷和炮塔的补给箱
            if (collision.gameObject.name.Contains("TurretBox"))
            {
                if (playerShooting.curMine < playerShooting.maxMine)
                {
                    AudioSourceManager.instance.PlaySound(bonusClip);
                    playerShooting.curMine = 3;
                    GameManager.instance.gameUIManager.UpdateLeafMineUI(1);
                }
                if (playerShooting.hasTurret == false)
                {
                    AudioSourceManager.instance.PlaySound(bonusClip);
                    playerShooting.hasTurret = true;
                    GameManager.instance.gameUIManager.UpdateTurretUI(playerShooting.hasTurret);
                }

            }
        }
    }
    /// <summary>
    /// 玩家死亡
    /// </summary>
    public void PlayerDie()
    {
        if (currentHp<=0)
        {
            GameManager.instance.LoadMainScene();
            Instantiate(blood_dead,transform.position,transform.rotation);
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// 限制血量以及血量的显示
    /// </summary>
    public void LimitHp()
    {
        if (currentHp>=maxHP)
        {
            currentHp = maxHP;
        }
        else if(currentHp<=0)
        {
            currentHp = 0;
        }
        GameManager.instance.gameUIManager.UpdateHp(Mathf.Clamp(currentHp / maxHP,0,1));
    }
  
}
