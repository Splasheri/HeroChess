using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class animationManager : MonoBehaviour
{
    private static animationManager manager;
    public static animationManager instance
    {
        get
        {
            if (manager!=null)
            {
                return manager;
            }
            else
            {
                throw new System.Exception();
            }
        }
    }
    private Dictionary<string, GameObject> animationNames;
    private Dictionary<string, ParticleSystem> playingAnimations;
    private Dictionary<particleAnimation, ParticleSystem> massAnimation;

    public Entity currentEntity;
    // Start is called before the first frame update
    void Awake()
    {
        manager = this;
        animationNames = new Dictionary<string, GameObject>();
        playingAnimations = new Dictionary<string, ParticleSystem>();
        massAnimation = new Dictionary<particleAnimation,ParticleSystem>();
        animationNames.Add("SceneEffect", Resources.Load<GameObject>("Prefabs/SceneEffect") as GameObject);
        animationNames.Add("UnitCircle", Resources.Load<GameObject>("Prefabs/UnitCircle") as GameObject);
        animationNames.Add("MoveSlide", Resources.Load<GameObject>("Prefabs/MoveSlide") as GameObject);
        animationNames.Add("AttackAnimation", Resources.Load<GameObject>("Prefabs/AttackAnimation") as GameObject);
        animationNames.Add("HealOnce", Resources.Load<GameObject>("Prefabs/Effects/HealOnce") as GameObject);
        animationNames.Add("EmojiHappy", Resources.Load<GameObject>("Prefabs/Effects/EmojiHappy") as GameObject);
        animationNames.Add("Smoke", Resources.Load<GameObject>("Prefabs/Effects/Smoke") as GameObject);
        animationNames.Add("Spiner", Resources.Load<GameObject>("Prefabs/Effects/Spiner") as GameObject);
        animationNames.Add("Curse", Resources.Load<GameObject>("Prefabs/Effects/Curse") as GameObject);
        animationNames.Add("Poison", Resources.Load<GameObject>("Prefabs/Effects/Poison") as GameObject);
        animationNames.Add("ChilBlain", Resources.Load<GameObject>("Prefabs/Effects/ChilBlain") as GameObject);
        animationNames.Add("CasualBuff", Resources.Load<GameObject>("Prefabs/Effects/CasualBuff") as GameObject);
        animationNames.Add("Amourex", Resources.Load<GameObject>("Prefabs/Effects/Amourex") as GameObject);
        animationNames.Add("FireShield", Resources.Load<GameObject>("Prefabs/Effects/FireShield") as GameObject);
        animationNames.Add("Explosion", Resources.Load<GameObject>("Prefabs/Effects/Explosion") as GameObject);
        animationNames.Add("Sunstrike", Resources.Load<GameObject>("Prefabs/Effects/Sunstrike") as GameObject);
        animationNames.Add("Lightning", Resources.Load<GameObject>("Prefabs/Effects/Lightning") as GameObject);
    }
    
    
    public struct particleAnimation
    {
        public string name;
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;
        public Transform parent;
    }
    public void CreateUniqueAnimationSequence(params particleAnimation[] animations)
    {
        foreach (var animation in animations)
        {
            if (!string.IsNullOrEmpty(animation.name)&&!playingAnimations.ContainsKey(animation.name))
            {
                var instance = GameObject.Instantiate(animationNames[animation.name]);
                if (animation.parent != null)
                {
                    instance.transform.SetParent(animation.parent);
                }
                if (animation.position==Vector3.zero)
                {
                    instance.transform.localPosition = new Vector3(0,0,-10);
                }
                else
                {
                    instance.transform.localPosition = animation.position;
                }
                if (animation.rotation != Vector3.zero)
                {
                    instance.transform.rotation = Quaternion.Euler(animation.rotation);
                }
                if (animation.scale != Vector3.zero)
                {
                    instance.transform.localScale = animation.scale;
                }
                playingAnimations.Add(animation.name,instance.GetComponent<ParticleSystem>());
            }
        }
    }
    public void CreateMassAnimationName(params particleAnimation[] animations)
    {
        foreach(var anime in animations)
        {
            if (anime.position!=Vector3.zero && !string.IsNullOrEmpty(anime.name) && !massAnimation.ContainsKey(anime))
            {
                var instance = GameObject.Instantiate(animationNames[anime.name]);
                if (anime.parent != null)
                {
                    instance.transform.SetParent(anime.parent);
                }
                instance.transform.localPosition = new Vector3(anime.position.x, anime.position.y,-31);
                massAnimation.Add(anime, instance.GetComponent<ParticleSystem>());
            }
        }
    }
    public void CreateMassAnimationName(particleAnimation anime, int team, bool isActive=false)
    {
        if (anime.position != Vector3.zero && !string.IsNullOrEmpty(anime.name) && !massAnimation.ContainsKey(anime) && team!=0)
        {
            var instance = GameObject.Instantiate(animationNames[anime.name]);
            instance.transform.localPosition = anime.position;
            massAnimation.Add(anime, instance.GetComponent<ParticleSystem>());
            if (team<0)
            {
#pragma warning disable CS0618 // Тип или член устарел
                massAnimation[anime].startColor = Color.red;
#pragma warning restore CS0618 // Тип или член устарел
            }
            else
            {
#pragma warning disable CS0618 // Тип или член устарел
                massAnimation[anime].startColor = Color.green;
#pragma warning restore CS0618 // Тип или член устарел
            }
            if (isActive)
            {
#pragma warning disable CS0618 // Тип или член устарел
                massAnimation[anime].startColor = Color.yellow;
#pragma warning restore CS0618 // Тип или член устарел
            }
        }
    }
    public bool TryPlayUniqueAnimation(string name)
    {
        if (playingAnimations.ContainsKey(name))
        {
            playingAnimations[name].Play();
            return true;
        }
        else
        {
            return false;
        }
    }
    public void PlayMassAnimation(particleAnimation[] effects)
    {
        foreach (var effect in effects)
        {
            if (massAnimation.ContainsKey(effect))
            {
                massAnimation[effect].Play();
            }
        }
    }
    public void TryStopUniqueAnimationSeuqence(params string[]names)
    {
        foreach (var animation in names)
        {
            if (playingAnimations.ContainsKey(animation))
            {
                Destroy(playingAnimations[animation].gameObject);
                playingAnimations.Remove(animation);
            }
        }
    }
    public void DestroyMassAnimationInstance(particleAnimation animation)
    {
        if (!string.IsNullOrEmpty(animation.name) && animation.position!=Vector3.zero)
        {
            Destroy(massAnimation[animation].gameObject);
            massAnimation.Remove(animation);
        }
    }
    public void HideMassAnimation(string name)
    {
        if (!string.IsNullOrEmpty(name))
        {
            foreach (var animation in massAnimation.Keys)
            {
                if (animation.name==name)
                {
                    massAnimation[animation].transform.localPosition += new Vector3(0,0,10000);
                }
            }
        }
    }
    public void ShowMassAnimation(string name)
    {
        if (!string.IsNullOrEmpty(name))
        {
            foreach (var animation in massAnimation.Keys)
            {
                if (animation.name == name)
                {
                    massAnimation[animation].transform.localPosition -= new Vector3(0, 0, 10000);
                }
            }
        }
    }

    public void ShowSkillScroll(string name)
    {
        var scroll = GameObject.Find("SkillName").GetComponent<RectTransform>();
        scroll.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = name;
        scroll.localScale = Vector3.one;
    }
    public void HideSkillScroll()
    {
        var scroll = GameObject.Find("SkillName").GetComponent<RectTransform>();
        scroll.localScale = Vector3.zero;
    }

    public void CreateAnimationHandler(string name, bool isMass = false)
    {
        var instance = new GameObject();
        instance.AddComponent<SkillAnimationHandler>().name = name;
        if (isMass)
        {
            instance.GetComponent<SkillAnimationHandler>().isMass = true;
        }
    }
    public void RemoveFromList(string name)
    {
        if (playingAnimations.ContainsKey(name))
        {
            playingAnimations.Remove(name);
        }
    }
    public void RemoveFromList(string name, bool isMass)
    {
        if (!isMass)
        {
            if (playingAnimations.ContainsKey(name))
            {
                playingAnimations.Remove(name);
                var em = World.Active.EntityManager;
                em.AddComponentData<Action>(currentEntity, new Action());
                //HideSkillScroll();
            }
        }
        else
        {
            bool contains = false;
            particleAnimation k = new particleAnimation();
            do
            {
                contains = false;
                foreach (var anime in massAnimation)
                {
                    if (anime.Key.name == name)
                    {
                        contains = true;
                        k = anime.Key;
                    }
                }
                if (contains)
                {
                    massAnimation.Remove(k);
                }
            } while (contains == true);
            var em = World.Active.EntityManager;
            em.AddComponentData<Action>(currentEntity, new Action());
            //HideSkillScroll();
        }
    }
}
