using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vault.StateMachine
{
    public class IStateMachine<T>
    {
        public IState<T> State;
        public void Initialize(IState<T> state)
        {
            State = state;
            State.Enter();
        }

        public void ChangeState(IState<T> newState)
        {
            if (State != null)
            {
                State.Exit();
            }
            State = newState;
            State.Enter();
        }
    }
}
