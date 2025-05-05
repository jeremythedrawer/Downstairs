using UnityEngine;
using System.Collections;
public class DeathState : State
{
    public enum TypesOfDeath
    {
        None,
        Burned,
        Squished,
        ImpaledFromAbove,
        ImpaledFromBelow,
        Skewered
    }

    public static TypesOfDeath typeOfDeath = TypesOfDeath.None;
    public override void Enter()
    {
        movementController.canMove = false;
    }
    public override void Do()
    {

    }
    public override void FixedDo()
    {
    }
    public override void Exit()
    {
        base.Exit();
        
    }
    private IEnumerator DyingByEnemy()
    {
        yield return null;
        InvokeScreenFade();
        Exit();
    }

    private void InvokeScreenFade()
    {
        ScreenFader.Instance.FadeBlackToCheckpoint(() =>
        {
        });
    }

}
