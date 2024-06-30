using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Finish : MonoBehaviour
{
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.player;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (GameManager.instance.player.curKills==GameManager.instance.player.kills)
            {
                GlobalValue.UnLockLevel += 1;
                SceneManager.LoadScene(0);
            }
        }
    }
}
