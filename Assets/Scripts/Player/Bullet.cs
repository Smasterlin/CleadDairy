using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject effect_bullet;
    [SerializeField]private float liveTime;
    [SerializeField]private float speed;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, liveTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.up*Time.deltaTime*speed,Space.World );
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            Debug.Log("子弹打到墙上了");
            Destroy(gameObject);
            GameObject go= Instantiate(effect_bullet,transform.position,Quaternion.identity);
            Destroy(go,1);
        }
    }
}
