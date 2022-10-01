using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultPanel : MonoBehaviour
{
    public TextMeshProUGUI rankT;
    public TextMeshProUGUI distanceT;
    public TextMeshProUGUI bestT;
    public Image distanceImg;
    public Image charImg;
    private int scoreAniInt = 0;

    void Awake()
    {

    }

    void OnEnable()
    {
        PlayfabManager.instance.GetLeaderboard();
        rankT.text = $"{GameManager.instance.rank.ToString()} st";

        if (GameManager.instance.rank <= 3)
            rankT.color = new Color(255 / 255f, 221 / 255f, 0 / 255f, 255 / 255f);
        else if (GameManager.instance.rank <= 6)
            rankT.color = new Color(255 / 255f, 240 / 255f, 136 / 255f, 255 / 255f);
        else
            rankT.color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);

        distanceImg.fillAmount = 0;
        scoreAniInt = 0;
        bestT.text = $"Best Score : {GameManager.instance.bestScore: 0000} m";
        charImg.GetComponent<RectTransform>().anchoredPosition += new Vector2((100f), 0);
        StartCoroutine(ChangeDistance());
    }

    IEnumerator ChangeDistance()
    {
        float fillV = ((float)GameManager.instance.score / (float)GameManager.instance.bestPlayerScore);
        float smoothT = 10f;

        for (int i = 0; i < smoothT; i++)
        {
            if (GameManager.instance.score > scoreAniInt)
                scoreAniInt += Mathf.FloorToInt(GameManager.instance.score / 10f);
            else
                scoreAniInt = GameManager.instance.score;
            yield return new WaitForSeconds(0.1f);
            distanceImg.fillAmount = (fillV / smoothT) * i;

            charImg.GetComponent<RectTransform>().anchoredPosition = new Vector2(((fillV / smoothT) * i * 600) + 100, 100);
        }
    }

    public void PanelOff()
    {
        GameManager.instance.isResult = false;
        this.gameObject.SetActive(false);
    }

    void Update()
    {
        distanceT.text = $"{scoreAniInt: 0000} m";
    }
}
