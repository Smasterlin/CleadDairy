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
    //json���ݵĶ�ȡ
    public List<WEAPONPROPERTIES> weaponProtiesList;
    public string[] stories;
    //��ͬ��ֵ�����
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

        //�洢����
        #region �����������
        //        stories = new string[]
        //   {
        //"�ҽа���³����һ���߻��ٹ����ֱ�\nһ����ǰ��Ϊ����ı��ֱ��ϲ�����һ������ȥִ��һ�����Ϊ����ϴ��������\n�Һܸм��ϱ߶��ҵ������Լ��϶�����ȻҲ�ܿ���\n�����������������������ֻ����һ������ִ��\n���Ҳ�û���κ�η��У�һ��Ϊ���ҵĹ��Һ�����\nΪ�����������������Ҿ����Ѵ˴�����ľ���д���ռ�",
        //"5��13��\n���Ѿ�ִ������һ����\n����������ı���Ⱥ����ٵļ���������������ԭ����������\n��������������������ĺ����ҵļ���\n�ɶ���Ⱥ���ο���������������಻֪������ô�����ģ������޾�\n��Ҫ������ȫ������ɾ�",
        //"6��7��\n�������ｫ��һ������\n���ܺ��ۣ�����Ϊ���ҵĹ��Һ������ս��\n��Щ���������޾����ܶ࣬��Щ���ǲ��ڵ�Ѩ��\n��Ҫʱ��С�ģ���Ϊ�ϲ�������о���Ա˵������Щ����ṥ��˺ҧ��ʳ��������",
        //"7��1��\n�Ұ���Щ�����������ʬ��\n�����ϱ���ϵ���ˣ����������Ѿ�û�������ˣ�ȫ������Щ��ʬɱ���Ե���\n����û��ûҹ�ġ���ϴ���������ҵ�����ʮ��ƣ������һ���������������Ƕ�ô��������ʱ\nһ�е�һ�ж���ֵ�õ�",
        //"9��15��\n���Ѿ����ǵ��ҡ���ϴ���˶�������\n���Ѿ���ľ��\n�����췢����һ�����Ҳ��ò���¼������\n��������һλ����Ⱦȫ���������������а�����\n����������ҡ���ϴ�������������\n�������еĽ�ʬȫ����ͨ����ת����\n�����������о����ҵĹ�������˹��\n����ǰ������Ϊ����ʵ���ҵ�һ��ʧ���±�ը\n��ʬ���ǡ��޹����ģ���������ʵ�����������������ǰ�׿�˵��\n���Ժ������ܾ�ɱ�����ǣ�����ֱ�Ӹ����Ͽ�����ϵ\n�������Ѿ�����Ⱦ�ˣ���Ϊ��ʬ������ת���ģ�����һЩ��ʬ���иߵ��ǻ۵�\n������������ɱ������λ��ǰ�����ֱ��Ľ�ʬ�������\n���ҵ�ѡ����...",
        //"9��16��\n�������˰�����Ļ�������ǰ���ĵ�Ц�ˣ�һֱ��ͣ��л�ң�˵�Ҿ�������...",
        //"�ұ�����ǿ�Ƶ���\n��Ϊ�ϲ������о����˲�����ת��ҩ�����Խ����н�ʬת��������\nת���󽡿���������Իع��������Ҹ�������\n����Ҳ����ϴ�������������൱�ߵ�ְλ",
        //"9��16��\n�����ۿ�������������ҹ��ת�����˽�ʬ\n��ҧ���Ҳ����ܵ��ˣ���Ҫ�ҵ������������",
        //"�����ջ�������ɱ���˰����ᣬ֮���������Ͽ�������\n��׼������ʱ���һ���������\n��������Ϊ�߻��ٹ����ֱ�\n�һ��Ǻ����Լ��������Լ���·\n������������һλ����ϴ���ߣ���������֪�������������������\n��������Ⱦ������\n��û��ɱ�ң����Ƕ�����������ʧ����Զ��\n�ҵ���ʶ�𽥲�̫������\n��֪�����ж���λ��ϴ�߱���������ط�\n����˭����������..."
        //   }; 
        #endregion
        #region ��������������ֵ�ĸ�ֵ(���ṹ���б�ֱ�Ӹ�ֵ)
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
        //��ȡ����
#if UNITY_STANDALONE_WIN
        LoadWeaponPropertiesJson();
        LoadStoriesJson();
#elif UNITY_ANDROID
        StartCoroutine(LoadWeaponPropertiesJson());
        StartCoroutine(LoadStoriesJson());
#endif
        //�����Ƿ���ȷ��ȡ��������
        //for (int i = 0; i < weaponProtiesList.Count; i++)
        //{
        //    Debug.Log(weaponProtiesList[i].minInaccuracy);
        //}

        //�����Ƿ�洢��������ɹ�
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
    /// �洢Ϊjson
    /// </summary>
    private void SaveByJson()
    {
        //��Ҫ�洢������ת��json�ַ���
        //Ҫ�洢�����ݴ洢������    �洢��json��������Щ 
        //����/�����������ڸ�Ŀ¼֮��
        string filePath = Application.streamingAssetsPath + "/stories.json";
        string  saveJsonStr= JsonMapper.ToJson(stories);

        //������д��json�ļ�
        //Ҫд������   д��ʲô����  �ر�д��
        StreamWriter sw = new StreamWriter(filePath);
        sw.Write(saveJsonStr);
        sw.Close();
    }
#if UNITY_STANDALONE_WIN
    /// <summary>
    /// ��ȡjson����
    /// </summary>
    private void LoadWeaponPropertiesJson()
    {
        weaponProtiesList = new();
        //ֻʹ��pc�˵Ķ�ȡ
        //��·���µ�json�ļ�����ȡ����
        string filePath = Application.streamingAssetsPath + "/weaponProtiesList.json";
        if (File.Exists(filePath))
        {
            //��Ŀ¼�µ�json�ļ��������ַ���
            StreamReader sr = new StreamReader(filePath);
            string jsonStr = sr.ReadToEnd();
            sr.Close();
            //�Ѷ�����������ת�ɶ���(�������������)
            weaponProtiesList = JsonMapper.ToObject<List<WEAPONPROPERTIES>>(jsonStr);
        }

        if (weaponProtiesList.Count == 0)
        {
            Debug.Log("��ȡjson�ļ�����");
        }
    }
    /// <summary>
    /// ��ȡ�������json
    /// </summary>
    private void LoadStoriesJson()
    {
        //ֻʹ��pc�˵Ķ�ȡ
        //��·���µ�json�ļ�����ȡ����
        string filePath = Application.streamingAssetsPath + "/stories.json";
        if (File.Exists(filePath))
        {
            //��Ŀ¼�µ�json�ļ��������ַ���
            StreamReader sr = new StreamReader(filePath);
            string jsonStr = sr.ReadToEnd();
            sr.Close();
            //�Ѷ�����������ת�ɶ���(�������������)
            stories = JsonMapper.ToObject<string[]>(jsonStr);
        }

        if (stories.Length == 0)
        {
            Debug.Log("��ȡjson�ļ�����");
        }
    } 
#elif UNITY_ANDROID
    /// <summary>
    /// ��ȡjson����
    /// </summary>
    private IEnumerator LoadWeaponPropertiesJson()
    {
        weaponProtiesList = new();
        //��·���µ�json�ļ�����ȡ����
        string filePath = Application.streamingAssetsPath + "/weaponProtiesList.json";

        ///WWW��ȡJson�ķ��� ���ڰ�׿�˶�ȡ����
        //WWW www = new(filePath);
        //while (!www.isDone)
        //{
        //    yield return null;
        //}
        //string jsonStr = www.text;

        ///UnityWebRequest��ȡJson�ķ��� ���ڰ�׿�˶�ȡ����
        UnityWebRequest unityWebRequest = UnityWebRequest.Get(filePath);
        yield return unityWebRequest.SendWebRequest();
        string jsonStr = unityWebRequest.downloadHandler.text;
        weaponProtiesList = JsonMapper.ToObject<List<WEAPONPROPERTIES>>(jsonStr);

        if (weaponProtiesList.Count == 0)
        {
            Debug.Log("��ȡjson�ļ�����");
        }
    }
    /// <summary>
    /// ��ȡ�������json
    /// </summary>
    private IEnumerator LoadStoriesJson()
    {
        //ֻʹ��pc�˵Ķ�ȡ
        //��·���µ�json�ļ�����ȡ����
        string filePath = Application.streamingAssetsPath + "/stories.json";

        ///www��ȡJSON�ķ��� ���ڰ�׿�˶�ȡjson�ļ�
        //WWW www = new(filePath);
        //while (!www.isDone)
        //{
        //    yield return null;
        //}
        //string jsonStr = www.text;

        ///UnityWebRequest��ȡjson�ķ��� ���ڰ�׿�˶�ȡjson�ļ�
        UnityWebRequest unityWebRequest = UnityWebRequest.Get(filePath);
        yield return unityWebRequest.SendWebRequest();
        string jsonStr = unityWebRequest.downloadHandler.text;
        stories = JsonMapper.ToObject<string[]>(jsonStr);

        if (stories.Length == 0)
        {
            Debug.Log("��ȡjson�ļ�����");
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
