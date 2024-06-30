using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalValue : MonoBehaviour
{
    //玩家数据

    public static int Money
    {
        get { return PlayerPrefs.GetInt("Money", 0); }
        set { PlayerPrefs.SetInt("Money",value); }
    }
    public static int UnLockLevel
    {
        get { return PlayerPrefs.GetInt("UnLockLevel",0); }
        set { PlayerPrefs.SetInt("UnLockLevel",value); }
    }
    public static int SelectLevel
    {
        get { return PlayerPrefs.GetInt("SelectLevel", 1); }
        set { PlayerPrefs.SetInt("SelectLevel",value); }
    }
    public static int GunLevel
    {
        get { return PlayerPrefs.GetInt("GunLevel",1); }
        set { PlayerPrefs.SetInt("GunLevel",value); }
    }

    //系统设置
    public static float Volumn 
    {
        get { return PlayerPrefs.GetFloat("Volumn",1);}
        set { PlayerPrefs.SetFloat("Volumn",value); }
    }
    public static float JoyStickSize
    {
        get { return PlayerPrefs.GetFloat("JoyStickSize",0.3f); }
        set { PlayerPrefs.SetFloat("JoyStickSize",value); }
    }
}
