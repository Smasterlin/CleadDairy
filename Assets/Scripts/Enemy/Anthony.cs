using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anthony : Enemy
{
    protected override void Update()
    {
        base.Update();
    }

    protected override void Die()
    {
        if (HP<=0)
        {
            GameManager.instance.isAnthonyDead = true;
            PlayerPrefs.SetInt("isAnthonyDead", 1);
        }
        base.Die();
    }
}
