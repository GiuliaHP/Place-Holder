using System;
using UnityEngine;

public class PersistentManager : MonoBehaviour
{
    public static PersistentManager Instance { get; private set; }
    public bool triggerObjectInOtherScene = false;
    public bool isLapinGone = false;
    public GameObject lapin;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (isLapinGone)
        {
            lapin.SetActive(false);
        }
    }
}