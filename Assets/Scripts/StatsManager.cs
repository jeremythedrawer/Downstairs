using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager instance;

    public List<SolitaryFish> solitaryFishList;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void CheckOffFish(SolitaryFish fish)
    {
        foreach(SolitaryFish solitaryFish in solitaryFishList)
        {
            if(solitaryFish == fish)
            {
                solitaryFish.uncovered = true;
                solitaryFishList.Remove(solitaryFish);
                break;
            }
        }
    }
}
