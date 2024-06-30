using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnthonyZombie : EnemySoldier
{
    [SerializeField] private GameObject mineGo;
    [SerializeField] private GameObject turretGo;

    [SerializeField]private int mineCD;
    private float curMineCD;
    [SerializeField]private int turretCD;
    private float curTurretCD;
    protected override void Start()
    {
        base.Start();
        if (GameManager.instance.isAnthonyDead==true)
        {
            GameManager.instance.player.enemyList.Remove(this);
            Destroy(gameObject);
        }
        curTurretCD = 10;
    }
    protected override void Update()
    {
        base.Update();
        UseItem();
    }

    private void UseItem()
    {
        if (hit.collider == null)
        {
            return;
        }
        if (!hit.collider.CompareTag("Player"))
        {
            return;
        }
        //放置地雷
        if (curMineCD<=0)
        {
            curMineCD = mineCD;
            Instantiate(mineGo,transform.position,Quaternion.identity);
        }
        else
        {
            curMineCD -= Time.deltaTime;
        }
        //放置炮台
        if (curTurretCD<=0)
        {
            curTurretCD = turretCD;
            Instantiate(turretGo,transform.position,Quaternion.identity);
        }
        else
        {
            curTurretCD -= Time.deltaTime;
        }
    }

    protected override void LookAtPlayerAndAttack()
    {
        //敌人看向玩家
        LookAtPlayer();

        //开枪射击
        if (curAttackCD <= 0 && curMagzine > 0)
        {
            AudioSourceManager.instance.PlaySound(shootClip);
            CreatShootGunBullet();
            
            curAttackCD = attackCD;
            curMagzine -= 1;
            if (curMagzine <= 0)
            {
                curReload = reload;
            }
        }

    }
    /// <summary>
    /// 散弹枪射击
    /// </summary>
    private void CreatShootGunBullet()
    {
        float index = 0;
        for (int i = 0; i < 5; i++)
        {
            Instantiate(bulletGo,shootPoint.transform.position, Quaternion.Euler(
            0, 0, transform.rotation.eulerAngles.z + (-inaccuracy + inaccuracy * index)));
            index += 0.5f;
        }
    }
    /// <summary>
    /// 看向玩家
    /// </summary>
    private void LookAtPlayer()
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
}
