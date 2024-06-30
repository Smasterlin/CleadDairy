using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private Player player;
    private Enemy nearestEnemy;
    private float distance;

    private float minDistance;
    private float maxDistance;

    private LayerMask layerMask;
    private RaycastHit2D hit;

    private float curAttackCD;
    [SerializeField] private float attackCD;

    [Header("***�ӵ���Դ***")]
    [SerializeField] private GameObject bulletsGo;
    [SerializeField] private GameObject[] shootPoint;
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private int bullets;
    [SerializeField] GameObject mark;

    private int turretLevel;
    [SerializeField] private float inacuuracy;

    [SerializeField]private bool isEnemyTurret;
    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.player;
        minDistance = maxDistance = 7;
        mark.SetActive(false);
        if (isEnemyTurret==true)
        {
            layerMask = ~(1 << 10) & ~(1 << 2);
        }
        else
        {
            layerMask = ~(1 << 9) & ~(1 << 2);
        }
        
        turretLevel = GlobalValue.GunLevel / 2;
        if (turretLevel==0)
        {
            turretLevel = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (curAttackCD>0)
        {
            curAttackCD -= Time.deltaTime;
        }
        if (isEnemyTurret==true)//�����ҵ���̨
        {
            CheckPlayer();
        }
        else
        {
            ChestNeastEnemy();
        }
    }
    #region �����ҵ���̨
    /// <summary>
    /// ���߼���������
    /// </summary>
    private void CheckPlayer()
    {
        if (player==null)
        {
            return;
        }
        hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position
                , 10, layerMask);
        //Debug.DrawRay(transform.position, player.enemyList[i].transform.position - transform.position, Color.red, 20);

        if (hit.collider != null)//���߼���м�⵽����
        {
            LookAtPlayer();
            AttackPlayer();
        }
    }
    /// <summary>
    /// �������
    /// </summary>
    private void LookAtPlayer()
    {
        Vector3 moveDirection = player.transform.position - transform.position;
        if (moveDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveDirection.x, moveDirection.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, -Vector3.forward);//2D������z���Ƿ���
        }
    }
    private void AttackPlayer()
    {
        if (curAttackCD <= 0 && bullets > 0)
        {
            AudioSourceManager.instance.PlaySound(shootClip);
            bullets -= 1;

            Instantiate(bulletsGo, shootPoint[0].transform.position, Quaternion.Euler(0, 0, transform.rotation.
                eulerAngles.z + Random.Range(-inacuuracy, inacuuracy)));
            //if (turretLevel == 3)
            //{
            //    Instantiate(bulletsGo, shootPoint[1].transform.position, Quaternion.Euler(0, 0, transform.rotation.
            //        eulerAngles.z + Random.Range(-inacuuracy, inacuuracy)));
            //}

            curAttackCD = attackCD;
        }
    }
    #endregion


    #region ��Ե��˵�����
    /// <summary>
    /// ���߼����������
    /// </summary>
    private void CheckEnemy()
    {
        //ȷ�����������ĵ���
        for (int i = 0; i < player.enemyList.Count; i++)
        {
            hit = Physics2D.Raycast(transform.position, player.enemyList[i].transform.position - transform.position
                , 4, layerMask);
            //Debug.DrawRay(transform.position, player.enemyList[i].transform.position - transform.position, Color.red, 20);

            if (hit.collider != null)//���߼���м�⵽����
            {
                if (!hit.collider.CompareTag("Wall"))//���߼��Ĳ���ǽ��
                {
                    //�����Һ�ÿ������֮��ľ���
                    distance = Vector3.Distance(transform.position, player.enemyList[i].transform.position);
                    if (distance < maxDistance && distance < minDistance)
                    {
                        nearestEnemy = player.enemyList[i];
                    }
                }
            }
        }
    }
    /// <summary>
    /// �����������
    /// </summary>
    private void ChestNeastEnemy()
    {
        CheckEnemy();
        //������ĵ���������ʾ����־
        if (nearestEnemy != null)
        {
            //��ʾ��־
            ShowMark();
            //��ҿ�����ˣ� 2D��Ϸ�Ŀ�������
            LookAtEnemy();
            //�Զ���������
            AttackEnemy();
        }
        else//��ʧĿ��
        {
            mark.SetActive(false);

            //��ʧĿ��֮�󣬾Ͳ����������ǣ�����ƶ������տ��Ʒ���ǰ��
            //transform.eulerAngles = new Vector3(0, 0, player.moveAngle);
            //transform.rotation = Quaternion.Euler(0, 0, player.moveAngle);
        }
    }
    private void AttackEnemy()
    {
        if (curAttackCD <= 0 && bullets > 0)
        {
            AudioSourceManager.instance.PlaySound(shootClip);
            bullets -= 1;

            Instantiate(bulletsGo, shootPoint[0].transform.position, Quaternion.Euler(0, 0, transform.rotation.
                eulerAngles.z + Random.Range(-inacuuracy, inacuuracy)));
            if (turretLevel == 3)
            {
                Instantiate(bulletsGo, shootPoint[1].transform.position, Quaternion.Euler(0, 0, transform.rotation.
                    eulerAngles.z + Random.Range(-inacuuracy, inacuuracy)));
            }

            curAttackCD = attackCD;
        }
    }
    private void LookAtEnemy()
    {
        Vector3 moveDirection = nearestEnemy.transform.position - transform.position;
        if (moveDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveDirection.x, moveDirection.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, -Vector3.forward);//2D������z���Ƿ���
        }
    }
    /// <summary>
    /// �ڵ���������ʾ��־
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
        if (isEnemyTurret == false)
        {
            if (nearestEnemy != null)
            {
                mark.transform.position = nearestEnemy.transform.position;
                if (hit.collider!=null)
                {
                    if (hit.collider.CompareTag("Wall"))
                    {
                        minDistance = maxDistance = 10;
                        nearestEnemy = null;
                    }
                }
            }
            else
            {
                minDistance = maxDistance = 10;
                nearestEnemy = null;
            }
        }
    }
    #endregion
}
