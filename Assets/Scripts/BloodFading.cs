using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodFading : MonoBehaviour
{
    [SerializeField]private SpriteRenderer spriteRender;
    [SerializeField]private float waitTime;
    [SerializeField]private float speed;
    private bool startFade=false;
    public bool isEnemyBlood;
    [SerializeField] private AudioClip playerDieClip;
    [SerializeField] private AudioClip enemyDieClip;
    //[SerializeField] private AudioClip explosion;
    // Start is called before the first frame update
    void Start()
    {
        if (isEnemyBlood == true)
        {
            AudioSourceManager.instance.PlaySound(enemyDieClip);
        }
        else
        {
            AudioSourceManager.instance.PlaySound(playerDieClip);
        }
        //spriteRender =transform.Find("Blood").GetComponent<SpriteRenderer>();
        Invoke("Fading",waitTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (startFade==true)
        {
            Color color = spriteRender.color;
            color.a -=speed*Time.deltaTime;
            color.a = Mathf.Clamp(color.a,0,1);
            spriteRender.color = color;
            if (color.a<=0)
            {
                Destroy(gameObject);
            }
        }
    }
    private void Fading()
    {
        startFade = true;
    }
}
