using ActionSystem;
using Animate;
using Animate.Resolve;
using Character;
using Character.Data;
using Character.States;
using Character.View;
using Core.Loaders;
using Cursor.Manager;
using Cursor.Sensitive;
using Dialogue.Data;
using Dialogue.Resolve;
using Dialogue.Resolve.Data;
using Input;
using InputSystem;
using Interactions;
using Inventory.Actions;
using Inventory.View;
using Inventory.View.Data;
using Journal.Quest;
using Journal.Quest.View;
using Journal.Quest.View.Data;
using Journal.View;
using Journal.View.Data;
using Movement;
using Movement.Data;
using PointNClick.Data;
using QuestSystem;
using QuestSystem.Actions;
using QuestSystem.View;
using QuestSystem.View.Data;
using StateMachine;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UIElements;
using Utils.Runners;
using Utils.Services;
using VContainer;
using VContainer.Unity;

namespace Core.DI
{
    public class MainScope : LifetimeScope
    {
        [Header("PointNClick")]
        [SerializeField] private PointNClickConfigSO pointNClickConfigSO;
        [SerializeField] private MoverConfigSO moverConfigSO;
        
        [Header("DialogueSystem")]
        [SerializeField] private DialogueViewConfigSO dialogueViewConfigSO;

        [Header("Inventory")] 
        [SerializeField] private InventoryViewConfigSO inventoryViewConfigSO;
        
        [Header("QuestSystem")]
        [SerializeField] private QuestManagerViewConfigSO questManagerViewConfigSO;
        
        [Header("Journal")]
        [SerializeField] private JournalViewConfigSO journalViewConfigSO;
        [SerializeField] private QuestPageViewConfigSO questPageViewConfigSO;

        protected override void Configure(IContainerBuilder builder)
        {
            ConfigureCommon(builder);
            ConfigureServices(builder);
            ConfigureInput(builder);
            ConfigurePointNClick(builder);
            ConfigureCharacter(builder);
            ConfigureActionSystem(builder);
            ConfigureAnimate(builder);
            ConfigureDialogueSystem(builder);
            ConfigureQuestSystem(builder);
            ConfigureJournal(builder);
            ConfigureInventory(builder);
            ConfigureCharacterStateMachine(builder);
        }

        private void ConfigureCommon(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<CoroutineRunner>();
            builder.RegisterComponentInHierarchy<UIDocument>();
            
            builder.Register<ILoader<Object>, ResourcesLoader>(Lifetime.Singleton);
        }

        private void ConfigureServices(IContainerBuilder builder)
        {
            builder.Register<UIService>(Lifetime.Singleton);
        }

        private void ConfigureInput(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<InputSystemUIInputModule>();
            
            builder.Register<PointNClickActions>(Lifetime.Singleton);
            
            builder.UseEntryPoints(entryPoints =>
            {
                entryPoints.Add<InputHandler>();
            });
        }

        private void ConfigurePointNClick(IContainerBuilder builder)
        {
            builder.RegisterInstance(pointNClickConfigSO);

            builder.RegisterComponentInHierarchy<UIElementsCursorManager>().As<ICursorManager>();

            builder.Register<UICursorSensitive.Factory>(Lifetime.Singleton);
            builder.Register<IInteracter, Interacter>(Lifetime.Singleton);
        }

        private void ConfigureCharacter(IContainerBuilder builder)
        {
            builder.RegisterInstance(moverConfigSO);
            
            builder.RegisterComponentInHierarchy<CharacterNavMeshMover>().As<ICharacterMover>();
            
            builder.RegisterComponentInHierarchy<CharacterAnimationParams>();
            builder.Register<CharacterComponents>(Lifetime.Singleton);
        }

        private void ConfigureActionSystem(IContainerBuilder builder)
        {
            builder.Register<ActionResolver>(Lifetime.Singleton);
        }

        private void ConfigureAnimate(IContainerBuilder builder)
        {
            builder.Register<AnimationResolver>(Lifetime.Singleton);
        }
        
        private void ConfigureDialogueSystem(IContainerBuilder builder)
        {
            builder.RegisterInstance(dialogueViewConfigSO);
            
            builder.Register<DVariablesContainer>(Lifetime.Singleton);
            builder.Register<DialogueData>(Lifetime.Singleton);
            builder.Register<DialogueController>(Lifetime.Singleton);
            
            builder.UseEntryPoints(entryPoints =>
            {
                entryPoints.Add<DialogueView>().AsSelf();
            });
        }

        private void ConfigureQuestSystem(IContainerBuilder builder)
        {
            builder.RegisterInstance(questManagerViewConfigSO);
            
            builder.Register<QuestManager>(Lifetime.Singleton);
            
            builder.UseEntryPoints(entryPoints =>
            {
                entryPoints.Add<QuestActionsCollector>();
                entryPoints.Add<QuestManagerView>();
            });
        }
        
        private void ConfigureInventory(IContainerBuilder builder)
        {
            builder.RegisterInstance(inventoryViewConfigSO);
            
            builder.UseEntryPoints(entryPoints =>
            {
                entryPoints.Add<ItemActionsCollector>();
                entryPoints.Add<InventoryView>().AsSelf();
                entryPoints.Add<ItemInfoView>().AsSelf();
            });
        }

        private void ConfigureJournal(IContainerBuilder builder)
        {
            builder.RegisterInstance(journalViewConfigSO);
            builder.RegisterInstance(questPageViewConfigSO);

            builder.Register<JournalQuests>(Lifetime.Singleton);
            
            builder.UseEntryPoints(entryPoints =>
            {
                entryPoints.Add<JournalView>();
                entryPoints.Add<QuestPageView>();
            });
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