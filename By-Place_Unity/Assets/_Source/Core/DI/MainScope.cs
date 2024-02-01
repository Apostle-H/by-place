using Character;
using Character.States;
using Character.View;
using DialogueSystem;
using DialogueSystem.ActionSystem;
using Input;
using InputSystem;
using PointNClick.Cursor.Manager;
using PointNClick.Data;
using PointNClick.Interactions;
using PointNClick.Movement;
using PointNClick.Movement.Data;
using QuestSystem;
using StateMachine;
using UnityEngine;
using Utils.Runners;
using VContainer;
using VContainer.Unity;

namespace Core.DI
{
    public class MainScope : LifetimeScope
    {
        [Header("PointNClick")]
        [SerializeField] private PointNClickConfigSO pointNClickConfigSO;
        [SerializeField] private MoverConfigSO moverConfigSO;

        protected override void Configure(IContainerBuilder builder)
        {
            ConfigureCommon(builder);
            ConfigureInput(builder);
            ConfigurePointNClick(builder);
            ConfigureCharacter(builder);
            ConfigureDialogueSystem(builder);
            ConfigureQuestSystem(builder);
            ConfigureCharacterStateMachine(builder);
        }

        private void ConfigureCommon(IContainerBuilder builder) => 
            builder.RegisterComponentInHierarchy<CoroutineRunner>();

        private void ConfigureInput(IContainerBuilder builder)
        {
            builder.Register<PointNClickActions>(Lifetime.Singleton);
            builder.RegisterEntryPoint<InputHandler>();
        }

        private void ConfigurePointNClick(IContainerBuilder builder)
        {
            builder.RegisterInstance(pointNClickConfigSO);
            builder.RegisterInstance(moverConfigSO);

            builder.RegisterComponentInHierarchy<UIElementsCursorManager>().As<ICursorManager>();

            builder.RegisterComponentInHierarchy<NavMeshMover>().As<IMover>();

            builder.Register<IInteracter, Interacter>(Lifetime.Singleton);
            builder.Register<CharacterComponents>(Lifetime.Singleton);
        }

        private void ConfigureCharacter(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<CharacterAnimationParams>();
        }

        private void ConfigureDialogueSystem(IContainerBuilder builder)
        {
            builder.Register<ActionResolver>(Lifetime.Singleton);
            builder.RegisterComponentInHierarchy<DialogueResolver>();
        }

        private void ConfigureQuestSystem(IContainerBuilder builder)
        {
            builder.Register<QuestManager>(Lifetime.Singleton);
        }

        private void ConfigureCharacterStateMachine(IContainerBuilder builder)
        {
            builder.Register<CharacterFreeState>(Lifetime.Singleton);
            builder.Register<CharacterInteractingState>(Lifetime.Singleton);
            builder.Register<IStatesProvider, CharacterStatesProvider>(Lifetime.Singleton);
            
            builder.UseEntryPoints(entryPoints =>
            {
                entryPoints.Add<CharacterStateMachine>();
            });
        }
    }
}