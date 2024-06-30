using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAIM : MonoBehaviour
{
    [SerializeField] private Transform cameraTrans;
    [SerializeField] private GameObject mark;

    private float miniDistance;
    private float maxDistance;

    private Player player;
    private Enemy nearestEnemy;//距离玩家最近的敌人

    private RaycastHit2D hit;
    private LayerMask layerMask;
    private int layerValue;
    private float distance;
    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.player;
        mark.SetActive(false);
        miniDistance = maxDistance = 10;
        //打开出第九层之外的射线检测，第九层是玩家自己
        layerMask = ~(1 << 9) & ~(1 << 2);
        //打开所有层级的射线检测
        //layerMask = ~(1<<0);
    }

    // Update is called once per frame
    void Update()
    {
        ChestNeastEnemy();
        //摄像机跟随
        cameraTrans.position = transform.position - new Vector3(0,0,10);
    }
    /// <summary>
    /// 射线检测最近的敌人
    /// </summary>
    private void CheckEnemy()
    {
        for (int i = 0; i < player.enemyList.Count; i++)
        {

            hit = Physics2D.Raycast(transform.position, player.enemyList[i].transform.position - transform.position
                , 10, layerMask);
            //Debug.DrawRay(transform.position, player.enemyList[i].transform.position - transform.position, Color.red, 20);

            if (hit.collider != null)
            {
                if (!hit.collider.CompareTag("Wall"))//射线检测的不是墙体
                {
                    //检测玩家和每个敌人之间的距离
                    distance = Vector3.Distance(transform.position, player.enemyList[i].transform.position);
                    if (distance < maxDistance && distance < miniDistance)
                    {
                        nearestEnemy = player.enemyList[i];
                    }
                }
            }
        }
    }
    /// <summary>
    /// 显示出最近敌人标志
    /// </summary>
    private void ShowMark()
    {
        //mark.transform.SetParent(nearestEnemy.transform);
        //mark.transform.localPosition = Vector3.zero;
        //mark.transform.localRotation = transform.rotation;

        mark.SetActive(true);
    }
    private void LateUpdate()
    {
        if (nearestEnemy!=null)
        {
            mark.transform.position = nearestEnemy.transform.position;
            if (hit.collider!=null)
            {
                if (hit.collider.CompareTag("Wall"))
                {
                    Debug.Log("检测到墙壁了");
                    miniDistance = maxDistance = 10;
                    nearestEnemy = null;
                }
            }
        }
        else
        {
            miniDistance = maxDistance = 10;
            nearestEnemy = null;
        }
    }
    /// <summary>
    /// 看向敌人
    /// </summary>
    private void LookAtEnemy()
    {
        Vector3 moveDirection = nearestEnemy.transform.position - transform.position;
        if (moveDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveDirection.x, moveDirection.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, -transform.forward);//2D世界是z轴是反的
            //Debug.Log("看向敌人了的转向角度是"+angle);
            //Debug.Log("看向敌人了的角度是"+ transform.eulerAngles);
        }  
    }
    private void ChestNeastEnemy()
    {
        if (player.enemyList.Count<=0)
        {
            return;
        }
        //确定离玩家最近的敌人
        CheckEnemy();

        if (nearestEnemy!=null)
        {
            //在最近的敌人身上显示出标志
            ShowMark();
            //玩家看向敌人， 2D游戏的看向问题
            LookAtEnemy();
           
        }
        else//丢失目标
        {
            mark.SetActive(false);

            //丢失目标之后，就不受最近敌人牵引，移动方向按照控制方向前进
            //transform.eulerAngles = new Vector3(0, 0, player.moveAngle);
            transform.rotation = Quaternion.Euler(0,0,player.moveAngle);
            //Debug.Log("按自己的方向移动");
        }
        
    }
}
