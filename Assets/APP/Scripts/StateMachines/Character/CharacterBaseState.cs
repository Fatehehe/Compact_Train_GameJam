using UnityEngine;

public abstract class CharacterBaseState : State
{
    protected CharacterStateMachine stateMachine;
    public CharacterBaseState(CharacterStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
}
