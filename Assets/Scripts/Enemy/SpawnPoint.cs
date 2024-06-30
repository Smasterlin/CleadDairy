using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField]private float delayTimer;
    [SerializeField] private float spawnTimer;
    [SerializeField] private GameObject enemy;
    [SerializeField]private int limit;
    [SerializeField]private bool startSpawn;
    // Start is called before the first frame update
    void Start()
    {
        delayTimer = spawnTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (startSpawn==true)
        {
            if (delayTimer>0)
            {
                delayTimer -= Time.deltaTime;
            }
            else
            {
                if (limit > 0)
                {
                    Debug.Log("产生敌人了");
                    Instantiate(enemy, transform.position + new Vector3(Random.Range(-0.1f, 0.1f),
                        Random.Range(-0.1f, 0.1f), 0), transform.rotation);
                    delayTimer = spawnTimer;
                    limit -= 1;
                }
                else
                {
                    Destroy(gameObject);
                }
            }
            
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (startSpawn==false)
            {
                startSpawn = true;
            }
        }
    }

}
