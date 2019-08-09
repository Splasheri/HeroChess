//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System;

//public class test : MonoBehaviour
//{

//    public desu d;
//    public List<character> characters;
//    [System.Serializable]
//    public struct character
//    {
//        public Vector3 position;
//        public Material material;
//        public Mesh mesh;
//        public int layer;

//        public Vector3 Position { get => position; set => position = value; }
//        public Material Material { get => material; set => material = value; }
//        public Mesh Mesh { get => mesh; set => mesh = value; }
//    }

//    [System.Serializable]
//    public struct MeshData
//    {
//        private List<Vector3> vertices;
//        private List<Vector2> uvs;
//        private int[] triangles;

//        public List<Vector2> Uvs { get => uvs; set => uvs = value; }
//        public List<Vector3> Vertices { get => vertices; set => vertices = value; }
//        public int[] Triangles { get => triangles; set => triangles = value; }
//    }

//    [System.Serializable]
//    public struct data
//    {
//        public float x;
//        public float y;
//        public float width;
//        public float height;
//    }
//    [System.Serializable]
//    public struct Position
//    {
//        public float x;
//        public float y;
//        public float skX;
//    }
//    [System.Serializable]
//    public struct Bone
//    {
//        public string name;
//        public string parent;
//        public Position transform;
//    }
//    [System.Serializable]
//    public struct Slot
//    {
//        public string name;
//        public string parent;
//    }
//    [System.Serializable]
//    public struct AnimationData
//    {
//        public int duration;
//        public string name;
//        public List<BoneMotion> bone;
//    }
//    [System.Serializable]
//    public struct BoneMotion
//    {
//        public string name;
//        public List<translateMotion> translateFrame;
//        public List<rotateMotion> rotateFrame;
//    }
//    [System.Serializable]
//    public struct translateMotion
//    {
//        public int duration;
//        public float x;
//        public float y;
//    }
//    [System.Serializable]
//    public struct rotateMotion
//    {
//        public string name;
//        public float rotate;
//    }

//    [System.Serializable]
//    public struct Display
//    {
//        public string type;
//        public string name;
//        public float width;
//        public float height;
//        public List<float> vertices;
//        public List<float> uvs;
//        public List<int> triangles;
//        public Position transform;
//    }
//    [System.Serializable]
//    public struct slotData
//    {
//        public string name;
//        public List<Display> display;
//    }
//    [System.Serializable]
//    public struct Skin
//    {
//        public List<slotData> slot;
//    }

//    [System.Serializable]
//    public struct Armature
//    {
//        public data aabb;
//        public List<Bone> bone;
//        public List<Slot> slot;
//        public List<Skin> skin;
//        public List<AnimationData> animation;
//    }
//    [System.Serializable]
//    public struct desu
//    {
//        public List<Armature> armature;

//    }

//    [System.Serializable]
//    public struct BoneData
//    {
//        public string parent;
//        public Position transform;
//    }
//    public Dictionary<string, BoneData> BoneList;
//    public Dictionary<string, string> SlotList;


//    private void Start()
//    {
//        d = new desu() { };
//        characters = new List<character>();
//        d = JsonUtility.FromJson<desu>(Resources.Load<TextAsset>("dragonbones_assets/DwarfDrill_ske_ske").text);
//        PrepareDics();
//        for(int i=0;i<d.armature[0].skin[0].slot.Count;i++)
//        {
//            character newcharacter = new character();
//            if (d.armature[0].skin[0].slot[i].display[0].type=="mesh")
//            {
//                PrepareMesh(i);
//                newcharacter.Position = new Vector3();
//                var bd = BoneList[SlotList[d.armature[0].skin[0].slot[i].display[0].name]];
//                do
//                {
//                    newcharacter.Position = new Vector3(bd.transform.x, bd.transform.y);
//                    if (string.IsNullOrEmpty(bd.parent))
//                    {
//                        bd = BoneList[bd.parent];
//                    }
//                }
//                while (string.IsNullOrEmpty(bd.parent));
//                newcharacter.Material = new Material(Shader.Find("Sprites/Default")) { mainTexture = Resources.Load<Texture2D>("dragonbones_assets/DwarfDrill_ske_texture/" + d.armature[0].skin[0].slot[i].display[0].name)};
//                newcharacter.Mesh = PrepareMesh(i);
//            }
//            characters.Add(newcharacter);
//        }
//        var vertices = new List<Vector3>();
//        var uvs = new List<Vector2>();
//        var triangles = d.armature[0].skin[0].slot[5].display[0].triangles.ToArray();
//        for (int i = 0; i < d.armature[0].skin[0].slot[5].display[0].vertices.Count / 2; i++)
//        {
//            vertices.Add(new Vector3(d.armature[0].skin[0].slot[5].display[0].vertices[i * 2], d.armature[0].skin[0].slot[5].display[0].vertices[i * 2 + 1]*-1, 0));
//            uvs.Add(new Vector2(d.armature[0].skin[0].slot[5].display[0].uvs[i * 2], 1 - d.armature[0].skin[0].slot[5].display[0].uvs[i * 2 + 1]));
//        }
//        GameObject.Find("Quad").GetComponent<MeshFilter>().mesh = new Mesh() {vertices = vertices.ToArray(),    uv = uvs.ToArray(),     triangles = triangles};        
//        GameObject.Find("Quad").GetComponent<MeshRenderer>().material = new Material(Shader.Find("Sprites/Default")){ mainTexture = Resources.Load<Texture2D>("dragonbones_assets/DwarfDrill_ske_texture/Skirt")};
//        GameObject.Find("Quad").GetComponent<MeshCollider>().sharedMesh = new Mesh() { vertices = vertices.ToArray(), uv = uvs.ToArray(), triangles = triangles };
//        vertices = new List<Vector3>();
//        uvs = new List<Vector2>();
//        triangles = d.armature[0].skin[0].slot[0].display[0].triangles.ToArray();
//        for (int i = 0; i < d.armature[0].skin[0].slot[0].display[0].vertices.Count / 2; i++)
//        {
//            vertices.Add(new Vector3(d.armature[0].skin[0].slot[0].display[0].vertices[i * 2], d.armature[0].skin[0].slot[0].display[0].vertices[i * 2 + 1] * -1, 0));
//            uvs.Add(new Vector2(d.armature[0].skin[0].slot[0].display[0].uvs[i * 2], 1 - d.armature[0].skin[0].slot[0].display[0].uvs[i * 2 + 1]));
//        }
//        GameObject.Find("Quad1").GetComponent<MeshFilter>().mesh = new Mesh() { vertices = vertices.ToArray(), uv = uvs.ToArray(), triangles = triangles };
//        GameObject.Find("Quad1").GetComponent<MeshRenderer>().material = new Material(Shader.Find("Sprites/Default")) { mainTexture = Resources.Load<Texture2D>("dragonbones_assets/DwarfDrill_ske_texture/Left Arm") };
//        GameObject.Find("Quad1").GetComponent<MeshCollider>().sharedMesh = new Mesh() { vertices = vertices.ToArray(), uv = uvs.ToArray(), triangles = triangles };
//        vertices = new List<Vector3>();
//        uvs = new List<Vector2>();
//        triangles = d.armature[0].skin[0].slot[7].display[0].triangles.ToArray();
//        for (int i = 0; i < d.armature[0].skin[0].slot[7].display[0].vertices.Count / 2; i++)
//        {
//            vertices.Add(new Vector3(d.armature[0].skin[0].slot[7].display[0].vertices[i * 2], d.armature[0].skin[0].slot[7].display[0].vertices[i * 2 + 1] * -1, 0));
//            uvs.Add(new Vector2(d.armature[0].skin[0].slot[7].display[0].uvs[i * 2], 1 - d.armature[0].skin[0].slot[7].display[0].uvs[i * 2 + 1]));
//        }
//        GameObject.Find("Quad2").GetComponent<MeshFilter>().mesh = new Mesh() { vertices = vertices.ToArray(), uv = uvs.ToArray(), triangles = triangles };
//        GameObject.Find("Quad2").GetComponent<MeshRenderer>().material = new Material(Shader.Find("Sprites/Default")) { mainTexture = Resources.Load<Texture2D>("dragonbones_assets/DwarfDrill_ske_texture/Head") };
//        GameObject.Find("Quad2").GetComponent<MeshCollider>().sharedMesh = new Mesh() { vertices = vertices.ToArray(), uv = uvs.ToArray(), triangles = triangles };
//        vertices = new List<Vector3>();
//        uvs = new List<Vector2>();
//        triangles = d.armature[0].skin[0].slot[3].display[0].triangles.ToArray();
//        for (int i = 0; i < d.armature[0].skin[0].slot[3].display[0].vertices.Count / 2; i++)
//        {
//            vertices.Add(new Vector3(d.armature[0].skin[0].slot[3].display[0].vertices[i * 2], d.armature[0].skin[0].slot[3].display[0].vertices[i * 2 + 1] * -1, 0));
//            uvs.Add(new Vector2(d.armature[0].skin[0].slot[3].display[0].uvs[i * 2], 1 - d.armature[0].skin[0].slot[3].display[0].uvs[i * 2 + 1]));
//        }
//        GameObject.Find("Quad3").GetComponent<MeshFilter>().mesh = new Mesh() { vertices = vertices.ToArray(), uv = uvs.ToArray(), triangles = triangles };
//        GameObject.Find("Quad3").GetComponent<MeshRenderer>().material = new Material(Shader.Find("Sprites/Default")) { mainTexture = Resources.Load<Texture2D>("dragonbones_assets/DwarfDrill_ske_texture/Leg") };
//        GameObject.Find("Quad3").GetComponent<MeshCollider>().sharedMesh = new Mesh() { vertices = vertices.ToArray(), uv = uvs.ToArray(), triangles = triangles };
//    }
//    //private void Update()
//    //{
//    //    foreach (var character in characters)
//    //    {
//    //        Graphics.DrawMesh
//    //            (
//    //                character.Mesh,
//    //                Matrix4x4.TRS(character.position,Quaternion.identity,new Vector3(1,-1,1)),
//    //                character.Material,
//    //                0
//    //            );
//    //    }
//    //}
//    private void PrepareDics()
//    {
//        BoneList = new Dictionary<string, BoneData>();
//        SlotList = new Dictionary<string, string>();
//        foreach (var bone in d.armature[0].bone)
//        {
//            BoneList.Add(bone.name, new BoneData() { parent = bone.parent, transform = bone.transform });
//        }
//        foreach (var slot in d.armature[0].slot)
//        {
//            SlotList.Add(slot.name, slot.parent);
//        }
//    }
//    private Mesh PrepareMesh (int slotIndex)
//    {
//        var vertices = new List<Vector3>();
//        var uvs = new List<Vector2>();
//        var triangles = d.armature[0].skin[0].slot[slotIndex].display[0].triangles.ToArray();
//        for (int i = 0; i < d.armature[0].skin[0].slot[slotIndex].display[0].vertices.Count / 2; i++)
//        {
//            vertices.Add(new Vector3(d.armature[0].skin[0].slot[slotIndex].display[0].vertices[i * 2]*-1, d.armature[0].skin[0].slot[slotIndex].display[0].vertices[i * 2 + 1]*-1, 0));
//            uvs.Add(new Vector2(d.armature[0].skin[0].slot[slotIndex].display[0].uvs[i * 2], 1-d.armature[0].skin[0].slot[slotIndex].display[0].uvs[i * 2 + 1]));
//        }
//        return new Mesh() { vertices = vertices.ToArray(), uv = uvs.ToArray(), triangles = triangles };
//    }
//}    