using System;
using System.Linq;
using FSM;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.UIElements;

namespace AndoomiUtils{
    public static class Helpful
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public static Vector3 Vec2ToVec3(this Vector2 _value)
        {
            return new Vector3(_value.x, _value.y, 0.0f);
        }
        public static Vector3 Vec2ToVec3YInverted(this Vector2 _value) {
            return new Vector3(_value.x, -_value.y, 0);
        }
        public static Vector3 Vec2ToVec3Z(this Vector2 _value)
        {
            return new Vector3(_value.x, 0.0f, _value.y);
        }

        public static Vector2 Vec3ToVec2(this Vector3 _value) {
            return new Vector2(_value.x, _value.y);
        }

        public static Vector2 Rotate(this Vector2 _value, float _angle) {
            var vectoR = _value;
            var angleInRadians = _angle * Mathf.Deg2Rad;
            vectoR.x = _value.x * Mathf.Cos(angleInRadians) - _value.y * Mathf.Sin(angleInRadians);
            vectoR.y = _value.x * Mathf.Sin(angleInRadians) + _value.y * Mathf.Cos(angleInRadians);

            return vectoR;
        }

        public static Vector3 Rotate(this Vector3 _value, float _angle) {
            return _value.Vec3ToVec2().Rotate(_angle).Vec2ToVec3();
        }

        public static float Remap(this float _value, float _oldmin, float _oldmax, float _newmin, float _newmax) {
            if(_oldmax-_oldmin == 0) return 0;
            return _newmin + (_newmax - _newmin)*(_value/(_oldmax-_oldmin));
        }

        public static IPredicate Opposite(this IPredicate _funcPredicate) {
            return new FuncPredicate(() => !_funcPredicate.Evaluate());
        }

        // UI Toolkit Extensions //////////////////////////
        public static VisualElement CreateChild(this VisualElement _parent, params string[] _classes) {
            var child = new VisualElement();
            child.AddClass(_classes).AddTo(_parent);
            return child;
        }

        public static T CreateChild<T>(this VisualElement _parent, params string[] _classes) where T : VisualElement, new() {
            var child = new T();
            child.AddClass(_classes).AddTo(_parent);
            return child;
        }

        public static T AddTo<T>(this T _child, VisualElement _parent) where T : VisualElement {
            _parent.Add(_child);
            return _child;
        }
        public static T AddClass<T>(this T _visualElement, params string[] _classes) where T : VisualElement {
            foreach(string cls in _classes) {
                if (!string.IsNullOrEmpty(cls)) {
                    _visualElement.AddToClassList(cls);
                }
            }

            return _visualElement;
        }

        public static T RemoveClass<T>(this T _visualElement, params string[] _classes) where T : VisualElement {
            foreach(string cls in _classes) {
                if (!string.IsNullOrEmpty(cls)) {
                    _visualElement.RemoveFromClassList(cls);
                }
            }

            return _visualElement;
        }

        public static T WithManipulator<T>(this T _visualElement, IManipulator _manipulator) where T : VisualElement {
            _visualElement.AddManipulator(_manipulator);
            return _visualElement;
        }
        ///////////////////////////////////////////////////
        
        public static void LockCursor() {
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
        }
        public static void ReleaseCursor() {
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
        }

        public static Vector2 ClampVector2(Vector2 _vector, float _xMin, float _xMax, float _yMin, float _yMax) {
            _vector.x = Mathf.Clamp(_vector.x, _xMin, _xMax);
            _vector.y = Mathf.Clamp(_vector.y, _yMin, _yMax);
            return _vector;
        }
    }

    public abstract class Timer : IDisposable
    {
        public float CurrentTime {get; protected set;}
        public bool IsRunning {get; protected set;}

        protected float initialTime;
        public float Progress => Mathf.Clamp(CurrentTime/initialTime, 0.0f, 1.0f);
        public float ReverseProgress => 1.0f - Progress;
        public Action OnTimerStart = delegate{};
        public Action OnTimerStop = delegate{};
        public Action<float> OnTimerTick = delegate{};

        protected Timer(float _value) {
            initialTime = _value;
        }

        public void Start() {
            CurrentTime = initialTime;
            if (!IsRunning) {
                IsRunning = true;
                OnTimerStart.Invoke();
            }
        }
        public void Stop() {
            if (IsRunning) {
                IsRunning = false;
                OnTimerStop.Invoke();
            }
        }

        public abstract void Tick();
        public abstract bool IsFinished{get;}
        public void Resume() => IsRunning = true;
        public void Pause() => IsRunning = false;

        public virtual void Reset() => CurrentTime = initialTime;

        public virtual void Reset(float _newTime) {
            initialTime = _newTime;
            Reset();
        }

        bool disposed;

        ~Timer() {
            DisposeOf();
        }
        public void Dispose()
        {
            DisposeOf();
            GC.SuppressFinalize(this);
        }

        protected virtual void DisposeOf() {
            if (disposed) return;

            // if (_disposing) {
                // TimerManager.DeregisterTimer(this)
            // }

            disposed = true;
        }
    }

    public class CountdownTimer : Timer {
        public CountdownTimer(float _value, bool _loop = false) : base(_value) { m_Loop = _loop; }
        bool m_Loop = false;
        public override void Tick()
        {
            if (IsRunning && CurrentTime > 0.0f){
                CurrentTime -= Time.deltaTime;
                OnTimerTick.Invoke(Time.deltaTime);
            }
            if (IsRunning && CurrentTime <= 0.0f)
            {
                Stop();
                if (m_Loop) Start();
            }
        }

        public void Break() {
            Stop();
            Reset();
        }

        public override bool IsFinished => CurrentTime <= 0.0f;
    }

}
