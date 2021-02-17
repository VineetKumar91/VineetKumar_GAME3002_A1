using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class WinnerScript : MonoBehaviour
{
    static public int ending = 0;
    private SoundManager SoundManagerReference;
    [SerializeField]
    TextMeshProUGUI winnerText;
    // Start is called before the first frame update
    void Start()
    {
        SoundManagerReference = GameObject.FindObjectOfType<SoundManager>();
        Cursor.visible = false;
        switch (ending)
        {
            case 0:
                winnerText.text = "Player 1 Wins!";         // Player 1 Winner
                break;


            case 1:
                winnerText.text = "Player 2 Wins!";         // Player 2 Winner
                break;


            case 2:
                winnerText.text = "Its a Tie!";             // Tie
                break;
        }
        SoundManagerReference.PlayEnding();
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(QuitGame());
    }

    // Quit Game after a delay
    IEnumerator QuitGame()
    {
        yield return new WaitForSeconds(3);
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
