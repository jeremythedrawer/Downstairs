using UnityEngine;

public class StateList : MonoBehaviour
{
    public CharacterType characterType;
    public enum CharacterType
    {
        Player
    }

    [HideInInspector] public RunState runState;

    [HideInInspector] public DeathState deathState;

}

