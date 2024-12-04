using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LivesCounter : MonoBehaviour
{
    public TMP_Text scoreText;

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Lives: " + GameMaster.Instance.lives.ToString();
    }
}
