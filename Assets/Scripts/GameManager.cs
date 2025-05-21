using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public LayerMask bulletLayer;
    private void Awake()
    {
        if (instance == null) instance = this; 
    }

    private void Start()
    {
#if !UNITY_EDITOR
    Cursor.visible = false;
#endif
    }
}
