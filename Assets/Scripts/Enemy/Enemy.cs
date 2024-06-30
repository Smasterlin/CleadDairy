using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Enemy : MonoBehaviour
{
    //引用
    protected Player player;
    protected Transform playerTrans;
    protected Vector3 playerLastTrans;
    protected Rigidbody2D rigid;
    protected LayerMask layerMask;

    [Header("***敌人属性***")]
    [SerializeField] protected float speed;
    [SerializeField] protected bool follow;
    [SerializeField]protected int reward;
    [SerializeField] protected float HP;
    [SerializeField] protected float curHp;
    protected RaycastHit2D hit;

    [Header("***资源引用***")]
    [SerializeField] protected GameObject[] bloodGos;//被玩家子弹打到受伤的血迹
    [SerializeField] protected GameObject deadBloos;
    [SerializeField] protected GameObject effect_blood;//受伤溅血的特效
    [SerializeField] protected GameObject effect_explode;
    [SerializeField] protected AudioClip explodeClip;
    protected virtual void Start()
    {
        player = GameManager.instance.player;
        playerTrans = player.transform;
        player.enemyList.Add(this);
        //reward = 20;

        //curHp = HP=50;
        rigid = GetComponent<Rigidbody2D>();
        layerMask = ~(1 << 10) & ~(1 << 2);

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (playerTrans==null)
        {
            return;
        }
        hit = Physics2D.Raycast(transform.position,playerTrans.position-transform.position,
            4,layerMask);
        SearchAndFollow();
        Move();
    }
    /// <summary>
    /// 看向玩家并攻击
    /// </summary>
    protected virtual void LookAtPlayerAndAttack()
    {
        Vector3 moveDirection = playerTrans.position - transform.position;
        if (moveDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveDirection.x, moveDirection.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, -Vector3.forward);

            follow = true;
            playerLastTrans = playerTrans.position;
            //Debug.Log(222);
        }
    }
    protected void SearchAndFollow()
    {
        if (follow==true&& Vector3.Distance(transform.position,playerLastTrans)<0.1f)
        {
            follow = false;
            Debug.Log(111);
        }
        //确定僵尸转向
        if (hit.collider!=null)//射线检测有检测到玩家
        {
            if (!hit.collider.CompareTag("Wall"))
            {
                LookAtPlayerAndAttack();
            }
            else if (follow == true)//丢失玩家目标，只检测到墙
            {
                Vector3 moveDirection = playerLastTrans - transform.position;
                if (moveDirection != Vector3.zero)
                {
                    float angle = Mathf.Atan2(moveDirection.x, moveDirection.y) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.AngleAxis(angle, -Vector3.forward);
                    //Debug.Log(333);
                }
            }
        }
        else if (follow == true)//没有检测到东西
        {
            Vector3 moveDirection = playerLastTrans - transform.position;
            if (moveDirection != Vector3.zero)
            {
                float angle = Mathf.Atan2(moveDirection.x, moveDirection.y) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, -Vector3.forward);
                //Debug.Log(333);
            }
        }

    }
    protected void Move()
    {
        if (hit.collider!=null)//向玩家移动
        {
            if (hit.collider.CompareTag("Player"))
            {
                rigid.AddRelativeForce(new Vector2(0,speed));
                //Debug.Log(5555);
            }
        }
        if (follow==true)//丢失玩家目标，想着玩家最后一次位置移动
        {
            rigid.AddRelativeForce(new Vector2(0,speed*0.5f));
            //Debug.Log(444);
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            //被子弹打到，产生溅血特效
            Instantiate(effect_blood,transform.position,Quaternion.Euler(0,0,
               collision.transform.rotation.eulerAngles.z+180));
            if (collision.gameObject.name.Contains("Bullect0"))
            {
                Debug.Log("子弹一打中扣血了");
                curHp -= 10 * GlobalValue.GunLevel * 0.5f;
                Instantiate(bloodGos[0], transform.position, Quaternion.Euler(0, 0,
                    collision.transform.rotation.eulerAngles.z + Random.Range(-15, 15)));
            }
            if (collision.gameObject.name.Contains("Bullect1"))
            {
                curHp -= 50;
                Instantiate(bloodGos[1], transform.position, Quaternion.Euler(
                    0, 0, collision.transform.rotation.eulerAngles.z));
            }
            if (collision.gameObject.name.Contains("Bullect2"))
            {
                curHp -= 20;
                Instantiate(bloodGos[2], transform.position, Quaternion.Euler(0, 0
                    , collision.transform.rotation.eulerAngles.z + Random.Range(-20, 20)));
            }
            if (collision.gameObject.name.Contains("Bullect3"))
            {
                curHp -= 40;
                Instantiate(bloodGos[2], transform.position, Quaternion.Euler(0, 0,
                    collision.transform.rotation.eulerAngles.z));
            }
            
            //子弹打到敌人身上，对敌人产生的后退力
            rigid.AddRelativeForce(new Vector2(0,GameManager.instance.playerShooting.repulsion));
        }

        if (collision.CompareTag("Mine"))
        {
            curHp -= 70;
            AudioSourceManager.instance.PlaySound(explodeClip);
            Instantiate(effect_explode,transform.position,transform.rotation);
            Destroy(collision.gameObject);
        }
        Die();
    }

    protected virtual void Die()
    {
        if (curHp<=0)
        {
            player.curKills += 1;
            GlobalValue.Money += reward;
            player.enemyList.Remove(this);
            Instantiate(deadBloos,transform.position,transform.rotation);
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// 僵尸碰到玩家，对玩家的持续性伤害
    /// </summary>
    /// <param name="collision"></param>
    protected void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.currentHp -= 0.5f;
            player.delayRegen = false;
            player.LimitHp();
            player.PlayerDie();
        }
        else
        {
            player.delayRegen = true;
        }
    }
}
