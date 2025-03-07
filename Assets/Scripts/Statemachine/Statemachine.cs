
using System;
using System.Collections.Generic;
using FSM;
using AndoomiUtils;

public class Statemachine
{
    StateNode current;
    public IState CurrentState => current.State;
    Dictionary<Type, StateNode> nodes = new();

    HashSet<ITransition> anyTransitions = new();

    public Action<IState> OnStateChange;

    public void Tick(float _dt) {
        var transition = GetTransition();
        if (transition != null) ChangeState(transition.To);

        current.State?.OnTick(_dt);
    }
    public void Physics(float _fdt) {
        current.State?.OnPhysics(_fdt);
    }

    
    public void ChangeState(IState state) {
        current?.State?.OnExit();
        current = nodes[state.GetType()];
        OnStateChange?.Invoke(current.State);
        current.State?.OnEnter();
    }
    
    ITransition GetTransition() {
        foreach (var transition in anyTransitions)
            if (transition.Condition.Evaluate() && current.State != transition.To)
                return transition;
        
        foreach (var transition in current.Transitions)
            if (transition.Condition.Evaluate())
                return transition;

        return null;
    }

    public void AddTransition(IState _from, IState _to, IPredicate _condition) {
        GetOrAddNode(_from).AddTransition(GetOrAddNode(_to).State, _condition);
    }
    public void AddTwowayTransition(IState _from, IState _to, IPredicate _codition) {
        AddTransition(_from, _to, _codition);
        AddTransition(_to, _from, _codition.Opposite());
    }
    public void AddAnyTransition(IState _to, IPredicate _condition) {
        anyTransitions.Add(new Transition(GetOrAddNode(_to).State, _condition));
    }

    StateNode GetOrAddNode(IState _state) {
        var node = nodes.GetValueOrDefault(_state.GetType());

        if (node == null) {
            node = new StateNode(_state);
            nodes.Add(_state.GetType(), node);
        }

        return node;
    }

    public void AddState(IState _state)
    {
        GetOrAddNode(_state);
    }

    class StateNode {
        public IState State {get;}
        public HashSet<ITransition> Transitions {get;}

        public StateNode(IState _state) {
            State = _state;
            Transitions = new HashSet<ITransition>();
        }

        public void AddTransition(IState _to, IPredicate _condition) {
            Transitions.Add(new Transition(_to, _condition));
        }
    }
}
