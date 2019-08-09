using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateBefore(typeof(InitiativeSystem))]
public class RenderSystem : ComponentSystem
{

    public List<Material> DeathAnimation;
    string[] effectNames;
    List<string> unitNames;
    TextAsset effectsTxt;
    Material black, white, activeUnit, attackedUnit;
    List<Matrix4x4> whiteList, blackList;
    List<List<Material>> effectList;
    List<Material> figureList;
    EntityQuery attackEffects;    
    protected override void OnCreate()
    {
        effectList = new List<List<Material>>();
        DeathAnimation = new List<Material>();
        figureList = new List<Material>();
        blackList = new List<Matrix4x4>();
        whiteList = new List<Matrix4x4>();
        black = LoadMaterial("AsphaltCell");
        white = LoadMaterial("LeavesCell");
        activeUnit = LoadMaterial("ActiveUnit");
        attackedUnit = LoadMaterial("AttackedUnit");        
        attackedUnit.SetColor(0,new Color(Color.white.r, Color.white.g, Color.white.b, 0));
        LoadDeathAnimation();
        CreateLists();
        this.Enabled = false;        
    }

    private void CreateLists()
    {
        for (int i = 0; i < 64; i++)
        {
            Vector3 v = new Vector3(2.25f*(i%8),2.25f*(i/8),1.5f);
            Matrix4x4 m = new Matrix4x4();
            m.SetTRS(v, Quaternion.identity, Vector3.one);
            if (((i%8+i/8)%2)==0)
            {
                blackList.Add(m);
            }
            else
            {
                whiteList.Add(m);
            }
        }
    }
    protected override void OnUpdate()
    {
        if (Spawner.instance.unitNames.Count!=figureList.Count)
        {
            LoadCharacters();
        }
        EntityQuery closeUpCheck = GetEntityQuery(typeof(WaitForAttackAnimEnd));
        if (closeUpCheck.CalculateLength()==0)
        {
            Graphics.DrawMeshInstanced(CreateQuadMesh(2.25f, 2.25f), 0, black, blackList, null, UnityEngine.Rendering.ShadowCastingMode.Off, false, 0, Camera.main);
            Graphics.DrawMeshInstanced(CreateQuadMesh(2.25f, 2.25f), 0, white, whiteList, null, UnityEngine.Rendering.ShadowCastingMode.Off, false, 0, Camera.main);
            Entities.WithAll<Unit>().WithNone<View>().ForEach /*DRAW FIGURES ON BOARD*/
                (
                    (ref Translation trans, ref UnitType type) =>
                    {
                       Graphics.DrawMesh(CreateQuadMesh(1, 1), Matrix4x4.TRS(trans.Value, Quaternion.identity,new Vector3(2.25f,2.25f,1)), figureList[type.index], 1);
                    }
                );

            Entities.WithAll<UnitAnimation>().ForEach
                (
                    (Entity entity, ref Translation position)=>
                    {
                        var pos = position;
                        Entities.WithAll<Unit, Target>().ForEach /*Draw target marker*/
                            (
                             (Entity id, ref Translation trans) =>
                             {
                                 if (attackedUnit.color.a<1)
                                 {
                                     attackedUnit.color = new Color(Color.white.r, Color.white.g, Color.white.b, attackedUnit.color.a + 0.5f*Time.deltaTime);
                                 }
                                 else
                                 {
                                     SquadsManagement.instance.DisplayDamage();
                                     attackedUnit.color = new Color(Color.white.r, Color.white.g, Color.white.b, 0f);
                                     PostUpdateCommands.RemoveComponent<UnitAnimation>(entity);
                                     PostUpdateCommands.AddComponent<WaitForAttackAnimEnd>(entity, new WaitForAttackAnimEnd() { });
                                     animationManager.instance.DestroyMassAnimationInstance(new animationManager.particleAnimation() { name = "UnitCircle", position = new Vector3(pos.Value.x, pos.Value.y, -30) });
                                     animationManager.instance.CreateMassAnimationName(new animationManager.particleAnimation() { name = "UnitCircle", position = new Vector3(pos.Value.x, pos.Value.y, -30) }, EntityManager.GetSharedComponentData<Team>(entity).value);  
                                 }
                                 Graphics.DrawMesh(CreateQuadMesh(1, 1), Matrix4x4.TRS(new Vector3(trans.Value.x, trans.Value.y, trans.Value.z - 10), Quaternion.identity, new Vector3(2.25f, 2.25f, 1f)), attackedUnit, 1);
                             }
                            );
                    }
                );
            Entities.WithAll<Dead, UnitAnimation>().ForEach(
                    (Entity id, ref View view, ref Translation trans) =>
                    {
                        if (view.frame == DeathAnimation.Count - 1)
                        {
                            Graphics.DrawMesh(CreateQuadMesh(2.25f, 2.25f), trans.Value, Quaternion.identity, DeathAnimation[view.frame], 15);
                            PostUpdateCommands.DestroyEntity(id);
                        }
                        else
                        {
                            PostUpdateCommands.SetComponent<View>(id, new View() { frame = view.frame + 1, state = 0 });
                            Graphics.DrawMesh(CreateQuadMesh(2.25f, 2.25f), trans.Value, Quaternion.identity, DeathAnimation[view.frame], 15);
                        }
                    }
                );
        }
        /*CLOSEUP ANIMATION CREATE*/
        else
        {
            Entities.WithAll<WaitForAttackAnimEnd>().ForEach
                (
                    (Entity attackerId) =>
                    {
                        Entities.WithAll<CloseUp, Target>().ForEach
                            (
                                (Entity id, ref View view, ref UnitType unitType, ref Translation translation) =>
                                {
                                    createCloseUp.CreateAnimation(unitType.index, translation.Value, -1);
                                    PostUpdateCommands.DestroyEntity(id);
                                }
                            );
                        Entities.WithNone<Target>().WithAll<CloseUp>().ForEach
                            (
                                (Entity id, ref View view, ref UnitType unitType, ref Translation translation) =>
                                {
                                    createCloseUp.CreateAnimation(unitType.index, translation.Value, 1);
                                    animationManager.instance.HideMassAnimation("UnitCircle");
                                    PostUpdateCommands.DestroyEntity(id);
                                }
                            );
                    }
                );
        }
    }

    private void LoadCharacters()
    {
        figureList.Clear();
        Texture2D texture;
        foreach(var name in Spawner.instance.unitNames)
        {
            texture = Resources.Load<Texture2D>("Units/fig/"+name);
            figureList.Add(new Material(Shader.Find("Sprites/Default")) { mainTexture = texture});
        }
    }
    private void LoadDeathAnimation()
    {
        int i = 1;
        Texture2D texture = Resources.Load<Texture2D>("Effects/StateDeath/StateDeath_" + i);
        while (texture != null)
        {
            Material mat = new Material(Shader.Find("Sprites/Default"));
            mat.mainTexture = texture;
            i++;
            texture = Resources.Load<Texture2D>("Effects/StateDeath/StateDeath_" + i);
            DeathAnimation.Add(mat);
        }
    }
    private Material LoadMaterial( string s)
    {
        return Resources.Load<Material>("Materials/"+s);
    }

    public static Mesh CreateQuadMesh(float height, float width)
    {
        Vector3[] vertices = new Vector3[4];
        Vector2[] uv = new Vector2[4];
        int[] triangles = new int[6];
        vertices[0] = new Vector3(-.5f * width, -.5f * height);
        vertices[1] = new Vector3(-.5f * width, +.5f * height);
        vertices[2] = new Vector3(+.5f * width, +.5f * height);
        vertices[3] = new Vector3(+.5f * width, -.5f * height);

        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(0, 1);
        uv[2] = new Vector2(1, 1);
        uv[3] = new Vector2(1, 0);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 3;
        triangles[3] = 1;
        triangles[4] = 2;
        triangles[5] = 3;
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        return mesh;
    }
}
