using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dan.Main;

public class LeaderBoardManager : MonoBehaviour
{
    public GameMaster MainHub;
    [SerializeField] private GameObject leaderboardParent; //turn leaderboard on and off
    [SerializeField] private List<TextMeshProUGUI> names; //list of names
    [SerializeField] private List<TextMeshProUGUI> scores; //list of scores

    private string publicLeaderboardKey = "aeaeeb7c9e5697fbfe63300c20b75b55c3b665f56a178ac9c93ba779065e054e";

    private void Start()
    {
        GetLeaderboard();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            if(leaderboardParent.activeInHierarchy)
            {
                leaderboardParent.SetActive(false);
            }
            else
            {
                leaderboardParent.SetActive(true);
            }
        }
    }

    public void GetLeaderboard()
    {
        LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, ((msg) =>
        {
            int loopLength = (msg.Length < names.Count) ? msg.Length : names.Count;
            for(int i = 0; i < loopLength; i++)
            {
                names[i].text = msg[i].Username;
                scores[i].text = msg[i].Score.ToString();
            }
        }));
    }

    public void SetLeaderboardEntry(string username, int score)
    {
        LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, username, score, ((msg) =>
        {
            GetLeaderboard();
        }));

        LeaderboardCreator.ResetPlayer();
    }
}
