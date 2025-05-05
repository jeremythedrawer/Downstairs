using System.Collections.Generic;
using UnityEngine;

public abstract class StateManager : MonoBehaviour
{
    public StateList stateList;
    public StateMachine machine;

    public bool showStateList;

    public State state => machine.state;

    protected void SetState(State newState)
    {
        machine.Set(newState);
    }

    public void SetupInstances(PlayerBrain _playerBrain)
    {
        machine = new StateMachine();

        State[] allChildStates = GetComponentsInChildren<State>();

        if (allChildStates.Length == 0)
        {
            Debug.LogError("No child states found under " + gameObject.name);
        }
        foreach (State state in allChildStates)
        {
            state.SetCore(this);
            state.SetBrain(_playerBrain);
        }
    }

    private void OnDrawGizmos()
    {
        #if UNITY_EDITOR
        if (Application.isPlaying && state != null)
        {
            if (showStateList)
            {
                List<State> states = machine.GetActiveStateBranch();
                UnityEditor.Handles.Label(transform.position + new Vector3(0, 2, 0), "Active States: " + string.Join(" > ", states));
            }
        }
        #endif
    }
}
