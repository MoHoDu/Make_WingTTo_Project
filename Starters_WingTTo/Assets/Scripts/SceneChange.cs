using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneChange : MonoBehaviour
{
    public GameObject resultPanel;

    void Awake()
    {
        Debug.Log("Awake");
        // PlayfabManager.instance.LogText = GameObject.Find("LogBK").GetComponentsInChildren<TextMeshProUGUI>();
        PlayfabManager.instance.GetStat();
        UpdateLeaderBoard();
    }

    public void UpdateLeaderBoard()
    {
        PlayfabManager.instance.GetLeaderboard();
    }

    public void ChangeScene(int num)
    {
        SceneManager.LoadScene(num);
    }

    void Update()
    {
        if (GameManager.instance.isResult && !resultPanel.activeSelf)
        {
            resultPanel.SetActive(true);
        }
    }
}
