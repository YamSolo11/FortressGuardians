using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardDisplay : MonoBehaviour
{
    public GameObject leaderboardUI;
    private bool leaderboardVisible = false;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowBoard()
    {
        if (leaderboardVisible == false)
        {
            leaderboardUI.SetActive(true);
            leaderboardVisible = true;
        }
        else
        {
            leaderboardUI.SetActive(false);
            leaderboardVisible = false;
        }
    }
}
