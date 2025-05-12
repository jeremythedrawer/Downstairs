using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public LayerMask bulletLayer;
    private void Awake()
    {
        if (instance == null) instance = this; 
    }
}
