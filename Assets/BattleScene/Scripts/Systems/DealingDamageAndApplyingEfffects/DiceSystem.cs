using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceSystem
{
    public static int RollBunchOfDice(int count, int type, bool reroll=false)
    {
        if (count==0)
        {            
            return 0;
        }
        int summary = 0;
        for (int i = 0; i < count; i++)
        {
            int number = DiceRoll(type);
            if (number == 1 && reroll)
            {
                number = DiceRoll(type);
            }
            summary += number;
        }
        return summary;
    }
    public static int DiceRoll(int dX)
    {
        return Random.Range(1,dX+1);
    }
}
