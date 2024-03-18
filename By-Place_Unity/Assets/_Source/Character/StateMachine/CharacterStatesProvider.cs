﻿using System;
using System.Collections.Generic;
using Character.StateMachine.States;
using StateMachine;
using VContainer;

namespace Character.StateMachine
{
    public class CharacterStatesProvider : ICharacterStatesProvider
    {
        private readonly Dictionary<Type, IState> _states = new();
        
        public IState StartingState { get; private set; }

        [Inject]
        public CharacterStatesProvider(CharacterFreeState characterFreeState, CharacterInteractingState characterInteractingState)
        {
            StartingState = characterFreeState;
            
            _states.Add(characterFreeState.GetType(), characterFreeState);
            _states.Add(characterInteractingState.GetType(), characterInteractingState);
        }
        
        public Dictionary<Type, IState> Get() => _states;
    }
}