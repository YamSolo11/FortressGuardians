using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public GameObject tower;
    public Turret towerSets;
    public GameObject nodeUI;

    public int currency = 400;

    public static GameMaster Instance { get; private set; }

    void Awake()
    {
        if (Instance == null) { Instance = this; } else if (Instance != this) { Destroy(this); }
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(tower != null)
        {
            nodeUI.SetActive(true);
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                tower = null;
                nodeUI.SetActive(false);
            }
        }
    }

    public void upgrade()
    {
        towerSets.range = towerSets.range + 5f;
        towerSets.fireRate = towerSets.fireRate + .5f;
    }

    public void sell()
    {
        Destroy(tower);
        //tower = null;
    }
}
