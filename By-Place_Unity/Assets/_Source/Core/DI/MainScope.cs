﻿using InputSystem;
using Movement;
using PointNClick;
using PointNClick.Data;
using UnityEngine;
using Utils.Runners;
using VContainer;
using VContainer.Unity;

namespace Core.DI
{
    public class MainScope : LifetimeScope
    {
        [Header("Common")] 
        [SerializeField] private CoroutineRunner coroutineRunner;
        
        [Header("PointNClick")]
        [SerializeField] private NavMeshMover characterMover;
        [SerializeField] private PointNClickConfigSO pointNClickConfigSO;
        
        protected override void Configure(IContainerBuilder builder)
        {
            ConfigureCommon(builder);
            ConfigurePointNClick(builder);
        }

        private void ConfigureCommon(IContainerBuilder builder) => builder.RegisterComponent(coroutineRunner);

        private void ConfigurePointNClick(IContainerBuilder builder)
        {
            builder.Register<PointNClickActions>(Lifetime.Singleton);
            
            builder.RegisterComponent(characterMover);
            builder.Register<CharacterComponents>(Lifetime.Singleton);
            builder.RegisterInstance(pointNClickConfigSO);
            builder.RegisterEntryPoint<PointNClickInvoker>();
        }
    }
}