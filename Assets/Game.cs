using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    [SerializeField][Range(-1, 1)] float chance = -1; // if chance less 0 - random
    [SerializeField] Button[] buttons;
    [SerializeField] Image plChoiceHolder;
    [SerializeField] Image aiChoiceHolder;
    [SerializeField] Text scoreText;
    [SerializeField] Text resultText;
    [SerializeField] Button endButton;
    [SerializeField] GameObject endObj;
    [SerializeField] Sprite[] gameSpr;

    // Win table
    // -1 - Pair
    // 0 - Rock win
    // 1 - Scissors win
    // 2 - Paper win
    int[,] gameRelation = new int[,] {{-1, 0, 2}, {0, -1, 1}, {2, 1, -1}};
    int plScore = 0;
    int aiScore = 0;
    int aiChoice = 0;


    // Start is called before the first frame update
    void Start()
    {
        endObj.SetActive(false);
        scoreText.text = "0:0";

        endButton.onClick.AddListener(EndButtonCallback);

        for (int i = 0; i < buttons.Length; i++)
        {
            int ii = i;
            buttons[i].onClick.AddListener(() => StartCoroutine(Choice(ii)));
        }
    }

    void EndButtonCallback()
    {
        foreach (Button b in buttons)
            b.interactable = true;

        plChoiceHolder.sprite = null;
        aiChoiceHolder.sprite = null;

        endObj.SetActive(false);
    }

    IEnumerator Choice(int val)
    {
        foreach (Button b in buttons)
            b.interactable = false;

        plChoiceHolder.sprite = gameSpr[val];

        yield return new WaitForSeconds(0.1f);

        if(chance < 0)
            aiChoice = Random.Range(0, 3);
        else
        {
            aiChoice = GetInt(Random.value <= chance, val);
        }

        aiChoiceHolder.sprite = gameSpr[aiChoice];

        yield return new WaitForSeconds(2);

        int result = gameRelation[val, aiChoice];
        if(result == -1)
        {
            resultText.text = "Pair";
        }
        else if(result == val)
        {
            plScore++;
            resultText.text = "Win";
        }
        else
        {
            aiScore++;
            resultText.text = "Lose";
        }

        scoreText.text = string.Format("{0}:{1}", plScore, aiScore);
        endObj.SetActive(true);
    }

    int GetInt(bool aiWin, int enemy)
    {
        int result = -1;
        for (int i = 0; i < 3; i++)
        {
            int res = gameRelation[enemy, i];
            if(aiWin && res != -1 && res != enemy)
                result = i;
            else if(!aiWin && res == enemy)
                result = i;
        }
        return result;
    }
}
