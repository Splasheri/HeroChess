using UnityEngine;
using Unity.Entities;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Text;
using Unity.Collections;
using Unity.Transforms;
using Unity.Mathematics;
using System.Collections;

public class SkillPatterns
{
    public enum skill
    {
        Nothing,
        BadSign,
        Taunt,
        Overcharge,
        HundredStrike,
        ThousandStrike,
        DryadTouch,
        HealingTree,
        MichaelCurse,
        MindControll,
        ChaoticJump,
        SlimePoison,
        HordeGrowing,
        Brawler,
        DeathAway,
        Regeneration,
        Chilblain,
        Hook,
        Amoreux,
        FireCover,
        GrandmaCakes,
        AngelBlessing,
        WaterPool,
        ElectricTrap,
        NineTails,
        PowerfullWings,
        MythicalPatron,
        SunStrike,
        GrowingTurtle,
        XoerCurse,
        FeastOfHeroes,
        MagicMissiles,
        WeepingWillow,
        ClockworkEngine,
        QueenTouch,
        Whirlpool,
        Stoner,
        Devour,
        CurseRelease,
        Absorbing,
        DragonBlessing,
        Absorbtion
    }

    public void chooseSkill(skill s, EntityManager em, EntityCommandBuffer buffer, Entity caster)
    {
        string methodName = "Skill"+s.ToString();
        MethodInfo mi = this.GetType().GetMethod(methodName);
        mi.Invoke(this, new object[] {  (EntityManager)em,  (EntityCommandBuffer)buffer,  (Entity)caster } );
    }

    public void SkillNothing(EntityManager em, EntityCommandBuffer buffer, Entity caster)
    {
        buffer.AddComponent<Action>(caster, new Action() { });
    }

    public void SkillBadSign(EntityManager em, EntityCommandBuffer buffer, Entity caster)
    {
        Entity target;
        Lvl lvl = em.GetComponentData<Lvl>(caster);
        if (ChooseBestTarget(out target, em.GetSharedComponentData<Team>(caster).value * -1, new EntityQueryDesc { None = new ComponentType[] { typeof(BadSign) }, All = new ComponentType[] { typeof(Unit), typeof(Cost), typeof(HP), typeof(Team), typeof(Lvl) } }))
        {
            var pos = em.GetComponentData<BoardPosition>(target);
            buffer.AddComponent
            (
                target,
                new BadSign() { duration = lvl.value }
            );
            buffer.AddComponent<Cooldown>(caster, new Cooldown() { duration = 1 });
            animationManager.instance.CreateUniqueAnimationSequence(new animationManager.particleAnimation() { name = "Curse", position = new Vector3(pos.cell % 8, pos.cell / 8, -4), parent = GameObject.Find("Board").transform, scale = new Vector3(1, 2, 1) });
            animationManager.instance.CreateAnimationHandler("Curse");
            animationManager.instance.TryPlayUniqueAnimation("Curse");
            if (SquadsManagement.instance.allCards.ContainsKey(em.GetComponentData<Id>(target).value))
            {
                SquadsManagement.instance.allCards[em.GetComponentData<Id>(target).value].GetComponent<UnitCardFight>().UpdateData(target);
            }
        }
        else
        {
            buffer.AddComponent<Action>(caster, new Action() { });
        }
    }
    public void SkillTaunt(EntityManager em, EntityCommandBuffer buffer, Entity caster)
    {
        Lvl lvl = em.GetComponentData<Lvl>(caster);
        Cost cost = em.GetComponentData<Cost>(caster);
        buffer.SetComponent(caster, new Cost() { value = cost.value + lvl.value * 50 });
        var pos = em.GetComponentData<BoardPosition>(caster);
        animationManager.instance.CreateUniqueAnimationSequence(new animationManager.particleAnimation() { name = "CasualBuff", position = new Vector3(pos.cell % 8, pos.cell / 8, -4), parent = GameObject.Find("Board").transform, scale = new Vector3(2, 4, 1) });
        animationManager.instance.CreateAnimationHandler("CasualBuff");
        animationManager.instance.TryPlayUniqueAnimation("CasualBuff");
        if (SquadsManagement.instance.allCards.ContainsKey(em.GetComponentData<Id>(caster).value))
        {
            SquadsManagement.instance.allCards[em.GetComponentData<Id>(caster).value].GetComponent<UnitCardFight>().UpdateData(caster);
        }
    }
    public void SkillOvercharge(EntityManager em, EntityCommandBuffer buffer, Entity caster)
    {
        if (DiceSystem.DiceRoll(20) == 1)
        {
            KillUnit(caster,buffer);
            var pos = em.GetComponentData<BoardPosition>(caster);
            animationManager.instance.CreateUniqueAnimationSequence(new animationManager.particleAnimation() { name = "Explosion", position = new Vector3(pos.cell % 8, pos.cell / 8, -4), parent = GameObject.Find("Board").transform, scale = new Vector3(2, 4, 1) });
            animationManager.instance.TryPlayUniqueAnimation("Explosion");
            if (SquadsManagement.instance.allCards.ContainsKey(em.GetComponentData<Id>(caster).value))
            {
                SquadsManagement.instance.allCards[em.GetComponentData<Id>(caster).value].GetComponent<UnitCardFight>().UpdateData(caster);
            }
        }
    }
    public void SkillHundredStrikes(EntityManager em, EntityCommandBuffer buffer, Entity caster)
    {
        if (DiceSystem.DiceRoll(8) == 8)
        {
            buffer.AddComponent(caster, new OneMoreAttack() { });
            var pos = em.GetComponentData<BoardPosition>(caster);
            animationManager.instance.CreateUniqueAnimationSequence(new animationManager.particleAnimation() { name = "CasualBuff", position = new Vector3(pos.cell % 8, pos.cell / 8, -4), parent = GameObject.Find("Board").transform, scale = new Vector3(2, 4, 1) });
            animationManager.instance.CreateAnimationHandler("CasualBuff");
            animationManager.instance.TryPlayUniqueAnimation("CasualBuff");
            if (SquadsManagement.instance.allCards.ContainsKey(em.GetComponentData<Id>(caster).value))
            {
                SquadsManagement.instance.allCards[em.GetComponentData<Id>(caster).value].GetComponent<UnitCardFight>().UpdateData(caster);
            }
        }
    }
    public void SkillThousandStrikes(EntityManager em, EntityCommandBuffer buffer, Entity caster)
    {
        if (DiceSystem.DiceRoll(4) == 4)
        {
            buffer.AddComponent(caster, new OneMoreAttack() { });
            var pos = em.GetComponentData<BoardPosition>(caster);
            animationManager.instance.CreateUniqueAnimationSequence(new animationManager.particleAnimation() { name = "CasualBuff", position = new Vector3(pos.cell % 8, pos.cell / 8, -4), parent = GameObject.Find("Board").transform, scale = new Vector3(2, 4, 1) });
            animationManager.instance.CreateAnimationHandler("CasualBuff");
            animationManager.instance.TryPlayUniqueAnimation("CasualBuff");
            if (SquadsManagement.instance.allCards.ContainsKey(em.GetComponentData<Id>(caster).value))
            {
                SquadsManagement.instance.allCards[em.GetComponentData<Id>(caster).value].GetComponent<UnitCardFight>().UpdateData(caster);
            }
        }
    }
    public void SkillDryadTouch(EntityManager em, EntityCommandBuffer buffer, Entity caster)
    {
        Entity target;
        var attack = em.GetComponentData<Attack>(caster);
        if (ChooseBestTarget(out target, em.GetSharedComponentData<Team>(caster).value, new EntityQueryDesc { None = new ComponentType[] { typeof(FesteringWound), typeof(Dead) }, All = new ComponentType[] { typeof(Unit), typeof(Lvl), typeof(HP), typeof(Team), typeof(BoardPosition) } }))
        {
            var hp = em.GetComponentData<HP>(target);
            var pos = em.GetComponentData<BoardPosition>(target);
            int heal = DiceSystem.RollBunchOfDice(attack.amountOfCubes, attack.typeOfCubes);
            buffer.SetComponent<HP>
            (
                target,
                new HP() { startValue = hp.startValue, currentValue = hp.currentValue + heal > hp.startValue ? hp.startValue : hp.currentValue + heal }
            );
            animationManager.instance.CreateUniqueAnimationSequence(new animationManager.particleAnimation() { name = "HealOnce", position = new Vector3(pos.cell%8, pos.cell/ 8, 0), parent = GameObject.Find("Board").transform, scale = new Vector3(2,4,1)});
            animationManager.instance.CreateAnimationHandler("HealOnce");
            animationManager.instance.TryPlayUniqueAnimation("HealOnce");
            if (SquadsManagement.instance.allCards.ContainsKey(em.GetComponentData<Id>(target).value))
            {
                SquadsManagement.instance.allCards[em.GetComponentData<Id>(target).value].GetComponent<UnitCardFight>().UpdateData(target);
            }
        }
    }
    public void SkillHealingTree(EntityManager em, EntityCommandBuffer buffer, Entity caster)
    {
        EntityQueryDesc eqd = new EntityQueryDesc { None = new ComponentType[] { typeof(FesteringWound) }, All = new ComponentType[] { typeof(Unit), typeof(Cost), typeof(HP), typeof(Team) } };
        EntityQuery allies = em.CreateEntityQuery(eqd);
        allies.SetFilter(new Team() { value = em.GetSharedComponentData<Team>(caster).value });
        var alliesE = allies.ToEntityArray(Allocator.TempJob);
        var alliesHp = allies.ToComponentDataArray<HP>(Allocator.TempJob);
        var a = em.GetComponentData<Attack>(caster);
        List<animationManager.particleAnimation> anims = new List<animationManager.particleAnimation>();
        for (int i = 0; i < allies.CalculateLength(); i++)
        {
            var pos = em.GetComponentData<BoardPosition>(alliesE[i]);
            int postHealHp = alliesHp[i].currentValue + DiceSystem.RollBunchOfDice(a.amountOfCubes, a.typeOfCubes);
            buffer.SetComponent
                   (
                       alliesE[i],
                       new HP() { currentValue = postHealHp > alliesHp[i].startValue ? alliesHp[i].startValue : postHealHp, startValue = alliesHp[i].startValue }
                   );
            anims.Add(new animationManager.particleAnimation() { name = "HealOnce", position = new Vector3(pos.cell % 8, pos.cell / 8, -4), parent = GameObject.Find("Board").transform, scale = new Vector3(2, 4, 1) });
            if (SquadsManagement.instance.allCards.ContainsKey(em.GetComponentData<Id>(alliesE[i]).value))
            {
                SquadsManagement.instance.allCards[em.GetComponentData<Id>(alliesE[i]).value].GetComponent<UnitCardFight>().UpdateData(alliesE[i]);
            }
        }
        alliesE.Dispose();
        alliesHp.Dispose();
        animationManager.instance.CreateMassAnimationName(anims.ToArray());
        animationManager.instance.CreateAnimationHandler("HealOnce",true);
        animationManager.instance.PlayMassAnimation(anims.ToArray());
    }
    public void SkillMichaelCurse(EntityManager em, EntityCommandBuffer buffer, Entity caster)
    {
        Entity target;
        var lvl = em.GetComponentData<Lvl>(caster);
        if (ChooseBestTarget(out target, em.GetSharedComponentData<Team>(caster).value * -1, new EntityQueryDesc { None = new ComponentType[] { typeof(MichaelCurse) }, All = new ComponentType[] { typeof(Unit), typeof(Lvl), typeof(HP), typeof(Team), typeof(BoardPosition) } }))
        {
            var pos = em.GetComponentData<BoardPosition>(target);
            buffer.AddComponent
            (
                target,
                new MichaelCurse() { duration = lvl.value / 2 }
            );
            if (em.HasComponent<Curse>(target))
            {
                buffer.AddComponent
                (
                   target,
                    new Curse() { duration = lvl.value / 2 }
                );
            }
            animationManager.instance.CreateUniqueAnimationSequence(new animationManager.particleAnimation() { name = "Curse", position = new Vector3(pos.cell % 8, pos.cell / 8, -4), parent = GameObject.Find("Board").transform, scale = new Vector3(1, 2, 1) });
            animationManager.instance.CreateAnimationHandler("Curse");
            animationManager.instance.TryPlayUniqueAnimation("Curse");
            if (SquadsManagement.instance.allCards.ContainsKey(em.GetComponentData<Id>(target).value))
            {
                SquadsManagement.instance.allCards[em.GetComponentData<Id>(target).value].GetComponent<UnitCardFight>().UpdateData(target);
            }
        }
    }
    public void SkillMindControll(EntityManager em, EntityCommandBuffer buffer, Entity caster)
    {
        EntityQuery enemies = em.CreateEntityQuery(typeof(Unit), typeof(Team), typeof(Lvl));
        Entity target;
        var cost = em.GetComponentData<Cost>(caster);
        if (ChooseBestTarget(out target, em.GetSharedComponentData<Team>(caster).value * -1, null, cost.value))
        {
            var pos = em.GetComponentData<BoardPosition>(target);
            buffer.SetSharedComponent<Team>
            (
                target,
                new Team() { value = em.GetSharedComponentData<Team>(caster).value }
            );
            animationManager.instance.CreateUniqueAnimationSequence(new animationManager.particleAnimation() { name = "HealOnce", position = new Vector3(pos.cell % 8, pos.cell / 8, -4), parent = GameObject.Find("Board").transform, scale = new Vector3(2, 4, 1) });
            animationManager.instance.CreateAnimationHandler("HealOnce");
            animationManager.instance.TryPlayUniqueAnimation("HealOnce");
            buffer.AddComponent<Cooldown>(caster, new Cooldown() { duration = 6 });
        }
    }
    public void SkillChaoticJump(EntityManager em, EntityCommandBuffer buffer, Entity caster)
    {
        var pos = em.GetComponentData<BoardPosition>(caster);
        EntityQueryDesc noneOccupied = new EntityQueryDesc()
        {
            None = new ComponentType[] { typeof(Occupied) },
            All = new ComponentType[] { typeof(Cell) }
        };
        EntityQuery freeCells = em.CreateEntityQuery(noneOccupied);
        var allCells = freeCells.ToEntityArray(Allocator.TempJob);
        var cellPosition = freeCells.ToComponentDataArray<Cell>(Allocator.TempJob);
        int randomCell = UnityEngine.Random.Range(0, freeCells.CalculateLength());
        animationManager.instance.DestroyMassAnimationInstance(new animationManager.particleAnimation() { name = "UnitCircle", position = new Vector3(pos.cell % 8 * 2.25f, pos.cell / 8 * 2.25f, -30) });
        buffer.SetComponent(caster, new BoardPosition() { cell = cellPosition[randomCell].number });
        buffer.AddComponent(allCells[randomCell], new Occupied() { });
        buffer.SetComponent(caster, new Translation() { Value = new float3(cellPosition[randomCell].number % 8 * 2.25f, cellPosition[randomCell].number / 8 * 2.25f, 0.4f) });
        animationManager.instance.CreateMassAnimationName(new animationManager.particleAnimation() { name = "UnitCircle", position = new Vector3(cellPosition[randomCell].number % 8 * 2.25f, cellPosition[randomCell].number / 8 * 2.25f, -30) }, em.GetSharedComponentData<Team>(caster).value);
        allCells.Dispose();
        cellPosition.Dispose();
        animationManager.instance.CreateUniqueAnimationSequence(new animationManager.particleAnimation() { name = "HealOnce", position = new Vector3(pos.cell % 8, pos.cell / 8, -4), parent = GameObject.Find("Board").transform, scale = new Vector3(2, 4, 1) });
        animationManager.instance.CreateAnimationHandler("HealOnce");
        animationManager.instance.TryPlayUniqueAnimation("HealOnce");
    }
    public void SkillSlimePoison(EntityManager em, EntityCommandBuffer buffer, Entity caster)
    {
        Entity target;
        var lvl = em.GetComponentData<Lvl>(caster);
        if (ChooseBestTarget(out target, em.GetSharedComponentData<Team>(caster).value * -1, new EntityQueryDesc { None = new ComponentType[] { typeof(Poison) }, All = new ComponentType[] { typeof(Unit), typeof(Cost), typeof(HP) } }))
        {
            var pos = em.GetComponentData<BoardPosition>(target);
            buffer.AddComponent
            (
                target,
                new Poison() { duration = lvl.value }
            );
            animationManager.instance.CreateUniqueAnimationSequence(new animationManager.particleAnimation() { name = "Poison", position = new Vector3(pos.cell % 8, pos.cell / 8, -4), parent = GameObject.Find("Board").transform, scale = new Vector3(2, 4, 1) });
            animationManager.instance.CreateAnimationHandler("Poison");
            animationManager.instance.TryPlayUniqueAnimation("Poison");
            if (SquadsManagement.instance.allCards.ContainsKey(em.GetComponentData<Id>(target).value))
            {
                SquadsManagement.instance.allCards[em.GetComponentData<Id>(target).value].GetComponent<UnitCardFight>().UpdateData(target);
            }
        }
    }
    public void SkillFishTail(EntityManager em, EntityCommandBuffer buffer, Entity caster)
    {
        if(!em.HasComponent<FishTail>(caster))
        {
            buffer.AddComponent<FishTail>(caster, new FishTail() { });
        }
        buffer.AddComponent<Action>(caster, new Action() { });
    }
    public void SkillHordeGrowing(EntityManager em, EntityCommandBuffer buffer, Entity caster)
    {
        var a = em.GetComponentData<Attack>(caster);
        var hp = em.GetComponentData<HP>(caster);
        buffer.SetComponent(caster, new Attack() { amountOfCubes = a.amountOfCubes + 1, index = a.index, typeOfCubes = a.typeOfCubes });
        buffer.SetComponent(caster, new HP() { currentValue = hp.currentValue + 4, startValue = hp.startValue + 4 });
        var pos = em.GetComponentData<BoardPosition>(caster);
        animationManager.instance.CreateUniqueAnimationSequence(new animationManager.particleAnimation() { name = "CasualBuff", position = new Vector3(pos.cell % 8, pos.cell / 8, -4), parent = GameObject.Find("Board").transform, scale = new Vector3(2, 4, 1) });
        animationManager.instance.CreateAnimationHandler("CasualBuff");
        animationManager.instance.TryPlayUniqueAnimation("CasualBuff");
        if (SquadsManagement.instance.allCards.ContainsKey(em.GetComponentData<Id>(caster).value))
        {
            SquadsManagement.instance.allCards[em.GetComponentData<Id>(caster).value].GetComponent<UnitCardFight>().UpdateData(caster);
        }
    }
    public void SkillBrawler(EntityManager em, EntityCommandBuffer buffer, Entity caster)
    {
        EntityQuery enemies = em.CreateEntityQuery(typeof(Unit), typeof(Team), typeof(BoardPosition), typeof(HP));
        var attack = em.GetComponentData<Attack>(caster);
        var pos = em.GetComponentData<BoardPosition>(caster);
        enemies.SetFilter(new Team() { value = em.GetSharedComponentData<Team>(caster).value * -1 });
        var enemyE = enemies.ToEntityArray(Allocator.TempJob);
        var enemyPosition = enemies.ToComponentDataArray<BoardPosition>(Allocator.TempJob);
        var enemyHp = enemies.ToComponentDataArray<HP>(Allocator.TempJob);
        int damage = DiceSystem.RollBunchOfDice(attack.amountOfCubes, attack.typeOfCubes);
        for (int i = 0; i < enemyPosition.Length; i++)
        {
            if (AttackPatterns.kingAttack(pos.cell, enemyPosition[i].cell))
            {
                buffer.SetComponent
                (
                    enemyE[i],
                    new HP() { startValue = enemyHp[i].startValue, currentValue = enemyHp[i].currentValue - damage}
                );
                if (SquadsManagement.instance.allCards.ContainsKey(em.GetComponentData<Id>(enemyE[i]).value))
                {
                    SquadsManagement.instance.allCards[em.GetComponentData<Id>(enemyE[i]).value].GetComponent<UnitCardFight>().UpdateData(enemyE[i], damage);
                }                             
            }
        }
        enemyHp.Dispose();
        enemyE.Dispose();
        enemyPosition.Dispose();
        animationManager.instance.CreateUniqueAnimationSequence(new animationManager.particleAnimation() { name = "Spiner", position = new Vector3(pos.cell % 8, pos.cell / 8, -4), parent = GameObject.Find("Board").transform, rotation = new Vector3(180,0,0) });
        animationManager.instance.CreateAnimationHandler("Spiner");
        animationManager.instance.TryPlayUniqueAnimation("Spiner");
    }
    public void SkillDeathAway(EntityManager em, EntityCommandBuffer buffer, Entity caster)
    {
        Entity target;
        var lvl = em.GetComponentData<Lvl>(caster);
        if (ChooseBestDeadTarget(out target, em.GetSharedComponentData<Team>(caster).value))
        {
            if (em.GetComponentData<HP>(target).currentValue < 0)
            {
                buffer.SetComponent<HP>(target, new HP() { currentValue = lvl.value * 4, startValue = em.GetComponentData<HP>(target).startValue });
            }
            var pos = em.GetComponentData<BoardPosition>(target);
            buffer.AddComponent<Cooldown>(caster, new Cooldown() { duration = 2 });
            animationManager.instance.CreateUniqueAnimationSequence(new animationManager.particleAnimation() { name = "HealOnce", position = new Vector3(pos.cell % 8, pos.cell / 8, -4), parent = GameObject.Find("Board").transform, scale = new Vector3(2, 4, 1) });
            animationManager.instance.CreateAnimationHandler("HealOnce");
            animationManager.instance.TryPlayUniqueAnimation("HealOnce");
            if (SquadsManagement.instance.allCards.ContainsKey(em.GetComponentData<Id>(target).value))
            {
                SquadsManagement.instance.allCards[em.GetComponentData<Id>(target).value].GetComponent<UnitCardFight>().UpdateData(target);
            }
        }
    }
    public void SkillRegeneration(EntityManager em, EntityCommandBuffer buffer, Entity caster)
    {
        var lvl = em.GetComponentData<Lvl>(caster);
        var hp = em.GetComponentData<HP>(caster);
        buffer.SetComponent<HP>(caster, new HP() { currentValue = hp.currentValue + lvl.value * 2 > hp.startValue ? hp.startValue : hp.currentValue + lvl.value * 2 });
        var pos = em.GetComponentData<BoardPosition>(caster);
        animationManager.instance.CreateUniqueAnimationSequence(new animationManager.particleAnimation() { name = "HealOnce", position = new Vector3(pos.cell % 8, pos.cell / 8, -4), parent = GameObject.Find("Board").transform, scale = new Vector3(2, 4, 1) });
        animationManager.instance.CreateAnimationHandler("HealOnce");
        animationManager.instance.TryPlayUniqueAnimation("HealOnce");
        if (SquadsManagement.instance.allCards.ContainsKey(em.GetComponentData<Id>(caster).value))
        {
            SquadsManagement.instance.allCards[em.GetComponentData<Id>(caster).value].GetComponent<UnitCardFight>().UpdateData(caster);
        }
    }
    public void SkillChilblain(EntityManager em, EntityCommandBuffer buffer, Entity caster)
    {
        Entity target;
        var cost = em.GetComponentData<Cost>(caster);
        int team = em.GetSharedComponentData<Team>(caster).value;
        if (ChooseBestTarget(out target, team * -1, new EntityQueryDesc { None = new ComponentType[] { typeof(ChilBlain) }, All = new ComponentType[] { typeof(Unit), typeof(Cost), typeof(HP) } }, cost.value))
        {
            buffer.AddComponent<ChilBlain>(target, new ChilBlain() { });
            var pos = em.GetComponentData<BoardPosition>(target);
            animationManager.instance.CreateUniqueAnimationSequence(new animationManager.particleAnimation() { name = "ChilBlain", position = new Vector3(pos.cell % 8, pos.cell / 8, -4), parent = GameObject.Find("Board").transform, scale = new Vector3(2, 4, 1) });
            animationManager.instance.CreateAnimationHandler("ChilBlain");
            animationManager.instance.TryPlayUniqueAnimation("ChilBlain");
        }
    }
    public void SkillHook(EntityManager em, EntityCommandBuffer buffer, Entity caster)
    {
        var pos = em.GetComponentData<BoardPosition>(caster);
        EntityQueryDesc freecell = new EntityQueryDesc() { None = new ComponentType[] { typeof(Occupied) }, All = new ComponentType[] { typeof(Cell) } };
        EntityQuery enemyQuery = em.CreateEntityQuery(typeof(Unit), typeof(BoardPosition), typeof(Team));
        enemyQuery.SetFilter(new Team() { value = em.GetSharedComponentData<Team>(caster).value * -1 });
        var enemyQueryPositionArr = enemyQuery.ToComponentDataArray<BoardPosition>(Allocator.TempJob);
        var enemyQueryArr = enemyQuery.ToEntityArray(Allocator.TempJob);
        EntityQuery freecellquery = em.CreateEntityQuery(freecell);
        var freeCellQueryArr = freecellquery.ToComponentDataArray<Cell>(Allocator.TempJob);
        var freeCellQueryEntity = freecellquery.ToEntityArray(Allocator.TempJob);
        List<Entity> hookCells = new List<Entity>();
        for (int i = 0; i < freecellquery.CalculateLength(); i++)
        {
            if (MovePatterns.PawnMover(freeCellQueryArr[i].number, pos.cell))
            {
                hookCells.Add(freeCellQueryEntity[i]);
            }
        }
        bool isHooked = false;
        foreach (var hookCell in hookCells)
        {
            if (!isHooked)
            {
                for (int i = 0; i < enemyQuery.CalculateLength(); i++)
                {
                    if (MovePatterns.TowerMover(em.GetComponentData<Cell>(hookCell).number, enemyQueryPositionArr[i].cell) && MovePatterns.TowerMover(pos.cell, enemyQueryPositionArr[i].cell))
                    {
                        isHooked = true;

                        //Entities.WithAll<Occupied, Cell>().ForEach((Entity cellcaster, ref Cell cell) => { if (cell.number == enemyQueryPositionArr[i].cell) { buffer.RemoveComponent<Occupied>(cellcaster); } });

                        animationManager.instance.DestroyMassAnimationInstance(new animationManager.particleAnimation() { name = "UnitCircle", position = new Vector3(enemyQueryPositionArr[i].cell % 8 * 2.25f, enemyQueryPositionArr[i].cell / 8 * 2.25f, -30) });
                        buffer.SetComponent(caster, new BoardPosition() { cell = em.GetComponentData<Cell>(hookCell).number });
                        buffer.AddComponent(hookCell, new Occupied() { });
                        buffer.SetComponent(caster, new Translation() { Value = new float3(em.GetComponentData<Cell>(hookCell).number % 8 * 2.25f, em.GetComponentData<Cell>(hookCell).number / 8 * 2.25f, 0.4f) });
                        animationManager.instance.CreateMassAnimationName(new animationManager.particleAnimation() { name = "UnitCircle", position = new Vector3(em.GetComponentData<Cell>(hookCell).number % 8 * 2.25f, em.GetComponentData<Cell>(hookCell).number / 8 * 2.25f, -30) }, em.GetSharedComponentData<Team>(caster).value);
                    }
                }
            }
        }
        freecellquery.Dispose();
        freeCellQueryEntity.Dispose();
        enemyQueryArr.Dispose();
        enemyQueryPositionArr.Dispose();
        enemyQuery.Dispose();
        freecellquery.Dispose();
    }
    public void SkillAmoureux(EntityManager em, EntityCommandBuffer buffer, Entity caster)
    {
        Entity target;
        var cost = em.GetComponentData<Cost>(caster);
        EntityQueryDesc eqd = new EntityQueryDesc { None = new ComponentType[] { typeof(Amoure)}, All = new ComponentType[] { typeof(Unit), typeof(Cost), typeof(HP) } };
        if (ChooseBestTarget(out target, em.GetSharedComponentData<Team>(caster).value * -1, eqd, cost.value))
        {
            var attack = em.GetComponentData<Attack>(target);
            buffer.AddComponent<Amoure>(target, new Amoure() { indexM = em.GetComponentData<Move>(target).index, indexA = attack.index });
            buffer.SetComponent<Attack>(target, new Attack() { index = (int)AttackPatterns.attackType.ZeroAttack, amountOfCubes = attack .amountOfCubes, typeOfCubes = attack .typeOfCubes, effect = attack .effect});
            buffer.SetComponent<Move>(target, new Move() { index = 6 });
            var pos = em.GetComponentData<BoardPosition>(target);
            animationManager.instance.CreateUniqueAnimationSequence(new animationManager.particleAnimation() { name = "Amoreux", position = new Vector3(pos.cell % 8, pos.cell / 8, -4), parent = GameObject.Find("Board").transform, scale = new Vector3(2, 4, 1) });
            animationManager.instance.CreateAnimationHandler("Amoreux");
            animationManager.instance.TryPlayUniqueAnimation("Amoreux");
        }
        buffer.AddComponent<Cooldown>(caster, new Cooldown() { duration = 2 });
    }
    public void SkillFireCover(EntityManager em, EntityCommandBuffer buffer, Entity caster)
    {
        Entity target;
        var attack = em.GetComponentData<Attack>(caster);
        var lvl = em.GetComponentData<Lvl>(caster);
        EntityQueryDesc eqd = new EntityQueryDesc { None = new ComponentType[] { typeof(FireShield) }, All = new ComponentType[] { typeof(Unit), typeof(Cost), typeof(HP) } };
        if (ChooseBestTarget(out target, em.GetSharedComponentData<Team>(caster).value, eqd))
        {
            buffer.AddComponent<FireShield>(target, new FireShield() { damage = attack.typeOfCubes * lvl.value });
            var pos = em.GetComponentData<BoardPosition>(target);
            animationManager.instance.CreateUniqueAnimationSequence(new animationManager.particleAnimation() { name = "FireShield", position = new Vector3(pos.cell % 8, pos.cell / 8, -4), parent = GameObject.Find("Board").transform, scale = new Vector3(2, 4, 1) });
            animationManager.instance.CreateAnimationHandler("FireShield");
            animationManager.instance.TryPlayUniqueAnimation("FireShield");
        }
    }
    public void SkillGrandmaCakes(EntityManager em, EntityCommandBuffer buffer, Entity caster)
    {
        var lvl = em.GetComponentData<Lvl>(caster);
        int heal = lvl.value;
        EntityQuery allUnitsQuery = em.CreateEntityQuery(typeof(Unit));
        var allUnits = allUnitsQuery.ToEntityArray(Allocator.TempJob);
        List<animationManager.particleAnimation> anims = new List<animationManager.particleAnimation>();
        foreach (var unit in allUnits)
        {
            var hp = em.GetComponentData<HP>(unit);
            var pos = em.GetComponentData<BoardPosition>(unit);
            buffer.SetComponent<HP>(unit, new HP() { startValue = hp.startValue, currentValue = hp.currentValue+heal*3>hp.startValue ? hp.startValue : hp.currentValue+heal*3});            
            anims.Add(new animationManager.particleAnimation() { name = "EmojiHappy", position = new Vector3(pos.cell % 8, pos.cell / 8, -4), parent = GameObject.Find("Board").transform, scale = new Vector3(2, 4, 1) });
            if (SquadsManagement.instance.allCards.ContainsKey(em.GetComponentData<Id>(unit).value))
            {
                SquadsManagement.instance.allCards[em.GetComponentData<Id>(unit).value].GetComponent<UnitCardFight>().UpdateData(unit);
            }
        }
        animationManager.instance.CreateMassAnimationName(anims.ToArray());
        animationManager.instance.CreateAnimationHandler("EmojiHappy",true);
        animationManager.instance.PlayMassAnimation(anims.ToArray());
        allUnits.Dispose();
    }
    public void SkillAngelBlessing(EntityManager em, EntityCommandBuffer buffer, Entity caster)
    {
        Entity target;
        EntityQueryDesc eqd = new EntityQueryDesc { None = new ComponentType[] { typeof(Bless) }, All = new ComponentType[] { typeof(Unit), typeof(Cost), typeof(HP), typeof(Team), typeof(Lvl) } };
        if (ChooseBestTarget(out target, em.GetSharedComponentData<Team>(caster).value, eqd))
        {
            buffer.AddComponent<Bless>(target, new Bless() { duration = 50 });
            var pos = em.GetComponentData<BoardPosition>(target);
            animationManager.instance.CreateUniqueAnimationSequence(new animationManager.particleAnimation() { name = "CasualBuff", position = new Vector3(pos.cell % 8, pos.cell / 8, -4), parent = GameObject.Find("Board").transform, scale = new Vector3(2, 4, 1) });
            animationManager.instance.CreateAnimationHandler("CasualBuff");
            animationManager.instance.TryPlayUniqueAnimation("CasualBuff");
        }
        else
        {
            buffer.AddComponent<Action>(caster, new Action() { });
        }
    }
    public void SkillWaterPool(EntityManager em, EntityCommandBuffer buffer, Entity caster)
    {
        EntityQueryDesc eqd = new EntityQueryDesc() { None = new ComponentType[] { typeof(Water) }, All = new ComponentType[] { typeof(Cell) } };
        EntityQuery waterCellsQuery = em.CreateEntityQuery(eqd);
        if (waterCellsQuery.CalculateLength() > 1)
        {
            var waterCellsEntity = waterCellsQuery.ToEntityArray(Allocator.TempJob);
            Entity firstCell, secondCell;
            firstCell = waterCellsEntity[UnityEngine.Random.Range(0, waterCellsQuery.CalculateLength())];
            secondCell = firstCell;
            while (secondCell != firstCell)
            {
                secondCell = waterCellsEntity[UnityEngine.Random.Range(0, waterCellsQuery.CalculateLength())];
            }
            buffer.AddComponent<Water>(firstCell, new Water() { });
            buffer.AddComponent<Water>(secondCell, new Water() { });
            waterCellsEntity.Dispose();
        }
        waterCellsQuery.Dispose();
    }
    public void SkillElectricTrap(EntityManager em, EntityCommandBuffer buffer, Entity caster)
    {
        var attack = em.GetComponentData<Attack>(caster);
        var lvl = em.GetComponentData<Lvl>(caster);
        EntityQueryDesc eqd = new EntityQueryDesc() { None = new ComponentType[] { typeof(ElectricTrap) }, All = new ComponentType[] { typeof(Cell) } };
        EntityQuery electricCellsQuery = em.CreateEntityQuery(eqd);
        if (electricCellsQuery.CalculateLength() > 1)
        {
            var electricCellsEntity = electricCellsQuery.ToEntityArray(Allocator.TempJob);
            Entity firstCell;
            firstCell = electricCellsEntity[UnityEngine.Random.Range(0, electricCellsQuery.CalculateLength())];
            buffer.AddComponent<ElectricTrap>(firstCell, new ElectricTrap() { damage = attack.typeOfCubes * lvl.value });
            electricCellsEntity.Dispose();
        }
        electricCellsQuery.Dispose();
    }
    public void SkillNineTails(EntityManager em, EntityCommandBuffer buffer, Entity caster)
    {
        var attack = em.GetComponentData<Attack>(caster);
        buffer.SetComponent<Attack>(caster, new Attack() { amountOfCubes = 9, index = attack.index, typeOfCubes = attack.typeOfCubes });
        var pos = em.GetComponentData<BoardPosition>(caster);
        animationManager.instance.CreateUniqueAnimationSequence(new animationManager.particleAnimation() { name = "CasualBuff", position = new Vector3(pos.cell % 8, pos.cell / 8, -4), parent = GameObject.Find("Board").transform, scale = new Vector3(2, 4, 1) });
        animationManager.instance.CreateAnimationHandler("CasualBuff");
        animationManager.instance.TryPlayUniqueAnimation("CasualBuff");
        if (SquadsManagement.instance.allCards.ContainsKey(em.GetComponentData<Id>(caster).value))
        {
            SquadsManagement.instance.allCards[em.GetComponentData<Id>(caster).value].GetComponent<UnitCardFight>().UpdateData(caster);
        }
    }
    public void SkillPowerfullWings(EntityManager em, EntityCommandBuffer buffer, Entity caster)
    {
        buffer.SetComponent<Move>(caster, new Move() { index = 6 });
        var pos = em.GetComponentData<BoardPosition>(caster);
        animationManager.instance.CreateUniqueAnimationSequence(new animationManager.particleAnimation() { name = "CasualBuff", position = new Vector3(pos.cell % 8, pos.cell / 8, -4), parent = GameObject.Find("Board").transform, scale = new Vector3(2, 4, 1) });
        animationManager.instance.CreateAnimationHandler("CasualBuff");
        animationManager.instance.TryPlayUniqueAnimation("CasualBuff");
    }
    public void SkillMythicalPatron(EntityManager em, EntityCommandBuffer buffer, Entity caster)
    {
        var mp = em.GetComponentData<MythicalPatron>(caster);
        if (mp.state == 1)
        {
            var attack = em.GetComponentData<Attack>(caster);
            var hp = em.GetComponentData<HP>(caster);
            buffer.SetComponent<Attack>(caster, new Attack() { amountOfCubes = mp.dessaDiceNumber, typeOfCubes = mp.dessaDiceType, index = mp.dessaIndex });
            buffer.SetComponent<MythicalPatron>(caster, new MythicalPatron() { state = 0, dessaDiceNumber = attack.amountOfCubes, dessaDiceType = attack.typeOfCubes, dessa_hp = hp.currentValue, dessaIndex = attack.index });
            buffer.SetComponent<HP>(caster, new HP() { startValue = hp.startValue, currentValue = mp.dessa_hp });            
        }
    }
    public void SkillSunStrike(EntityManager em, EntityCommandBuffer buffer, Entity caster)
    {
        Entity target;
        var attack = em.GetComponentData<Attack>(caster);
        var lvl = em.GetComponentData<Lvl>(caster);
        int team = em.GetSharedComponentData<Team>(caster).value;
        if (ChooseBestTarget(out target, team * -1))
        {
            int startHp = em.GetComponentData<HP>(target).startValue;
            int curHp = em.GetComponentData<HP>(target).currentValue;
            var pos = em.GetComponentData<BoardPosition>(target);
            int damage = DiceSystem.RollBunchOfDice(attack.amountOfCubes, attack.typeOfCubes) * lvl.value;
            animationManager.instance.CreateUniqueAnimationSequence(new animationManager.particleAnimation() { name = "Sunstrike", position = new Vector3(pos.cell % 8, pos.cell / 8, -4), parent = GameObject.Find("Board").transform, scale = new Vector3(2, 4, 1) });
            animationManager.instance.CreateAnimationHandler("Sunstrike");
            animationManager.instance.TryPlayUniqueAnimation("Sunstrike");
            buffer.SetComponent<HP>(target, new HP() { startValue = startHp, currentValue = curHp - damage});
            if (SquadsManagement.instance.allCards.ContainsKey(em.GetComponentData<Id>(target).value))
            {
                SquadsManagement.instance.allCards[em.GetComponentData<Id>(target).value].GetComponent<UnitCardFight>().UpdateData(target,damage);
            }
        }
    }
    public void SkillGrowingTurtle(EntityManager em, EntityCommandBuffer buffer, Entity caster)
    {
        var hp = em.GetComponentData<HP>(caster);
        var lvl = em.GetComponentData<Lvl>(caster);
        buffer.SetComponent(caster, new HP() { currentValue = hp.currentValue, startValue = hp.startValue + lvl.value * 2 });
        var pos = em.GetComponentData<BoardPosition>(caster);
        animationManager.instance.CreateUniqueAnimationSequence(new animationManager.particleAnimation() { name = "CasualBuff", position = new Vector3(pos.cell % 8, pos.cell / 8, -4), parent = GameObject.Find("Board").transform, scale = new Vector3(2, 4, 1) });
        animationManager.instance.CreateAnimationHandler("CasualBuff");
        animationManager.instance.TryPlayUniqueAnimation("CasualBuff");
    }
    public void SkillXoerCurse(EntityManager em, EntityCommandBuffer buffer, Entity caster)
    {
        Entity target;
        var lvl = em.GetComponentData<Lvl>(caster);
        if (ChooseBestTarget(out target, em.GetSharedComponentData<Team>(caster).value * -1, new EntityQueryDesc { None = new ComponentType[] { typeof(XoerCurse) }, All = new ComponentType[] { typeof(Unit), typeof(Lvl), typeof(HP), typeof(Team), typeof(BoardPosition) } }))
        {
            var pos = em.GetComponentData<BoardPosition>(target);
            buffer.AddComponent
            (
                target,
                new XoerCurse() { duration = lvl.value / 2 }
            );
            if (em.HasComponent<Curse>(target))
            {
                buffer.AddComponent
                (
                   target,
                    new Curse() { duration = 50}
                );
            }
            animationManager.instance.CreateUniqueAnimationSequence(new animationManager.particleAnimation() { name = "Curse", position = new Vector3(pos.cell % 8, pos.cell / 8, -4), parent = GameObject.Find("Board").transform, scale = new Vector3(1, 2, 1) });
            animationManager.instance.CreateAnimationHandler("Curse");
            animationManager.instance.TryPlayUniqueAnimation("Curse");
        }
    }
    public void SkillFeastOfHeroes(EntityManager em, EntityCommandBuffer buffer, Entity caster)
    {
        EntityQuery allies = em.CreateEntityQuery(typeof(Unit), typeof(Attack), typeof(Team), typeof(BoardPosition), typeof(Id));
        allies.SetFilter(new Team() { value = em.GetSharedComponentData<Team>(caster).value });
        var alliesArr = allies.ToComponentDataArray<Attack>(Allocator.TempJob);
        var alliesEArr = allies.ToEntityArray(Allocator.TempJob);
        List<animationManager.particleAnimation> anims = new List<animationManager.particleAnimation>();
        for (int i = 0; i < allies.CalculateLength(); i++)
        {
            var pos = em.GetComponentData<BoardPosition>(alliesEArr[i]);
            int amount = em.GetComponentData<Attack>(alliesEArr[i]).amountOfCubes;
            int type = em.GetComponentData<Attack>(alliesEArr[i]).typeOfCubes;
            int index = em.GetComponentData<Attack>(alliesEArr[i]).index;
            buffer.SetComponent<Attack>(alliesEArr[i], new Attack() { typeOfCubes = type, index = index, amountOfCubes = amount + 1 });
            anims.Add(new animationManager.particleAnimation() { name = "CasualBuff", position = new Vector3(pos.cell % 8, pos.cell / 8, -4), parent = GameObject.Find("Board").transform, scale = new Vector3(2, 4, 1) });            
            if (SquadsManagement.instance.allCards.ContainsKey(em.GetComponentData<Id>(alliesEArr[i]).value))
            {
                SquadsManagement.instance.allCards[em.GetComponentData<Id>(alliesEArr[i]).value].GetComponent<UnitCardFight>().UpdateData(alliesEArr[i]);
            }
        }
        animationManager.instance.CreateMassAnimationName(anims.ToArray());
        animationManager.instance.CreateAnimationHandler("CasualBuff", true);
        animationManager.instance.PlayMassAnimation(anims.ToArray());
        alliesEArr.Dispose();
        alliesArr.Dispose();
    }
    public void SkillMagicMissiles(EntityManager em, EntityCommandBuffer buffer, Entity caster)
    {
        var attack = em.GetComponentData<Attack>(caster);
        var lvl = em.GetComponentData<Lvl>(caster);
        List<Entity> targets = new List<Entity>();
        EntityQuery enemiesQ = em.CreateEntityQuery(typeof(Unit), typeof(HP), typeof(BoardPosition), typeof(Id), typeof(Attack), typeof(Team));
        enemiesQ.SetFilter(new Team() { value = em.GetSharedComponentData<Team>(caster).value * -1 });
        var enemiesE = enemiesQ.ToEntityArray(Allocator.TempJob);
        Entity targetE;
        List<animationManager.particleAnimation> anims = new List<animationManager.particleAnimation>();
        while (targets.Count < 3 && enemiesQ.CalculateLength() - targets.Count > 0)
        {
            targetE = enemiesE[UnityEngine.Random.Range(0, enemiesQ.CalculateLength())];
            if (!targets.Contains(targetE))
            {
                targets.Add(targetE);
            }
        }
        foreach (var target in targets)
        {
            int startHp = em.GetComponentData<HP>(target).startValue;
            int curHp = em.GetComponentData<HP>(target).currentValue;
            int damage = DiceSystem.RollBunchOfDice(attack.amountOfCubes, attack.typeOfCubes) * lvl.value / 2;
            var pos = em.GetComponentData<BoardPosition>(target);
            buffer.SetComponent<HP>(target, new HP() { startValue = startHp, currentValue = curHp - damage});
            anims.Add(new animationManager.particleAnimation() { name = "Lightning", position = new Vector3(pos.cell % 8, pos.cell / 8, -4), parent = GameObject.Find("Board").transform, scale = new Vector3(2, 4, 1) });
            if (SquadsManagement.instance.allCards.ContainsKey(em.GetComponentData<Id>(target).value))
            {
                SquadsManagement.instance.allCards[em.GetComponentData<Id>(target).value].GetComponent<UnitCardFight>().UpdateData(target, damage);
            }
        }
        enemiesE.Dispose();
        animationManager.instance.CreateMassAnimationName(anims.ToArray());
        animationManager.instance.CreateAnimationHandler("Lightning", true);
        animationManager.instance.PlayMassAnimation(anims.ToArray());
    }
    public void SkillWeepingWillow(EntityManager em, EntityCommandBuffer buffer, Entity caster)
    {
        var lvl = em.GetComponentData<Lvl>(caster);
        var hp = em.GetComponentData<HP>(caster);
        var a = em.GetComponentData<Attack>(caster);
        buffer.SetComponent(caster, new HP() { currentValue = hp.currentValue - lvl.value * 2, startValue = hp.startValue });
        buffer.SetComponent(caster, new Attack() { index = a.index, amountOfCubes = a.amountOfCubes + lvl.value / 10, typeOfCubes = a.typeOfCubes });
        var pos = em.GetComponentData<BoardPosition>(caster);
        animationManager.instance.CreateUniqueAnimationSequence(new animationManager.particleAnimation() { name = "CasualBuff", position = new Vector3(pos.cell % 8, pos.cell / 8, -4), parent = GameObject.Find("Board").transform, scale = new Vector3(2, 4, 1) });
        animationManager.instance.CreateAnimationHandler("CasualBuff");
        animationManager.instance.TryPlayUniqueAnimation("CasualBuff");
        if (SquadsManagement.instance.allCards.ContainsKey(em.GetComponentData<Id>(caster).value))
        {
            SquadsManagement.instance.allCards[em.GetComponentData<Id>(caster).value].GetComponent<UnitCardFight>().UpdateData(caster);
        }
    }
    public void SkillClockworkEngine(EntityManager em, EntityCommandBuffer buffer, Entity caster)
    {
        var lvl = em.GetComponentData<Lvl>(caster);
        var hp = em.GetComponentData<HP>(caster);
        var a = em.GetComponentData<Attack>(caster);
        buffer.SetComponent(caster, new HP() { currentValue = hp.currentValue + lvl.value * 2, startValue = hp.startValue });
        buffer.SetComponent(caster, new Attack() { index = a.index, amountOfCubes = a.amountOfCubes + lvl.value / 5, typeOfCubes = 20 });
        buffer.AddComponent<Cooldown>(caster, new Cooldown() { duration = 4 });
        var pos = em.GetComponentData<BoardPosition>(caster);
        animationManager.instance.CreateUniqueAnimationSequence(new animationManager.particleAnimation() { name = "CasualBuff", position = new Vector3(pos.cell % 8, pos.cell / 8, -4), parent = GameObject.Find("Board").transform, scale = new Vector3(2, 4, 1) });
        animationManager.instance.CreateAnimationHandler("CasualBuff");
        animationManager.instance.TryPlayUniqueAnimation("CasualBuff");
        if (SquadsManagement.instance.allCards.ContainsKey(em.GetComponentData<Id>(caster).value))
        {
            SquadsManagement.instance.allCards[em.GetComponentData<Id>(caster).value].GetComponent<UnitCardFight>().UpdateData(caster);
        }
    }
    public void SkillQueenTouch(EntityManager em, EntityCommandBuffer buffer, Entity caster)
    {
        Entity target;
        if (ChooseBestTarget(out target, em.GetSharedComponentData<Team>(caster).value))
        {
            buffer.SetComponent<HP>
            (
                target,
                new HP() { currentValue = em.GetComponentData<HP>(target).startValue }
            );
            var pos = em.GetComponentData<BoardPosition>(target);
            animationManager.instance.CreateUniqueAnimationSequence(new animationManager.particleAnimation() { name = "HealOnce", position = new Vector3(pos.cell % 8, pos.cell / 8, -4), parent = GameObject.Find("Board").transform, scale = new Vector3(2, 4, 1) });
            animationManager.instance.CreateAnimationHandler("HealOnce");
            animationManager.instance.TryPlayUniqueAnimation("HealOnce");
            if (SquadsManagement.instance.allCards.ContainsKey(em.GetComponentData<Id>(target).value))
            {
                SquadsManagement.instance.allCards[em.GetComponentData<Id>(target).value].GetComponent<UnitCardFight>().UpdateData(target);
            }
        }
    }
    public void SkillWhirlpool(EntityManager em, EntityCommandBuffer buffer, Entity caster)
    {
        EntityQuery targets = em.CreateEntityQuery(typeof(Unit), typeof(Team), typeof(Move));
        targets.SetFilter(new Team() { value = em.GetSharedComponentData<Team>(caster).value * -1 });
        var targetsE = targets.ToEntityArray(Allocator.TempJob);
        var targetsM = targets.ToComponentDataArray<Move>(Allocator.TempJob);
        List<animationManager.particleAnimation> anims = new List<animationManager.particleAnimation>();
        for (int i = 0; i < targets.CalculateLength(); i++)
        {
            var pos = em.GetComponentData<BoardPosition>(targetsE[i]);
            buffer.SetComponent<Move>(targetsE[i], new Move() { index = 4 });
            anims.Add(new animationManager.particleAnimation() { name = "EmojiHappy", position = new Vector3(pos.cell % 8, pos.cell / 8, -4), parent = GameObject.Find("Board").transform, scale = new Vector3(2, 4, 1) });
        }
        animationManager.instance.CreateMassAnimationName(anims.ToArray());
        animationManager.instance.CreateAnimationHandler("EmojiHappy", true);
        animationManager.instance.PlayMassAnimation(anims.ToArray());
        targetsM.Dispose();
    }
    public void SkillStoner(EntityManager em, EntityCommandBuffer buffer, Entity caster)
    {
        Entity target;
        var cost = em.GetComponentData<Cost>(caster);
        int team = em.GetSharedComponentData<Team>(caster).value;
        if (ChooseBestTarget(out target, team * -1, new EntityQueryDesc { None = new ComponentType[] { typeof(Stone) }, All = new ComponentType[] { typeof(Unit), typeof(Cost), typeof(HP) } }, cost.value))
        {
            buffer.AddComponent<Stone>(target, new Stone() { });
            var pos = em.GetComponentData<BoardPosition>(target);
            animationManager.instance.CreateUniqueAnimationSequence(new animationManager.particleAnimation() { name = "Curse", position = new Vector3(pos.cell % 8, pos.cell / 8, -4), parent = GameObject.Find("Board").transform, scale = new Vector3(2, 4, 1) });
            animationManager.instance.CreateAnimationHandler("Curse");
            animationManager.instance.TryPlayUniqueAnimation("Curse");
        }
    }
    public void SkillDevour(EntityManager em, EntityCommandBuffer buffer, Entity caster)
    {
        var cost = em.GetComponentData<Cost>(caster);
        Entity target;
        int team = em.GetSharedComponentData<Team>(caster).value;
        if (ChooseBestTarget(out target, team * -1, null, cost.value / 2))
        {
            KillUnit(target, buffer);
            buffer.AddComponent<Cooldown>(caster, new Cooldown() { duration = 4 });
        }
    }
    public void SkillCurseRelease(EntityManager em, EntityCommandBuffer buffer, Entity caster)
    {
        Entity target;
        var lvl = em.GetComponentData<Lvl>(caster);
        var cost = em.GetComponentData<Cost>(caster);
        int team = em.GetSharedComponentData<Team>(caster).value;
        if (ChooseBestTarget(out target, team * -1, new EntityQueryDesc { All = new ComponentType[] { typeof(Unit), typeof(Cost), typeof(HP), typeof(Curse) } }, cost.value))
        {
            int startHp = em.GetComponentData<HP>(target).startValue;
            int curHp = em.GetComponentData<HP>(target).currentValue;
            buffer.RemoveComponent<Curse>(target);
            buffer.SetComponent<HP>(target, new HP() { startValue = startHp, currentValue = curHp - lvl.value * 10 });
            var pos = em.GetComponentData<BoardPosition>(target);
            animationManager.instance.CreateUniqueAnimationSequence(new animationManager.particleAnimation() { name = "Curse", position = new Vector3(pos.cell % 8, pos.cell / 8, -4), parent = GameObject.Find("Board").transform, scale = new Vector3(2, 4, 1) });
            animationManager.instance.CreateAnimationHandler("Curse");
            animationManager.instance.TryPlayUniqueAnimation("Curse");
        }
       
    }
    public void SkillAbsorbing(EntityManager em, EntityCommandBuffer buffer, Entity caster)
    {
        var cost = em.GetComponentData<Cost>(caster);
        var hp = em.GetComponentData<HP>(caster);
        var a = em.GetComponentData<Attack>(caster);
        Entity target;
        int team = em.GetSharedComponentData<Team>(caster).value;
        if (ChooseBestTarget(out target, team * -1, null, cost.value / 3 * 2))
        {
            int targetHp = em.GetComponentData<HP>(target).startValue;
            int diceNumber = em.GetComponentData<Attack>(target).amountOfCubes;
            buffer.SetComponent<HP>(caster, new HP() { startValue = hp.startValue, currentValue = hp.currentValue + targetHp / 10 });
            buffer.SetComponent<Attack>(caster, new Attack() { index = a.index, amountOfCubes = a.amountOfCubes + diceNumber / 2, typeOfCubes = a.typeOfCubes });
            KillUnit(target, buffer);
            buffer.AddComponent<Cooldown>(caster, new Cooldown() { duration = 3 });
            if (SquadsManagement.instance.allCards.ContainsKey(em.GetComponentData<Id>(caster).value))
            {
                SquadsManagement.instance.allCards[em.GetComponentData<Id>(caster).value].GetComponent<UnitCardFight>().UpdateData(caster);
            }
        }
    }
    public void SkillDragonBlessing(EntityManager em, EntityCommandBuffer buffer, Entity caster)
    {
        int team = em.GetSharedComponentData<Team>(caster).value;
        EntityQueryDesc eqd = new EntityQueryDesc() { None = new ComponentType[] { typeof(DragonBless) }, All = new ComponentType[] { typeof(Unit), typeof(Cost), typeof(HP) } };
        EntityQuery allies = em.CreateEntityQuery(eqd);
        allies.SetFilter(new Team() { value = em.GetSharedComponentData<Team>(caster).value });
        var alliesE = allies.ToEntityArray(Allocator.TempJob);
        List<animationManager.particleAnimation> anims = new List<animationManager.particleAnimation>();
        for (int i = 0; i < allies.CalculateLength(); i++)
        {
            buffer.AddComponent<DragonBless>(alliesE[i], new DragonBless() { });
            if (!em.HasComponent<Bless>(alliesE[i]))
            {
                buffer.AddComponent<Bless>(alliesE[i], new Bless() { });
            }
            var pos = em.GetComponentData<BoardPosition>(alliesE[i]);
            anims.Add(new animationManager.particleAnimation() { name = "CasualBuff", position = new Vector3(pos.cell % 8, pos.cell / 8, -4), parent = GameObject.Find("Board").transform, scale = new Vector3(2, 4, 1) });
        }
        animationManager.instance.CreateMassAnimationName(anims.ToArray());
        animationManager.instance.CreateAnimationHandler("CasualBuff", true);
        animationManager.instance.PlayMassAnimation(anims.ToArray());
    }
    public void SkillAbsorbtion(EntityManager em, EntityCommandBuffer buffer, Entity caster)
    {
        Entity target;
        int team = em.GetSharedComponentData<Team>(caster).value;
        if (ChooseBestTarget(out target, team * -1))
        {
            KillUnit(target, buffer);
        }
    }



    private   bool ChooseBestTarget(out Entity targetEntity, int team = -1, EntityQueryDesc nones = null, int cost = int.MaxValue)
    {
        EntityManager em = World.Active.EntityManager;
        EntityQuery units;
        if (nones == null)
        {
            units = em.CreateEntityQuery(typeof(Unit), typeof(Lvl), typeof(HP), typeof(Team));
        }
        else
        {
            units = em.CreateEntityQuery(nones);
        }
        units.SetFilter(new Team() { value = team });
        var unitsE = units.ToEntityArray(Allocator.TempJob);
        var unitsHP = units.ToComponentDataArray<HP>(Allocator.TempJob);
        var unitsCost = units.ToComponentDataArray<Lvl>(Allocator.TempJob);
        int index = -1;
        float q = 0;
        for (int i = 0; i < unitsE.Length; i++)
        {
            if (unitsHP[i].currentValue > 0)
            {
                if ((unitsCost[i].value *  ((float)unitsHP[i].startValue / (float)unitsHP[i].currentValue)) > q)
                {
                    if (unitsCost[i].value <= cost)
                    {
                        index = i;
                        q = unitsCost[i].value * ((float)unitsHP[i].startValue / (float)unitsHP[i].currentValue);
                    }
                }
            }
        }
        if (index != -1)
        {
            targetEntity = unitsE[index];
            unitsE.Dispose();
            unitsHP.Dispose();
            unitsCost.Dispose();
            return true;
        }
        else
        {
            targetEntity = new Entity();
            unitsE.Dispose();
            unitsHP.Dispose();
            unitsCost.Dispose();
            return false;
        }
    }
    private   void KillUnit(Entity unit, EntityCommandBuffer buffer)
    {
        EntityManager em = World.Active.EntityManager;
        EntityQuery thanatosQ = em.CreateEntityQuery(typeof(SoulEater), typeof(HP), typeof(Attack));
        if (thanatosQ.CalculateLength() > 0)
        {
            var thanatosA = thanatosQ.ToEntityArray(Allocator.TempJob);
            foreach (var thanatos in thanatosA)
            {
                var thanatosAttack = em.GetComponentData<Attack>(thanatos);
                var thanatosHP = em.GetComponentData<HP>(thanatos);
                buffer.SetComponent<Attack>(thanatos, new Attack() { amountOfCubes = thanatosAttack.amountOfCubes + 1, index = thanatosAttack.index, typeOfCubes = thanatosAttack.typeOfCubes });
                buffer.SetComponent<HP>(thanatos, new HP() { startValue = thanatosHP.startValue, currentValue = thanatosHP.currentValue + 8 });
            }
            thanatosA.Dispose();
            thanatosQ.Dispose();
        }
        buffer.RemoveComponent<Unit>(unit);
        buffer.RemoveComponent<Action>(unit);
        buffer.AddComponent(unit, new View() { frame = 0, state = 0 });
        buffer.AddComponent(unit, new Dead() { });
        buffer.AddComponent(unit, new UnitAnimation() { });
    }
    private   bool ChooseBestDeadTarget(out Entity targetEntity, int team = 1)
    {
        EntityManager em = World.Active.EntityManager;
        EntityQuery units;
        units = em.CreateEntityQuery(typeof(Unit), typeof(Cost), typeof(HP));
        units.SetFilter(new Team() { value = team });
        var unitsE = units.ToEntityArray(Allocator.TempJob);
        var unitsHP = units.ToComponentDataArray<HP>(Allocator.TempJob);
        var unitsCost = units.ToComponentDataArray<Cost>(Allocator.TempJob);
        int index = -1;
        float q = 0;
        for (int i = 0; i < unitsE.Length; i++)
        {
            if (unitsCost[i].value / unitsHP[i].currentValue > q)
            {
                index = i;
                q = unitsCost[i].value / unitsHP[i].currentValue;
            }
        }
        unitsE.Dispose();
        unitsHP.Dispose();
        unitsCost.Dispose();
        if (index != -1)
        {
            targetEntity = unitsE[index];
            return true;
        }
        else
        {
            targetEntity = new Entity();
            return false;
        }
    }
}