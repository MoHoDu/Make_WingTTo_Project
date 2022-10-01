using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Create SkillInfo")]
public class Skill : ScriptableObject
{
    public string skillName;
    public int skillN;
    public Sprite skillSprite;
    public string skillText;
}
