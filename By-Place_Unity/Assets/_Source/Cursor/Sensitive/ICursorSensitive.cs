﻿using System;
using UnityEngine;

namespace Cursor.Sensitive
{
    public interface ICursorSensitive
    {
        public int Id { get; }
        public Texture2D Cursor { get; }
        public Vector2 HotSpot { get; }
        public string Text { get; }
        public bool Capture { get; }

        public event Action<ICursorSensitive> OnEnter;
        public event Action<ICursorSensitive> OnExit;
    }
}