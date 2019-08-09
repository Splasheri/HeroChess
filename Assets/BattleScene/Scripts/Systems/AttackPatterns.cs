using UnityEngine;
using Unity.Entities;
using UnityEngine.UI;
using System.Collections.Generic;

public static class AttackPatterns 
{
    public enum attackEffect
    {
        JustHit = 0,
        CriticalStrike = 1,
        HolyDazzling = 2,
        UnexpectedStrike = 3,
        HolyStrike = 4,
        MechanicalAccuracy = 5,
        MeltingBreath = 6,
        TearingStrike = 7,
        DemonicStrike = 8,
        MightyStrike = 9,
        PanicAttack = 10,
        ClawHit = 11,
        ClawStrike = 12,
        Vampire = 13,
        FuriousStrike = 14,
        Scavenger = 15,
    }
    public static string chooseEffect(out int damageOut, attackEffect index, EntityManager em, EntityCommandBuffer buffer, Entity attacker, Entity target, int damage)
    {
        string s;
        int damageO = 0;
        switch (index)
        {
            case 0:
                s = JustHit(out damageO, em, buffer, attacker, target, damage);
                damageOut = damageO;
                return s;
            case attackEffect.Scavenger:
                s = Scavenger(out damageO, em, buffer, attacker, target, damage);
                damageOut = damageO;
                return s;
            case attackEffect.CriticalStrike:
                s = CriticalStrike(out damageO, em, buffer, attacker, target, damage);
                damageOut = damageO;
                return s;
            case attackEffect.HolyDazzling:
                s = HolyDazzling(out damageO, em, buffer, attacker, target, damage);
                damageOut = damageO;
                return s;
            case attackEffect.UnexpectedStrike:
                s = UnexpectedStrike(out damageO, em, buffer, attacker, target, damage);
                damageOut = damageO;
                return s;
            case attackEffect.HolyStrike:
                s = HolyStrike(out damageO, em, buffer, attacker, target, damage);
                damageOut = damageO;
                return s;
            case attackEffect.MechanicalAccuracy:
                s = MechanicalAccuracy(out damageO, em, buffer, attacker, target, damage);
                damageOut = damageO;
                return s;
            case attackEffect.MeltingBreath:
                s = MeltingBreath(out damageO, em, buffer, attacker, target, damage);
                damageOut = damageO;
                return s;
            case attackEffect.TearingStrike:
                s = TearingStrike(out damageO, em, buffer, attacker, target, damage);
                damageOut = damageO;
                return s;
            case attackEffect.DemonicStrike:
                s = DemonicStrike(out damageO, em, buffer, attacker, target, damage);
                damageOut = damageO;
                return s;
            case attackEffect.MightyStrike:
                s = MightyStrike(out damageO, em, buffer, attacker, target, damage);
                damageOut = damageO;
                return s;
            case attackEffect.PanicAttack:
                s = PanicAttack(out damageO, em, buffer, attacker, target, damage);
                damageOut = damageO;
                return s;
            case attackEffect.ClawHit:
                s = ClawHit(out damageO, em, buffer, attacker, target, damage);
                damageOut = damageO;
                return s;
            case attackEffect.ClawStrike:
                s = ClawStrike(out damageO, em, buffer, attacker, target, damage);
                damageOut = damageO;
                return s;
            case attackEffect.Vampire:
                s = Vampire(out damageO, em, buffer, attacker, target, damage);
                damageOut = damageO;
                return s;
            case attackEffect.FuriousStrike:
                s = FuriousStrike(out damageO, em, buffer, attacker, target, damage);
                damageOut = damageO;
                return s;
            default:
                s = "";
                damageOut = damage;
                return s;
        }
    }
    public static string Scavenger(out int damageOut, EntityManager em, EntityCommandBuffer buffer, Entity attacker, Entity target, int damage)
    {
        if (!em.HasComponent<FesteringWound>(attacker))
        {
            int startHp = em.GetComponentData<HP>(attacker).startValue;
            int targetHp = em.GetComponentData<HP>(target).currentValue;
            if (damage>targetHp)
            {
                buffer.SetComponent<HP>(attacker, new HP() { startValue = startHp, currentValue = startHp });
                damageOut = damage;
                return "Scavenger";
            }
        }
        damageOut = damage;
        return "";
    }
    public static string CriticalStrike(out int damageOut, EntityManager em, EntityCommandBuffer buffer, Entity attacker, Entity target, int damage)
    {
        if (DiceSystem.DiceRoll(6) == 6)
        {
            damageOut = damage * 2;
            return "CRITICAL STRIKE";
        }
        else
        {
            damageOut = damage;
            return "";
        }
    }
    public static string HolyDazzling(out int damageOut, EntityManager em, EntityCommandBuffer buffer, Entity attacker, Entity target, int damage)
    {
        damageOut = damage;
        if (!em.HasComponent<Bless>(attacker))
        {
            buffer.AddComponent<Stunned>(target, new Stunned() { duration = 2 });
            return "BLIND";
        }
        return "";
    }
    public static string UnexpectedStrike(out int damageOut, EntityManager em, EntityCommandBuffer buffer, Entity attacker, Entity target, int damage)
    {
        string s = "";
        damageOut =  damage;
        if (DiceSystem.DiceRoll(6) == 6)
        {
            if (!em.HasComponent<Poison>(target))
            {
                buffer.AddComponent<Poison>(target, new Poison() { duration = 1 });
                s += "Poison attack\n";
            }
        }
        if (DiceSystem.DiceRoll(6) == 6)
        {
            if (!em.HasComponent<BrokenArmour>(target))
            {
                buffer.AddComponent<BrokenArmour>(target, new BrokenArmour() { duration = 1 });
                s += "BrokenArmour\n";
            }
        }
        if (DiceSystem.DiceRoll(6) == 6)
        {
            damageOut = damage*2;
            s += "CRITICAL STRIKE";
        }
        return s;
    }
    public static string HolyStrike(out int damageOut, EntityManager em, EntityCommandBuffer buffer, Entity attacker, Entity target, int damage)
    {
        damageOut = damage;
        if (!em.HasComponent<Bless>(attacker))
        {
            damageOut = damage*2;
            return "HOLY STRIKE";
        }
        return "";
    }
    public static string MechanicalAccuracy(out int damageOut, EntityManager em, EntityCommandBuffer buffer, Entity attacker, Entity target, int damage)
    {
        damageOut = damage;
        if (!em.HasComponent<Cooldown>(attacker))
        {
            damageOut = damage * 2;
            return "MECHANICAL CRIT";
        }
        return "";
    }
    public static string MeltingBreath(out int damageOut, EntityManager em, EntityCommandBuffer buffer, Entity attacker, Entity target, int damage)
    {
        damageOut = damage;
        if (!em.HasComponent<DestroyedArmour>(target))
        {
            buffer.AddComponent<DestroyedArmour>(target, new DestroyedArmour() { });
            return "MELTING BREATH";
        }
        return "";
    }
    public static string TearingStrike(out int damageOut, EntityManager em, EntityCommandBuffer buffer, Entity attacker, Entity target, int damage)
    {
        damageOut = damage;
        if (!em.HasComponent<FesteringWound>(target))
        {
            buffer.AddComponent<FesteringWound>(target, new FesteringWound() { });
            return "Festering Wound";
        }
        return "";
    }
    public static string DemonicStrike(out int damageOut, EntityManager em, EntityCommandBuffer buffer, Entity attacker, Entity target, int damage)
    {
        damageOut = damage;
        var hp = em.GetComponentData<HP>(target);
        buffer.SetComponent<HP>(target, new HP() { startValue = hp.startValue, currentValue = hp.currentValue / 2 });
        return "DEMONIC STRIKE";
    }
    public static string MightyStrike(out int damageOut, EntityManager em, EntityCommandBuffer buffer, Entity attacker, Entity target, int damage)
    {
        var hp = em.GetComponentData<HP>(target);
        damageOut = damage;
        if (DiceSystem.DiceRoll(10)==10)
        {
            buffer.SetComponent<HP>(target, new HP() { startValue = hp.startValue, currentValue = hp.currentValue / 2 });
            damageOut = 0;
            return "MIGHTY STRIKE";
        }
        return "";
    }
    public static string PanicAttack(out int damageOut, EntityManager em, EntityCommandBuffer buffer, Entity attacker, Entity target, int damage)
    {
        var a = em.GetComponentData<Attack>(target);
        buffer.SetComponent<Attack>(target, new Attack() { index = a.index, amountOfCubes = a.amountOfCubes, typeOfCubes = 6 });
        damageOut = damage;
        return "SPOOKY MONSTER";
    }
    public static string ClawHit(out int damageOut, EntityManager em, EntityCommandBuffer buffer, Entity attacker, Entity target, int damage)
    {
        damageOut = damage;
        if (DiceSystem.DiceRoll(4) == 4)
        {
            if (!em.HasComponent<Stunned>(target))
            {
                buffer.AddComponent<Stunned>(target, new Stunned() { duration = 1 });
                return "STUNNED";
            }
        }
        return "";
    }
    public static string ClawStrike(out int damageOut, EntityManager em, EntityCommandBuffer buffer, Entity attacker, Entity target, int damage)
    {
        damageOut = damage;
        if (!em.HasComponent<Stunned>(target))
        {
            buffer.AddComponent<Stunned>(target, new Stunned() { duration = 1 });
            return "STUNNED";
        }
        else
        {
            damageOut = damage*2;
            return "CRABICAL STRIKE";
        }
    }
    public static string Vampire(out int damageOut, EntityManager em, EntityCommandBuffer buffer, Entity attacker, Entity target, int damage)
    {
        int startHp = em.GetComponentData<HP>(attacker).startValue;
        int curHp = em.GetComponentData<HP>(attacker).currentValue;
        buffer.SetComponent<HP>(attacker, new HP() { currentValue = curHp + damage > startHp ? startHp : curHp + damage });
        damageOut = damage;
        return "Mmmmm delicious";
    }
    public static string FuriousStrike(out int damageOut, EntityManager em, EntityCommandBuffer buffer, Entity attacker, Entity target, int damage)
    {        
        int lvlProxy = em.GetComponentData<Lvl>(attacker).value;
        damageOut = damage;

        if (em.HasComponent<Bleeding>(target))
        {
            buffer.SetComponent<Bleeding>(target, new Bleeding() { damage = em.GetComponentData<Bleeding>(target).damage + lvlProxy });
            return "FURIOUS STRIKE";
        }
        else
        {
            buffer.AddComponent<Bleeding>(target, new Bleeding() { damage = lvlProxy });
            return "FURIOUS FINISH OFF";
        }
    }
    public static string JustHit(out int damageOut, EntityManager em, EntityCommandBuffer buffer, Entity attacker, Entity target, int damage)
    {
        damageOut = damage;
        return "No effect";
    }
    public static int Evasion(int damage)
    {
        if (DiceSystem.DiceRoll(10)==10)
        {
            return 0;
        }
        return damage;
    }


    public enum attackType
    {
        frontAttack = 6,
        kingAttack = 8,
        crossAttack = 4,
        lineAttack = 3,
        TentacleStrike = 0,
        ZeroAttack = 5
    }
    public static bool chooseType(int index, int attackerPosition, int targetPosition)
    {
        float x = (float)(attackerPosition / 8);
        float y = (float)(attackerPosition % 8);
        float a = (float)(targetPosition / 8);
        float b = (float)(targetPosition % 8);
        switch (index)
        {
            case 6:
                return frontAttack(attackerPosition, targetPosition);
            case 8:
                return kingAttack(attackerPosition, targetPosition);
            case 4:
                return crossAttack(attackerPosition, targetPosition);
            case 3:
                return lineAttack(attackerPosition, targetPosition);
            case 99:
                return TentacleStrike(attackerPosition, targetPosition);
            case 0:
                return ZeroAttack(attackerPosition, targetPosition);
            default:
                return kingAttack(attackerPosition, targetPosition);
        }
    }

    public static bool frontAttack(int attacker_cell, int target_cell)
    {
        int attackerX = attacker_cell % 8;
        int attackerY = attacker_cell / 8;
        int targetX = target_cell % 8;
        int targetY = target_cell / 8;
        if (Mathf.Abs(attackerX-targetX)<=1 &&  Mathf.Abs(attackerY-targetY)==1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static bool kingAttack(int attacker_cell, int target_cell)
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
    public static bool crossAttack (int attacker_cell, int target_cell)
    {
        int attackerX = attacker_cell % 8;
        int attackerY = attacker_cell / 8;
        int targetX = target_cell % 8;
        int targetY = target_cell / 8;
        if (Mathf.Abs(attackerX-targetX)==1&&Mathf.Abs(attackerY-targetY)==1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static bool lineAttack(int attacker_cell, int target_cell)
    {

        int attackerX = attacker_cell % 8;
        int attackerY = attacker_cell / 8;
        int targetX = target_cell % 8;
        int targetY = target_cell / 8;
        if (targetX == attackerX)
            return true;
        else
            return false;
    }
    public static bool TentacleStrike(int attacker_cell, int target_cell)
    {
        return true;
    }
    public static bool ZeroAttack(int attacker_cell, int target_cell)
    {
        return false;
    }
}