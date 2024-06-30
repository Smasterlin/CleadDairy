using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using UnityEngine.SceneManagement;
using System.Text;
using System;
using UnityEngine.Networking;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Player player;
    public PlayerShooting playerShooting;
    public GameUIManager gameUIManager;
    public Vector2 inputValue;
    public float inputAngle;
    //json数据的读取
    public List<WEAPONPROPERTIES> weaponProtiesList;
    public string[] stories;
    //不同结局的走向
    public int gunLevel;
    public bool isAnthonyDead;
    public bool showEnd;

    [SerializeField] private int unlockLevel;
    public bool[] firstEnterLevels;
    private void Awake()
    {
        GlobalValue.UnLockLevel = unlockLevel;
        instance = this;
        DontDestroyOnLoad(gameObject);

        //存储数据
        #region 故事情节数据
        //        stories = new string[]
        //   {
        //"我叫安德鲁，是一名踹基蒂国特种兵\n一个月前因为优异的表现被上层派往一个城市去执行一项代号为“清洗”的任务\n我很感激上边对我的信任以及肯定，当然也很开心\n但这次任务因其特殊性限制只能由一个人来执行\n但我并没有任何畏惧感，一切为了我的国家和人民\n为了作后续报告与纪念，我决定把此次任务的经历写成日记",
        //"5月13日\n我已经执行任务一周了\n这座城市真的被这群异类毁的几乎看不出来城市原本的样子了\n尽管这样，这座城市真的很像我的家乡\n可恶，这群外形酷似我们人类的异类不知道是怎么产生的，无穷无尽\n我要把它们全部清理干净",
        //"6月7日\n我来这里将近一个月了\n尽管很累，但我为了我的国家和人民而战斗\n这些异类无穷无尽，很多，有些还是藏在地穴里\n我要时刻小心，因为上层有相关研究人员说过，这些异类会攻击撕咬啃食我们人类",
        //"7月1日\n我把这些异类称作“僵尸”\n我与上边联系过了，这座城市已经没有生者了，全部被这些僵尸杀死吃掉了\n对于没日没夜的“清洗”工作，我的身心十分疲惫，但一想起我做的事情是多么至高无上时\n一切的一切都是值得的",
        //"9月15日\n我已经不记得我“清洗”了多少垃圾\n我已经麻木了\n但今天发生了一件让我不得不记录的事情\n我遇到了一位被感染全身无力的生者名叫安东尼\n安东尼告诉我“清洗”任务的真相是\n这里所有的僵尸全是普通百姓转化的\n而且这座城市就是我的故乡德里克斯城\n多年前政府因为这里实验室的一个失误导致爆炸\n僵尸们是“无辜”的，他是听到实验室里的生还者临终前亲口说的\n所以后续他拒绝杀死它们，政府直接跟他断开了联系\n昨天他已经被感染了，因为僵尸都是人转化的，所以一些僵尸是有高等智慧的\n所以他请求我杀了他这位生前是特种兵的僵尸，解放他\n而我的选择是...",
        //"9月16日\n我听从了安东尼的话，他走前开心的笑了，一直不停感谢我，说我救赎了他...",
        //"我被政府强制调回\n因为上层政府研究出了病毒的转化药，可以将所有僵尸转化回人类\n转化后健康的人类可以回归正常过幸福的生活\n而我也因“清洗”任务授予了相当高的职位",
        //"9月16日\n我亲眼看到安东尼在深夜被转化成了僵尸\n他咬了我并且跑掉了，我要找到他解放他和我",
        //"我最终还是亲手杀死了安东尼，之后与政府断开了联络\n在准备自刎时，我还是退缩了\n尽管我作为踹基蒂国特种兵\n我还是害怕自己动手送自己上路\n今天遇到了另一位“清洗”者，并把我所知道的所有事情告诉了他\n包括被感染的事情\n他没有杀我，而是丢下武器，消失在了远方\n我的意识逐渐不太清晰了\n不知道还有多少位清洗者被送来这个地方\n又有谁能拯救我们..."
        //   }; 
        #endregion
        #region 各个武器属性数值的赋值(给结构体列表直接赋值)
        //weaponProtiesList = new()
        //{
        //    //Pistol
        //    new WEAPONPROPERTIES()
        //    {
        //        minInaccuracy = 5,
        //        attackCD = 0.2f,
        //        recoilForce = 4.0f,
        //        destabilization = 3,
        //        aimingDeSpeed = 15.0f,
        //        maxInaccuracy = 16,
        //        magazine = 12,
        //        totalBullects = 150,
        //        reload = 0.7f,
        //        weight = 1.0f,
        //        repulsion = 5
        //    },
        //    //AK
        //    new WEAPONPROPERTIES()
        //    {
        //        minInaccuracy = 4,
        //        attackCD = 0.12f,
        //        recoilForce = 2.0f,
        //        destabilization = 5,
        //        aimingDeSpeed = 9f,
        //        maxInaccuracy = 17,
        //        magazine = 30,
        //        totalBullects = 120,
        //        reload = 2f,
        //        weight = 3.0f,
        //        repulsion = 6
        //    },
        //    //MG
        //    new WEAPONPROPERTIES()
        //    {
        //        minInaccuracy = 8,
        //        attackCD = 0.1f,
        //        recoilForce = 1.4f,
        //        destabilization = 4,
        //        aimingDeSpeed = 8f,
        //        maxInaccuracy = 18,
        //        magazine = 100,
        //        totalBullects = 200,
        //        reload = 6f,
        //        weight = 5.0f,
        //        repulsion = 6
        //    },
        //    //Snipe
        //    new WEAPONPROPERTIES()
        //    {
        //        minInaccuracy = 2,
        //        attackCD = 1.2f,
        //        recoilForce = 12.0f,
        //        destabilization = 10,
        //        aimingDeSpeed = 6.5f,
        //        maxInaccuracy = 20,
        //        magazine = 8,
        //        totalBullects = 30,
        //        reload = 2.5f,
        //        weight = 6.0f,
        //        repulsion = 30
        //    },
        //    //Shotgun
        //    new WEAPONPROPERTIES()
        //    {
        //        minInaccuracy = 15,
        //        attackCD = 0.5f,
        //        recoilForce = 4.0f,
        //        destabilization = 3,
        //        aimingDeSpeed = 7f,
        //        maxInaccuracy = 20,
        //        magazine = 2,
        //        totalBullects = 40,
        //        reload = 1.2f,
        //        weight = 1.5f,
        //        repulsion = 7
        //    },
        //    //Crossbow
        //    new WEAPONPROPERTIES()
        //    {
        //        minInaccuracy = 3,
        //        attackCD = 0.5f,
        //        recoilForce = 3.0f,
        //        destabilization = 4,
        //        aimingDeSpeed = 6f,
        //        maxInaccuracy = 10,
        //        magazine = 20,
        //        totalBullects = 60,
        //        reload = 2.5f,
        //        weight = 2.0f,
        //        repulsion = 15
        //    }
        //}; 
        #endregion
        //SaveByJson();
        LoadGame();
        //读取数据
#if UNITY_STANDALONE_WIN
        LoadWeaponPropertiesJson();
        LoadStoriesJson();
#elif UNITY_ANDROID
        StartCoroutine(LoadWeaponPropertiesJson());
        StartCoroutine(LoadStoriesJson());
#endif
        //测试是否正确读取到了数据
        //for (int i = 0; i < weaponProtiesList.Count; i++)
        //{
        //    Debug.Log(weaponProtiesList[i].minInaccuracy);
        //}

        //测试是否存储布尔数组成功
        //SetBoolArray("1",true,false);
        //bool[] boolArr =GetBoolArray("1");
        //for (int i = 0; i < boolArr.Length; i++)
        //{
        //    Debug.Log(boolArr[i]);
        //}
    }

    private void LoadGame()
    {
        if (PlayerPrefs.HasKey("FirstEnter"))
        {
            firstEnterLevels = GetBoolArray("FirstEnter");
        }
        else
        {
            firstEnterLevels = new bool[] { true, true, true, true, true, true };
        }
        if (PlayerPrefs.HasKey("isAnthonyDead"))
        {
            isAnthonyDead = Convert.ToBoolean(PlayerPrefs.GetInt("isAnthonyDead"));
        }
        else
        {
            isAnthonyDead = false;
        }
    }

    /// <summary>
    /// 存储为json
    /// </summary>
    private void SaveByJson()
    {
        //把要存储的数据转成json字符串
        //要存储的数据存储在哪里    存储的json数据有哪些 
        //加了/，才能生成在根目录之下
        string filePath = Application.streamingAssetsPath + "/stories.json";
        string  saveJsonStr= JsonMapper.ToJson(stories);

        //把数据写入json文件
        //要写入哪里   写入什么东西  关闭写入
        StreamWriter sw = new StreamWriter(filePath);
        sw.Write(saveJsonStr);
        sw.Close();
    }
#if UNITY_STANDALONE_WIN
    /// <summary>
    /// 读取json数据
    /// </summary>
    private void LoadWeaponPropertiesJson()
    {
        weaponProtiesList = new();
        //只使用pc端的读取
        //把路径下的json文件，读取出来
        string filePath = Application.streamingAssetsPath + "/weaponProtiesList.json";
        if (File.Exists(filePath))
        {
            //把目录下的json文件，读成字符串
            StreamReader sr = new StreamReader(filePath);
            string jsonStr = sr.ReadToEnd();
            sr.Close();
            //把读出来的数据转成对象(具体的武器属性)
            weaponProtiesList = JsonMapper.ToObject<List<WEAPONPROPERTIES>>(jsonStr);
        }

        if (weaponProtiesList.Count == 0)
        {
            Debug.Log("读取json文件出错");
        }
    }
    /// <summary>
    /// 读取故事情节json
    /// </summary>
    private void LoadStoriesJson()
    {
        //只使用pc端的读取
        //把路径下的json文件，读取出来
        string filePath = Application.streamingAssetsPath + "/stories.json";
        if (File.Exists(filePath))
        {
            //把目录下的json文件，读成字符串
            StreamReader sr = new StreamReader(filePath);
            string jsonStr = sr.ReadToEnd();
            sr.Close();
            //把读出来的数据转成对象(具体的武器属性)
            stories = JsonMapper.ToObject<string[]>(jsonStr);
        }

        if (stories.Length == 0)
        {
            Debug.Log("读取json文件出错");
        }
    } 
#elif UNITY_ANDROID
    /// <summary>
    /// 读取json数据
    /// </summary>
    private IEnumerator LoadWeaponPropertiesJson()
    {
        weaponProtiesList = new();
        //把路径下的json文件，读取出来
        string filePath = Application.streamingAssetsPath + "/weaponProtiesList.json";

        ///WWW读取Json的方法 用于安卓端读取数据
        //WWW www = new(filePath);
        //while (!www.isDone)
        //{
        //    yield return null;
        //}
        //string jsonStr = www.text;

        ///UnityWebRequest读取Json的方法 用于安卓端读取数据
        UnityWebRequest unityWebRequest = UnityWebRequest.Get(filePath);
        yield return unityWebRequest.SendWebRequest();
        string jsonStr = unityWebRequest.downloadHandler.text;
        weaponProtiesList = JsonMapper.ToObject<List<WEAPONPROPERTIES>>(jsonStr);

        if (weaponProtiesList.Count == 0)
        {
            Debug.Log("读取json文件出错");
        }
    }
    /// <summary>
    /// 读取故事情节json
    /// </summary>
    private IEnumerator LoadStoriesJson()
    {
        //只使用pc端的读取
        //把路径下的json文件，读取出来
        string filePath = Application.streamingAssetsPath + "/stories.json";

        ///www读取JSON的方法 用于安卓端读取json文件
        //WWW www = new(filePath);
        //while (!www.isDone)
        //{
        //    yield return null;
        //}
        //string jsonStr = www.text;

        ///UnityWebRequest读取json的方法 用于安卓端读取json文件
        UnityWebRequest unityWebRequest = UnityWebRequest.Get(filePath);
        yield return unityWebRequest.SendWebRequest();
        string jsonStr = unityWebRequest.downloadHandler.text;
        stories = JsonMapper.ToObject<string[]>(jsonStr);

        if (stories.Length == 0)
        {
            Debug.Log("读取json文件出错");
        }
    }
#endif


    public void LoadMainScene()
    {
        Invoke("ReturnMainScenes",2);
    }
    private void ReturnMainScenes()
    {
        SceneManager.LoadScene(0);
    }

    public void SetBoolArray(string key,params bool[] boolArray)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < boolArray.Length-1; i++)
        {
            sb.Append(boolArray[i]).Append("|");
        }
        sb.Append(boolArray[boolArray.Length-1]);

        PlayerPrefs.SetString(key,sb.ToString());
    }

    public bool[] GetBoolArray(string key)
    {
        string[] stringArray = PlayerPrefs.GetString(key).Split("|");
        bool[] boolArray = new bool[stringArray.Length];
        for (int i = 0; i < stringArray.Length; i++)
        {
            boolArray[i] = Convert.ToBoolean(stringArray[i]);
        }

        return boolArray;
    }
}
