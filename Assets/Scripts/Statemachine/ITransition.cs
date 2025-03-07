using UnityEngine;

namespace FSM {
    public interface ITransition
    {
        IState To {get;}
        IPredicate Condition {get;}
    }
}
