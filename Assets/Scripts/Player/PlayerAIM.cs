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
    private Enemy nearestEnemy;//�����������ĵ���

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
        //�򿪳��ھŲ�֮������߼�⣬�ھŲ�������Լ�
        layerMask = ~(1 << 9) & ~(1 << 2);
        //�����в㼶�����߼��
        //layerMask = ~(1<<0);
    }

    // Update is called once per frame
    void Update()
    {
        ChestNeastEnemy();
        //���������
        cameraTrans.position = transform.position - new Vector3(0,0,10);
    }
    /// <summary>
    /// ���߼������ĵ���
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
                if (!hit.collider.CompareTag("Wall"))//���߼��Ĳ���ǽ��
                {
                    //�����Һ�ÿ������֮��ľ���
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
    /// ��ʾ��������˱�־
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
                    Debug.Log("��⵽ǽ����");
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
    /// �������
    /// </summary>
    private void LookAtEnemy()
    {
        Vector3 moveDirection = nearestEnemy.transform.position - transform.position;
        if (moveDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveDirection.x, moveDirection.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, -transform.forward);//2D������z���Ƿ���
            //Debug.Log("��������˵�ת��Ƕ���"+angle);
            //Debug.Log("��������˵ĽǶ���"+ transform.eulerAngles);
        }  
    }
    private void ChestNeastEnemy()
    {
        if (player.enemyList.Count<=0)
        {
            return;
        }
        //ȷ�����������ĵ���
        CheckEnemy();

        if (nearestEnemy!=null)
        {
            //������ĵ���������ʾ����־
            ShowMark();
            //��ҿ�����ˣ� 2D��Ϸ�Ŀ�������
            LookAtEnemy();
           
        }
        else//��ʧĿ��
        {
            mark.SetActive(false);

            //��ʧĿ��֮�󣬾Ͳ����������ǣ�����ƶ������տ��Ʒ���ǰ��
            //transform.eulerAngles = new Vector3(0, 0, player.moveAngle);
            transform.rotation = Quaternion.Euler(0,0,player.moveAngle);
            //Debug.Log("���Լ��ķ����ƶ�");
        }
        
    }
}
