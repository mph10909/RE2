using UnityEngine;

public abstract class StateSO : ScriptableObject
{
    public abstract void EnterState(FSMController controller);
    public abstract void ExecuteState(FSMController controller);
    public abstract void ExitState(FSMController controller);
    public abstract void CheckTransitions(FSMController controller);
}
