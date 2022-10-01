using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net;
using UnityEngine.Networking;
using TMPro;
using Unity.VisualScripting;

[Inspectable]
public class GameManager : Singleton<GameManager>
{
    [Inspectable]
    public string nick = "";
    [Inspectable]
    public string email = "";
    [Inspectable]
    public string myID = "";
    [Inspectable]
    public int score = 0;
    [Inspectable]
    public int bestScore = 0;
    [Inspectable]
    public int rank = 1;
    [Inspectable]
    public int bestPlayerScore = 0;
    [Inspectable]
    public bool isResult = false;

    public int leaderBoardPlayerCount = 0;
    public List<string> leaderBoardNames = new List<string>();
    public List<float> leaderBoardRecords = new List<float>();

    [Inspectable]
    public int selectChar = 0;

    void Awake()
    {

    }

    void Update()
    {

    }
}
