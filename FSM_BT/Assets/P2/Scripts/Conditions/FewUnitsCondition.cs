using UnityEngine;

[CreateAssetMenu(fileName = "FewUnitsCondition", menuName = "FSM/Conditions/FewUnitsCondition")]
public class FewUnitsCondition : Condition
{
    public int threshold = 3;
    public float detectionRadius = 10f;
    public LayerMask unitMask;

    public override bool Check(StateMachine stateMachine)
    {
        Collider[] units = Physics.OverlapSphere(stateMachine.transform.position, detectionRadius, unitMask);
        return units.Length > 0 && units.Length <= threshold;
    }
}
