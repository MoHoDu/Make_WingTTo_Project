using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

[Inspectable]
public class Player : MonoBehaviour
{
    public GameObject[] characterList;

    bool saveData = false;

    [Inspectable]
    public bool isGhostDie = false;

    [Inspectable]
    public int skillIndex = 5;
    [Inspectable]
    public float addMoveSpeed = 0;
    [Inspectable]
    public bool isRocked = false;

    public Vector3 startPos;
    public TextMeshProUGUI scoreT;

    public TextMeshProUGUI goalText;
    public GameObject highscoreMap;
    public TextMeshProUGUI highscoreMeter;

    public GameObject[] skills;
    public Color skillOffColor;

    public GameObject leaderBoardPlayerPre;
    public GameObject playerDash;

    public GameObject[] mobilePanels;
    public int LTouchCount = 0;
    [Inspectable]
    public bool isLTouch = false;
    public bool isRTouch = false;

    public GameObject noaBall;

    void Awake()
    {
        for (int i = 0; i < 2; i++)
        {
            if (i == GameManager.instance.selectChar)
            {
                transform.GetChild(i).gameObject.SetActive(true);
                playerDash = transform.GetChild(i).GetChild(0).gameObject;
                playerDash.SetActive(false);
            }
            else
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        GameObject.Find("HighScore").transform.position = new Vector3((float)GameManager.instance.bestScore + 8.94f, 2.13f, 1.0f);
        if (GameManager.instance.leaderBoardPlayerCount != 0)
        {
            for (int i = 0; i < GameManager.instance.leaderBoardPlayerCount; i++)
            {
                GameObject playerPre = Instantiate(leaderBoardPlayerPre);
                playerPre.transform.position = new Vector3(GameManager.instance.leaderBoardRecords[i] + 8.94f, 2.13f, 0.9f);
                playerPre.transform.Find("Canvas").GetComponentInChildren<TextMeshProUGUI>().text = GameManager.instance.leaderBoardNames[i];
                if (i != 0)
                {
                    playerPre.transform.Find("flag").gameObject.SetActive(false);
                    playerPre.transform.GetChild(0).Find("BestT").gameObject.SetActive(false);
                }
            }
        }
        PlayfabManager.instance.GetStat();
        startPos = new Vector3(this.transform.position.x, 0, 0);
        GameManager.instance.score = 0;
        // transform.Find("Name").GetComponent<TextMeshProUGUI>().text = GameManager.instance.nick;

        highscoreMeter.text = GameManager.instance.bestScore.ToString();
        // GameObject skillParent = GameObject.Find("SkillPanel");
        // for (int i = 0; i < skillParent.transform.childCount; i++)
        // {
        //     skills[i] = skillParent.transform.GetChild(i).gameObject;
        // }

        skillOffColor = skills[0].GetComponent<Image>().color;
        skillIndex = 3;
    }

    public void SkillOn()
    {
        if (skillIndex == 3)
        {
            return;
        }
        int skillNum = characterList[GameManager.instance.selectChar].GetComponent<SkillIndex>().skills[skillIndex].skillN;
        SkillManager.instance.UseSkill(skillNum);
        skillIndex = 3;
    }

    private void FixedUpdate()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Touch[] tposes = Input.touches;
            foreach (Touch touch in tposes)
            {
                if (touch.position.x <= Screen.width / 2)
                {
                    LTouchCount = 1;
                }
                else if (touch.position.x > Screen.width / 2 && !isRTouch)
                {
                    SkillOn();
                    isRTouch = true;
                }
            }
            if (LTouchCount != 0)
                isLTouch = true;
            else
                isLTouch = false;
            LTouchCount = 0;
            isRTouch = false;
        }
        else
        {
            if (Input.GetMouseButton(1))
            {
                SkillOn();
            }
        }
    }

    void Update()
    {
        TextMeshProUGUI skillT = GameObject.Find("bkskilltext1").transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        if (skillIndex == 3)
            skillT.text = "<color=yellow>None :</color> Get diamonds to use skills";
        else
            skillT.text = characterList[GameManager.instance.selectChar].GetComponent<SkillIndex>().skills[skillIndex].skillText;

        if (GameManager.instance.bestScore.ToString() != GameManager.instance.bestScore.ToString())
            highscoreMeter.text = GameManager.instance.bestScore.ToString();

        int limitMeter = (GameManager.instance.bestScore - GameManager.instance.score);
        // Debug.Log($"limit {limitMeter} / score {GameManager.instance.score} / bestScore{GameManager.instance.bestScore}");
        goalText.text = $"<size=50><color=red>{limitMeter} m</color></size> to the highest record";

        for (int i = 0; i < skills.Length; i++)
        {
            if (skillIndex == i)
                skills[i].GetComponent<Image>().color = Color.white;
            else
                skills[i].GetComponent<Image>().color = skillOffColor;
        }

        if (limitMeter <= 0)
        {
            if (highscoreMap.activeSelf)
                highscoreMap.SetActive(false);
            goalText.text = $"<size=50><color=red>{GameManager.instance.score} m</color></size> : New record!!";
        }
        else if (limitMeter <= 300)
        {
            highscoreMap.GetComponent<RectTransform>().anchoredPosition = new Vector2(limitMeter, 0);
        }

        int scoreN = Mathf.FloorToInt(Vector3.Distance(startPos, new Vector3(this.transform.position.x, 0, 0)));
        scoreT.text = scoreN.ToString();
        GameManager.instance.score = scoreN;

        // GameObject.Find("Score").GetComponent<TextMeshProUGUI>().text = GameManager.instance.score.ToString();

        if (GameManager.instance.score > GameManager.instance.bestScore)
        {
            GameObject.Find("best").GetComponent<TextMeshProUGUI>().text = "Best Score";
        }
        else
        {
            GameObject.Find("best").GetComponent<TextMeshProUGUI>().text = "My Score";
        }

        if (saveData)
            return;

        if (isGhostDie)
        {
            if (GameManager.instance.bestPlayerScore < GameManager.instance.score)
            {
                GameManager.instance.bestPlayerScore = GameManager.instance.score;
            }
            GameManager.instance.isResult = true;
            PlayfabManager.instance.SetStat();
            // GameManager.instance.score = 0;
            saveData = true;
        }
    }
}
