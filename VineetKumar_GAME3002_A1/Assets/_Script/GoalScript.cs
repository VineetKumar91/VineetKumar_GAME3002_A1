using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoalScript : MonoBehaviour
{
    public TextMeshProUGUI Player1_TMP;
    public TextMeshProUGUI Player2_TMP;

    public GameObject[] Player1_Score;
    public GameObject[] Player2_Score;
    private GetAngle ArrowReference;

    public int Score1 = 0;
    public int Score2 = 0;

    public bool m_bPlayer1Turn = true;

    public int turnCount = 0;

    private int player1Index = 0;
    private int player2Index = 0;

    public bool ready = true;

    private SoundManager SoundManagerReference;

    //private GoalPrefabScript GoalPfScriptRef;
    // Start is called before the first frame update
    void Start()
    {
        SoundManagerReference = GameObject.FindObjectOfType<SoundManager>();
        ArrowReference = GameObject.FindGameObjectWithTag("Arrow").GetComponent<GetAngle>();
        //GoalPfScriptRef = GameObject.FindGameObjectWithTag("Notification").GetComponent<GoalPrefabScript>();
        Player1_TMP.outlineWidth = 0.3f;
        Player2_TMP.outlineWidth = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reset();
        }

        if (turnCount >= 10)
        {
            StartCoroutine(ChangeSceneRoutine());

        }
    }

    // On Collision Enter on the Goal   - Shoots and Scores
    void OnCollisionEnter(Collision ballCollision)
    {
        if (ready)
        {
            ready = false;
            //Debug.Log("Goal!!!");     // Works
            GoalTextAdjustScriptForPrefab.Create(true);

            if (m_bPlayer1Turn)
            {
                Score1++;
                Player1_Score[player1Index].GetComponent<Image>().color = Color.green;
                player1Index++;
                StartCoroutine(ChangeActivePlayer(m_bPlayer1Turn));
                SoundManagerReference.PlayScored();
            }
            else
            {
                Score2++;
                Player2_Score[player2Index].GetComponent<Image>().color = Color.green;
                player2Index++;
                StartCoroutine(ChangeActivePlayer(m_bPlayer1Turn));
                SoundManagerReference.PlayScored();
            }

            turnCount++;
            m_bPlayer1Turn = !m_bPlayer1Turn;
            StartCoroutine(ResetCallMethod());
        }
       
    }

    // Shoots but missses
    public void NoGoal()
    {
        if (ready)
        {
            ready = false;
            if (m_bPlayer1Turn)
            {
                Player1_Score[player1Index].GetComponent<Image>().color = Color.red;
                player1Index++;
                StartCoroutine(ChangeActivePlayer(m_bPlayer1Turn));
                SoundManagerReference.PlayMissed();
            }
            else
            {
                Player2_Score[player2Index].GetComponent<Image>().color = Color.red;
                player2Index++;
                StartCoroutine(ChangeActivePlayer(m_bPlayer1Turn));
                SoundManagerReference.PlayMissed();
            }
            turnCount++;
            m_bPlayer1Turn = !m_bPlayer1Turn;

            GoalTextAdjustScriptForPrefab.Create(false);
        }
        
        //Debug.Log("No Goal !!!");
    }

    void Reset()
    {
        //Debug.Log("Player Turns and Score Reset");

        Player1_TMP.outlineWidth = 0.3f;
        Player2_TMP.outlineWidth = 0.0f;
        
        Score1 = 0;
        Score2 = 0;

        m_bPlayer1Turn = true;

        turnCount = 0;

        player1Index = 0;
        player2Index = 0;

        for (int i = 0; i < 5; i++)
        {
            Player1_Score[i].GetComponent<Image>().color = Color.gray;
            Player2_Score[i].GetComponent<Image>().color = Color.gray;
        }
    }

    // Need Delay before actually resetting ball position
    IEnumerator ResetCallMethod()
    {
        //Debug.Log("Before Reset");
        yield return new WaitForSeconds(2);
        ArrowReference.Reset();
        setReady();
        //Debug.Log("After Waiting 2 Seconds");
    }

    // Ready to go again
    public void setReady()
    {
        ready = true;
    }

    // Highlight of Active Player of TMP
    IEnumerator ChangeActivePlayer(bool activePlayer)
    {
        yield return new WaitForSeconds(2);
        if (activePlayer)
        {
            Player1_TMP.outlineWidth = 0.0f;
            Player2_TMP.outlineWidth = 0.3f;
        }
        else
        {
            Player2_TMP.outlineWidth = 0.0f;
            Player1_TMP.outlineWidth = 0.3f;
        }
    }

    // Scene Change and pass value to determine who has won or tie
    IEnumerator ChangeSceneRoutine()
    {
        yield return new WaitForSeconds(2);
        if (Score1 > Score2)
        {
            WinnerScript.ending = 0;
        }
        else if (Score2 > Score1)
        {
            WinnerScript.ending = 1;
        }
        else
        {
            WinnerScript.ending = 2;
        }

        SceneManager.LoadScene("ResultScene");
    }
}

