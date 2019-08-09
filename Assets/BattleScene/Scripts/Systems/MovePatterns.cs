using UnityEngine;
using System.Collections.Generic;

public static class MovePatterns 
{
    public enum moveType {
        KingMover = 8,
        PawnMover = 4,
        KnightMover = 6
    }
    
    public static bool chooseType(int inp,int start, int finish)
    {   
        if (start==finish)
        {
            return true;
        }        
        switch (inp)
        {
            case 0:
                return Immovable();
            case 4:
                return PawnMover(start, finish);
            case 6:
                return KnightMover(start, finish);
            case 3:
                return BishopMover(start, finish);
            case 1:
                return TowerMover(start, finish);
            case 8:
                return KingMover(start, finish);
            case 99:
                return CheatMover();
            default :
                return Immovable();
        }
    }
    public static bool KingMover(int attacker_cell, int target_cell)
    {
        int attackerX = attacker_cell%8;
        int attackerY = attacker_cell/8;
        int targetX = target_cell%8;
        int targetY = target_cell/8;
        if (Mathf.Abs(attackerX-targetX)<=1&&Mathf.Abs(attackerY-targetY)<=1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static bool KnightMover(int attacker_cell, int target_cell)
    {
        int attackerX = attacker_cell%8;
        int attackerY = attacker_cell/8;
        int targetX = target_cell%8;
        int targetY = target_cell/8;
        if (attacker_cell==target_cell)
        {
            return true;
        }
        if (((targetX-attackerX)*(targetX-attackerX)+(targetY-attackerY)*(targetY-attackerY))==5)
        {
            return true;
        }
        else
        {
            return false;
        }
    }   
    public static bool Immovable()
    {
        return false;
    }
    public static bool PawnMover(int attacker_cell, int target_cell)
    {
        int attackerX = attacker_cell % 8;
        int attackerY = attacker_cell / 8;
        int targetX = target_cell % 8;
        int targetY = target_cell / 8;
        if (Mathf.Abs(attackerX - targetX) <= 1 && Mathf.Abs(attackerY-targetY)<=1 && (attackerY - targetY) * (attackerX - targetX)==0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static bool CheatMover()
    {
        return true;
    }
    public static bool BishopMover(int attacker_cell, int target_cell)
    {
        int attackerX = attacker_cell % 8;
        int attackerY = attacker_cell / 8;
        int targetX = target_cell % 8;
        int targetY = target_cell / 8;
        if (Mathf.Abs(attackerX-targetX)==Mathf.Abs(attackerY-targetY))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static bool TowerMover(int attacker_cell, int target_cell)
    {

        int attackerX = attacker_cell % 8;
        int attackerY = attacker_cell / 8;
        int targetX = target_cell % 8;
        int targetY = target_cell / 8;
        if ((attackerX==targetX&&attackerY!=targetY)||(attackerX!=targetX&&attackerY==targetY))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}