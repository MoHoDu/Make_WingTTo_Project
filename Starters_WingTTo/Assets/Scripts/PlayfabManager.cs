using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayfabManager : Singleton<PlayfabManager>
{
    public TMP_InputField EmailInput, PasswordInput, UsernameInput;
    public GameObject pressText;
    public TextMeshProUGUI[] LogText;

    public TextMeshProUGUI waringText;

    private void Awake()
    {
        if (GameObject.Find("WaringText") != null)
        {
            waringText = GameObject.Find("WaringText").GetComponent<TextMeshProUGUI>();
            WaringOff();
        }
    }

    private void Update()
    {

    }

    public void WaringOff()
    {
        waringText.transform.parent.gameObject.SetActive(false);
    }

    public void LoginBtn()
    {
        var request = new LoginWithEmailAddressRequest { Email = EmailInput.text, Password = PasswordInput.text };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, (error) =>
        {
            waringText.transform.parent.gameObject.SetActive(true);
            waringText.text = "Check your email or password";
            Invoke("WaringOff", 2f);
        });
    }

    void OnLoginSuccess(LoginResult result)
    {
        print("로그인 성공");
        GameManager.instance.email = EmailInput.text;
        GameManager.instance.myID = result.PlayFabId;
        // GameManager.instance.nick = UsernameInput.text;
        GetPlayerProfile(result.PlayFabId);

        pressText.SetActive(true);
        GameObject.Find("LoginBtn").SetActive(false);
        GameObject.Find("RegisBtn").SetActive(false);
        GameObject.Find("Email").SetActive(false);
        GameObject.Find("PS").SetActive(false);

        // GameManager.instance.nick = result.InfoResultPayload.PlayerProfile.DisplayName;
    }

    public void RegisterBtn()
    {
        var request = new RegisterPlayFabUserRequest { Email = EmailInput.text, Password = PasswordInput.text, Username = UsernameInput.text, DisplayName = UsernameInput.text };
        PlayFabClientAPI.RegisterPlayFabUser(request, (result) => print("회원가입 성공"), (error) =>
        {
            waringText.transform.parent.gameObject.SetActive(true);
            waringText.text = "Check email format / password (up to 6 words) / nickname (up to 2 words)";
            Invoke("WaringOff", 2f);
        });
    }

    void GetPlayerProfile(string playFabId)
    {
        PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest()
        {
            PlayFabId = playFabId,
            ProfileConstraints = new PlayerProfileViewConstraints()
            {
                ShowDisplayName = true
            }
        },
        result =>
            {
                Debug.Log("The player's DisplayName profile data is: " + result.PlayerProfile.DisplayName);
                if (GameManager.instance.nick == "")
                    GameManager.instance.nick = result.PlayerProfile.DisplayName;
            },
        error => Debug.LogError(error.GenerateErrorReport()));
    }

    public void GetStat()
    {
        int resultN = 0;
        var request = new GetPlayerStatisticsRequest { StatisticNames = new List<string> { "HighScore" } };
        PlayFabClientAPI.GetPlayerStatistics(request, (result) =>
                {
                    if (result.Statistics == null)
                        GameManager.instance.bestScore = resultN;
                    else
                    {
                        var stat = result.Statistics[0];
                        // print(stat.StatisticName);
                        // print(stat.Value);
                        resultN = stat.Value;
                        // GameManager.instance.nick = stat.StatisticName;
                        GameManager.instance.bestScore = resultN;
                    }
                }
            , (error) => print("데이터 불러오기 실패"));
    }

    public void SetStat()
    {
        // int stat = GetStat();
        if (GameManager.instance.bestScore >= GameManager.instance.score)
        {
            SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
            return;
        }

        var request = new UpdatePlayerStatisticsRequest { Statistics = new List<StatisticUpdate> { new StatisticUpdate { StatisticName = "HighScore", Value = GameManager.instance.score } } };
        PlayFabClientAPI.UpdatePlayerStatistics(request, (result) =>
            {
                print("값 저장됨");
                SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
            }
        , (error) => print("값 저장실패"));
    }

    public void GetLeaderboard()
    {
        if (LogText == null || LogText.Length != GameObject.Find("LogBK").GetComponentsInChildren<TextMeshProUGUI>().Length)
            LogText = GameObject.Find("LogBK").GetComponentsInChildren<TextMeshProUGUI>();

        var request = new GetLeaderboardRequest { StartPosition = 0, StatisticName = "HighScore", MaxResultsCount = 20, ProfileConstraints = new PlayerProfileViewConstraints() { ShowLocations = true, ShowDisplayName = true } };
        PlayFabClientAPI.GetLeaderboard(request, (result) =>
        {
            GameManager.instance.leaderBoardPlayerCount = result.Leaderboard.Count;
            GameManager.instance.leaderBoardNames.Clear();
            GameManager.instance.leaderBoardRecords.Clear();
            GameManager.instance.bestPlayerScore = result.Leaderboard[0].StatValue;
            for (int i = 0; i < result.Leaderboard.Count; i++)
            {
                var curBoard = result.Leaderboard[i];
                GameManager.instance.leaderBoardNames.Add(curBoard.DisplayName);
                GameManager.instance.leaderBoardRecords.Add(curBoard.StatValue);
                if (curBoard.DisplayName == GameManager.instance.nick)
                    GameManager.instance.rank = i + 1;
                GameObject.Find("LogBK").GetComponentsInChildren<TextMeshProUGUI>()[i].text = curBoard.Profile.Locations[0].CountryCode.Value + " / " + curBoard.DisplayName + " / " + curBoard.StatValue + "\n";
            }
        },
        (error) => print("리더보드 불러오기 실패"));
    }
}
