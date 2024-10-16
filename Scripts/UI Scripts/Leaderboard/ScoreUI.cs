using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    public RowUI rowUI;


    void Start()
    {
        
        var scores = LeaderboardManager.sortedList.ToArray();

        for (int i = 0; i < scores.Length; i++)
        {
            var row = Instantiate(rowUI, transform).GetComponent<RowUI>();
            row.rank.text = (i + 1).ToString();
            row.username.text = scores[i].username;
            row.score.text = scores[i].score.ToString();
        }


    }

   
}
