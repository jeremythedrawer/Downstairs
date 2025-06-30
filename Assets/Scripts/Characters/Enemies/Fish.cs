using UnityEngine;

public class Fish : MonoBehaviour
{
    public float moveBoundRadius;

    public FishShaderController shaderController;
    public CharacterStats stats;
    public Vector2 startPos {  get; private set; }
    protected Vector2 targetPos { get; private set; }

    protected virtual void OnEnable()
    {
        startPos = transform.position;
    }

    protected void GetNewPos()
    {
        targetPos = (Random.insideUnitCircle * moveBoundRadius) + startPos;
    }
}
