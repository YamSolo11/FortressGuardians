using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LivesTracker : MonoBehaviour
{
    [SerializeField]
    private GameMaster LivesHub;

    public TextMeshProUGUI livesText;

    // Update is called once per frame
    void Update()
    {
        livesText.text = "Lives: " + LivesHub.lives;
    }
}