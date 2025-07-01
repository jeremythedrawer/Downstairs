using System;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager instance;

    [Serializable]
    public class FoundFish
    {
        public Fish fish;
        public bool playerFoundFish {  get; set; }
    }

    public List<FoundFish> fishToFind;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
    private void Start()
    {
        HideCursor();
    }


    public void CheckOffFish(SolitaryFish fish)
    {
        foreach(FoundFish foundFish in fishToFind)
        {
            if(foundFish.fish == fish)
            {
                foundFish.playerFoundFish = true;
                break;
            }
        }
    }
    private void HideCursor()
    {
#if !UNITY_EDITOR
    Cursor.visible = false;
#endif
    }
}
