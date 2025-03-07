using TMPro;
using UnityEngine;

namespace FSM {
    public interface IState
    {
        void OnEnter();
        void OnExit();
        void OnTick(float _dt);
        void OnPhysics(float _fdt);

        string GetName();
    }
}
