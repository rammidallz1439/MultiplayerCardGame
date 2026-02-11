using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Vault.StateMachine
{
    public abstract class IState<T> 
    {
        protected T Owner;
        protected IStateMachine<T> StateMachine; // The state machine managing this state

        public IState(T owner, IStateMachine<T> stateMachine)
        {
            Owner = owner;
            StateMachine = stateMachine;
        }

        public abstract void Enter(); // Called when entering this state
        public abstract void Exit();
        public abstract void Update(); // Called every frame while in this state
        public abstract void FixedUpdate(); // Called every fixed frame-rate frame while in this state
    }
}

