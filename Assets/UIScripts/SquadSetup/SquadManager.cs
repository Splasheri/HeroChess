using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadManager : MonoBehaviour
{
    [System.Serializable]
    public struct UnitStats
    {
        public int id;
        public int lvl;
        public int position;
        public int team;
        public int unitType;
        public int hp;
        public int initiative;
        public int amountofdice;
        public int movetype;
        public int attacktype;
        public int dicetype;
        public SkillPatterns.skill skilltype;
        public AttackPatterns.attackEffect attackEffect;
        public int cost;
        public string characterName;
        public string fileName;

        public void ChangeId(int i)
        {
            id = i;
        }
        public void ChangeTeam(int i)
        {
            team = i;
        }
    };
}
