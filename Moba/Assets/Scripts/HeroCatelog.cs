
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HeroCatelog : MonoBehaviour
{
    [System.Serializable]
    struct HeroInfo
    {
        public GameObject heroPrefab;
        public string HeroName;
    }

    [SerializeField] private List<HeroInfo> heroList = new List<HeroInfo>();

    public static HeroCatelog sInstance = null;


    void Start()
    {
        if (sInstance = null)
            sInstance = this;
    }

    public GameObject GetHeroByName(string name)
    {
        return heroList.Find(x => x.HeroName == name).heroPrefab;
    }
}
