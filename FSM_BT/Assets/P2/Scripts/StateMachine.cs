using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public State initialState;
    public State currentState;

    public Transform[] patrolPoints;

    public FSMContext context = new FSMContext();

    private void Start()
    {
        ChangeState(initialState);
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentState.UpdateState(this);
            currentState.CheckTransitions(this);
        }
    }

    public void ChangeState(State state)
    {
        if (currentState == state || state == null)
        {
            return;
        }

        if (currentState != null)
        {
            currentState.ExitState(this);
        }

        currentState = state;
        currentState.EnterState(this);
    }
}

public class FSMContext
{
    public GameObject player;
    public int necessaryInt;

    public float investigationTimer = 0f;
}