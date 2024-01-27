using Character;
using Character.States;
using DialogueSystem;
using DialogueSystem.ActionSystem;
using Input;
using InputSystem;
using PointNClick.Cursor.Manager;
using PointNClick.Data;
using PointNClick.Interactions;
using PointNClick.Movement;
using PointNClick.Movement.Data;
using StateMachine;
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
        [SerializeField] private PointNClickConfigSO pointNClickConfigSO;
        [SerializeField] private MoverConfigSO moverConfigSO;
        [SerializeField] private UIElementsCursorManager uiElementsCursorManager;
        [SerializeField] private NavMeshMover characterMover;
        
        [Header("DialogueSystem")]
        [SerializeField] private DialogueResolver dialogueResolver;
        
        protected override void Configure(IContainerBuilder builder)
        {
            ConfigureCommon(builder);
            ConfigureInput(builder);
            ConfigurePointNClick(builder);
            ConfigureDialogueSystem(builder);
            ConfigureCharacterStateMachine(builder);
        }

        private void ConfigureCommon(IContainerBuilder builder) => builder.RegisterComponent(coroutineRunner);

        private void ConfigureInput(IContainerBuilder builder)
        {
            builder.Register<PointNClickActions>(Lifetime.Singleton);
            builder.RegisterEntryPoint<InputHandler>();
        }

        private void ConfigurePointNClick(IContainerBuilder builder)
        {
            builder.RegisterInstance(pointNClickConfigSO);
            builder.RegisterInstance(moverConfigSO);

            builder.RegisterComponent(uiElementsCursorManager).AsImplementedInterfaces();
            builder.RegisterComponent(characterMover).AsImplementedInterfaces();

            builder.Register<Interacter>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<CharacterComponents>(Lifetime.Singleton);
        }

        private void ConfigureDialogueSystem(IContainerBuilder builder)
        {
            builder.Register<ActionResolver>(Lifetime.Singleton);
            builder.RegisterComponent(dialogueResolver);
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