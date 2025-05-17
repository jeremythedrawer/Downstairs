using System.Collections;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    public bool isComplete { get; protected set; } = true;
    public bool playingAnimation { get; set; } = false;

    protected StateManager core;
    protected PlayerBrain brain;
    protected SphereCollider sphereCollider => brain.sphereCollider;
    protected MovementController movementController => brain.movementController;
    protected StateList stateList => core.stateList;

    public StateMachine machine;
    public StateMachine parent;

    public State state => machine.state;

    protected void Set(State newState)
    {
        machine.Set(newState);
    }

    public void SetCore(StateManager _core)
    {
        machine = new StateMachine();
        core = _core;
    }

    public void SetBrain(PlayerBrain _brain)
    {
        brain = _brain;
    }

    public virtual void Enter()
    { 
    }
    public virtual void Do()
    {

    }
    public virtual void FixedDo()
    {
    }
    public virtual void Exit()
    {
        isComplete = true;
        playingAnimation = false;
        machine.forceState = true;
    }

    public void DoBranch()
    {
        if (!isComplete)
        {
            Do();
            state?.DoBranch(); // finds leaf state by calling Do() on every branch
        }
    }
    
    public void FixedDoBranch()
    {
        if (!isComplete)
        {
            FixedDo();
            state?.FixedDoBranch(); // finds leaf state by calling Do() on every branch
        }
    }
    public void Initialise(StateMachine _parent)
    {
        parent = _parent;
        isComplete = false;
    }
}
