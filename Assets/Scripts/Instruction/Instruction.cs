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
        if (playerTrans==null)//因为是放在update里面的，所以要做个安全校验，避免报空
        {
            return;
        }
        //玩家靠近的时候，显示解释说明
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
