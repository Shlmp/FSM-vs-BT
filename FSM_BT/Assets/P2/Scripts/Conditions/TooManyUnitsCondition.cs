using UnityEngine;

[CreateAssetMenu(fileName = "TooManyUnitsCondition", menuName = "FSM/Conditions/TooManyUnitsCondition")]
public class TooManyUnitsCondition : Condition
{
    public int threshold = 5;
    public float detectionRadius = 10f;
    public LayerMask unitMask;

    public override bool Check(StateMachine stateMachine)
    {
        Collider[] units = Physics.OverlapSphere(stateMachine.transform.position, detectionRadius, unitMask);
        return units.Length > threshold;
    }
}
