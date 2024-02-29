using ActionSystem;
using Character;
using Character.Data;
using Character.States;
using Character.View;
using DialogueSystem.Data;
using DialogueSystem.Resolve;
using Input;
using InputSystem;
using Journal.Quest;
using Journal.Quest.View;
using Journal.Quest.View.Data;
using Journal.View;
using Journal.View.Data;
using Movement;
using Movement.Data;
using NPC.View;
using PointNClick.Cursor.Manager;
using PointNClick.Cursor.Sensitive;
using PointNClick.Data;
using PointNClick.Interactions;
using PointNClick.Items.Actions;
using PointNClick.Items.View;
using QuestSystem;
using QuestSystem.Actions;
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
            ConfigureDialogueSystem(builder);
            ConfigureQuestSystem(builder);
            ConfigureJournal(builder);
            ConfigureInventory(builder);
            ConfigureNPC(builder);
            ConfigureCharacterStateMachine(builder);
        }

        private void ConfigureCommon(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<CoroutineRunner>();
            builder.RegisterComponentInHierarchy<UIDocument>();
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
            builder.RegisterInstance(moverConfigSO);

            builder.RegisterComponentInHierarchy<UIElementsCursorManager>().As<ICursorManager>();

            builder.RegisterComponentInHierarchy<NavMeshMover>().As<IMover>();

            builder.Register<UICursorSensitive.Factory>(Lifetime.Singleton);
            builder.Register<IInteracter, Interacter>(Lifetime.Singleton);
        }

        private void ConfigureCharacter(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<CharacterAnimationParams>();
            builder.Register<CharacterComponents>(Lifetime.Singleton);
        }

        private void ConfigureDialogueSystem(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<DialogueView>();
            
            builder.Register<DVariablesContainer>(Lifetime.Singleton);
            builder.Register<ActionResolver>(Lifetime.Singleton);
            builder.Register<DialogueData>(Lifetime.Singleton);
            builder.Register<DialogueController>(Lifetime.Singleton);
        }

        private void ConfigureQuestSystem(IContainerBuilder builder)
        {
            builder.Register<QuestManager>(Lifetime.Singleton);
            
            builder.UseEntryPoints(entryPoints =>
            {
                entryPoints.Add<QuestActionsCollector>();
            });
        }
        
        private void ConfigureInventory(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<InventoryView>();
            
            builder.UseEntryPoints(entryPoints =>
            {
                entryPoints.Add<ItemActionsCollector>();
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

        private void ConfigureNPC(IContainerBuilder builder)
        {
            builder.Register<NPCsAnimators>(Lifetime.Singleton);
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