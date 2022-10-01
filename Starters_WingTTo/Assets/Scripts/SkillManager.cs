using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillEnum
{
    none = 0,
    skill1,
    skill2,
    skill3,
    // skill4,
    // skill5
}

public class SkillManager : Singleton<SkillManager>
{
    public SkillEnum skillenum = 0;

    public int SkillIndex = 0;

    void Awake()
    {

    }

    public void UseSkill(int skillN)
    {
        Debug.Log($"skill{skillN} on");
        switch (skillN)
        {
            case 0:
                NoaBall();
                // Debug.Log($"skill{skillN} play");
                break;
            case 1:
                // NoaBall();
                StartCoroutine(Dash());
                break;
            case 2:
                // NoaStars();
                NoaRock();
                break;
            case 3:
                NoaBlackHoll();
                break;
            case 4:
                NoaRock();
                break;
        }
    }

    IEnumerator Dash()
    {
        Debug.Log("Dash");
        GameObject playerObj = GameObject.Find("Player");
        SpriteRenderer charImg = playerObj.transform.GetChild(GameManager.instance.selectChar).GetComponent<SpriteRenderer>();
        charImg.color = new Color(1, 0, 0, 0.8f);
        playerObj.layer = 7;
        playerObj.GetComponent<Player>().addMoveSpeed = 5;
        playerObj.GetComponent<Player>().playerDash.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        charImg.color = new Color(1, 1, 1, 1);
        playerObj.layer = 3;
        playerObj.GetComponent<Player>().addMoveSpeed = 5;
        playerObj.GetComponent<Player>().playerDash.SetActive(false);
    }

    void NoaBall()
    {
        GameObject playerObj = GameObject.Find("Player");
        GameObject noaBall = playerObj.GetComponent<Player>().noaBall;
        GameObject curBall = Instantiate(noaBall);
        curBall.transform.position = playerObj.transform.position;
    }

    void NoaStars()
    {

    }

    void NoaBlackHoll()
    {

    }

    void NoaRock()
    {
        GameObject playerObj = GameObject.Find("Player");
        playerObj.GetComponent<Player>().isRocked = true;
    }

    void Update()
    {

    }
}
