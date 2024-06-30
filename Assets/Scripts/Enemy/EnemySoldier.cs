using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoldier : Enemy
{
    [Header("***资源***")]
    [SerializeField] protected AudioClip shootClip;
    [SerializeField] protected GameObject bulletGo;

    //引用
    [SerializeField] protected GameObject shootPoint;

    //属性
    [SerializeField] protected float attackCD;
    [SerializeField]protected float curAttackCD;
    [SerializeField] protected int magzine;
    [SerializeField]protected int curMagzine;
    [SerializeField] protected float reload;
    protected float curReload;
    protected float curInaccuracy;
    [SerializeField] protected float inaccuracy;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        curInaccuracy = inaccuracy;
    }

    // Update is called once per frame
    protected override void Update()
    {
        CalculateCD();
        base.Update();
    }
    protected void CalculateCD()
    {
        curAttackCD -= Time.deltaTime;
        curReload -= Time.deltaTime;
        if (curReload <= 0 && curMagzine <= 0)
        {
            curMagzine = magzine;
        }
    }
    protected override void LookAtPlayerAndAttack()
    {
        base.LookAtPlayerAndAttack();
        if (curAttackCD <= 0 && curMagzine > 0)
        {
            AudioSourceManager.instance.PlaySound(shootClip);
            Instantiate(bulletGo, shootPoint.transform.position, Quaternion.Euler(0, 0,
                shootPoint.transform.rotation.eulerAngles.z + Random.Range(-inaccuracy, inaccuracy)));
            curAttackCD = attackCD;
            curMagzine -= 1;
            if (curMagzine <= 0)
            {
                curReload = reload;
            }
        }
    }
}
