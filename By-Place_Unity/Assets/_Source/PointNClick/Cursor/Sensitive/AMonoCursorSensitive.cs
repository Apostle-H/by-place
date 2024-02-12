using System;
using PointNClick.Cursor.Sensitive.Data;
using UnityEngine;
using VContainer.Unity;

namespace PointNClick.Cursor.Sensitive
{
    public abstract class AMonoCursorSensitive : MonoBehaviour, ICursorSensitive
    {
        public abstract int Id { get; }
        public abstract Texture2D Cursor { get; }
        public abstract Vector2 HotSpot { get; }
        public abstract string Text { get; }
        
        public event Action<ICursorSensitive> OnEnter;
        public event Action<ICursorSensitive> OnExit;

        protected virtual void EnterInvoke() => OnEnter?.Invoke(this);
        protected virtual void ExitInvoke() => OnExit?.Invoke(this);
    }
}