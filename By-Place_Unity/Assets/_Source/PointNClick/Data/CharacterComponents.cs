using System;
using Movement;
using UnityEngine;
using VContainer;

namespace PointNClick.Data
{
    public class CharacterComponents
    {
        public IMoveable Mover { get; private set; }

        [Inject]
        public CharacterComponents(NavMeshMover mover) => Mover = mover;
    }
}