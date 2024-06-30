using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instruction : MonoBehaviour
{
    private float viewDistance;
    private TextMesh textMesh;
    private float distance;
    private Transform playerTrans;
    // Start is called before the first frame update
    void Start()
    {
        textMesh = GetComponent<TextMesh>();
        playerTrans = GameManager.instance.player.transform;
        viewDistance = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTrans==null)//��Ϊ�Ƿ���update����ģ�����Ҫ������ȫУ�飬���ⱨ��
        {
            return;
        }
        //��ҿ�����ʱ����ʾ����˵��
        distance = Vector3.Distance(transform.position,playerTrans.position);
        if (distance<viewDistance)
        {
            textMesh.characterSize = 0.2f * (1f-distance/viewDistance);
        }
        else
        {
            textMesh.characterSize = 0;
        }
    }
}
