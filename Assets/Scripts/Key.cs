using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] private GameObject lockGo;
    [SerializeField] private GameObject unclockGo;
    public void OpenDoor()
    {
        lockGo.SetActive(false);
        unclockGo.SetActive(true);
    }
}
