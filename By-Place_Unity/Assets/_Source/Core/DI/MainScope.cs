using ActionSystem;
using Animate;
using Animate.Resolve;
using Character;
using Character.Data;
using Character.States;
using Character.View;
using Core.Loaders;
using Core.StateMachine;
using Core.StateMachine.States;
using Cursor.Manager;
using Cursor.Sensitive;
using DefaultNamespace;
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
using Navigation.Location;
using PointNClick.Data;
using QuestSystem;
using QuestSystem.Actions;
using QuestSystem.View;
using QuestSystem.View.Data;
using SaveLoad;
using SaveLoad.Invoker;
using Sound;
using Sound.UI.Data;
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
        [Header("Sound")] 
        [SerializeField] private VisualElementsAudioConfigSO visualElementsAudioConfigSO;
        
        [Header("Navigation")] 
        [SerializeField] private ALocation defaultStartLocation;
        
        [Header("PointNClick")]
        [SerializeField] private PointNClickConfigSO pointNClickConfigSO;
        
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
            ConfigureCore(builder);
            ConfigureSaveLoad(builder);
            ConfigureCommon(builder);
            ConfigureServices(builder);
            ConfigureInput(builder);
            ConfigureAudio(builder);
            ConfigurePointNClick(builder);
            ConfigureNavigation(builder);
            ConfigureCharacter(builder);
            ConfigureActionSystem(builder);
            ConfigureAnimate(builder);
            ConfigureDialogueSystem(builder);
            ConfigureQuestSystem(builder);
            ConfigureJournal(builder);
            ConfigureInventory(builder);
        }

        private void ConfigureCore(IContainerBuilder builder)
        {
            builder.Register<WaitingState>(Lifetime.Singleton);
            builder.Register<CoreLoadState>(Lifetime.Singleton);
            builder.Register<ICoreStatesProvider, CoreStatesProvider>(Lifetime.Singleton);
            
            builder.UseEntryPoints(entryPoints =>
            {
                entryPoints.Add<CoreStateMachine>();
            });
        }

        private void ConfigureSaveLoad(IContainerBuilder builder)
        {
            builder.Register<ISaverLoader, JsonSaverLoader>(Lifetime.Singleton);
            builder.Register<SaveLoadInvoker>(Lifetime.Singleton);
            
            builder.UseEntryPoints(entryPoints =>
            {
                entryPoints.Add<OnDialogueSaver>();
            });
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

        private void ConfigureAudio(IContainerBuilder builder)
        {
            builder.RegisterInstance(visualElementsAudioConfigSO);
            
            builder.RegisterComponentInHierarchy<SourcesCollector>();
            
            builder.Register<AudioPlayer>(Lifetime.Singleton);
            builder.Register<VisualElementsAudio>(Lifetime.Singleton);
        }

        private void ConfigurePointNClick(IContainerBuilder builder)
        {
            builder.RegisterInstance(pointNClickConfigSO);

            builder.RegisterComponentInHierarchy<UIElementsCursorManager>().As<ICursorManager>();

            builder.Register<UICursorSensitive.Factory>(Lifetime.Singleton);
            builder.Register<IInteracter, Interacter>(Lifetime.Singleton);
        }

        private void ConfigureNavigation(IContainerBuilder builder)
        {
            builder.RegisterComponent(defaultStartLocation).As<ALocation>();

            builder.Register<LocationsProvider>(Lifetime.Singleton);
            builder.Register<StartLocationProvider>(Lifetime.Singleton);
            
            builder.UseEntryPoints(entryPoints =>
            {
                entryPoints.Add<LocationsManager>();
            });
        }

        private void ConfigureCharacter(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<CharacterNavMeshMover>().As<ICharacterMover>();
            
            builder.RegisterComponentInHierarchy<AnimatorParamsController>();
            builder.Register<CharacterComponents>(Lifetime.Singleton);
            
            builder.Register<CharacterFreeState>(Lifetime.Singleton);
            builder.Register<CharacterInteractingState>(Lifetime.Singleton);
            builder.Register<ICharacterStatesProvider, CharacterStatesProvider>(Lifetime.Singleton);
            
            builder.UseEntryPoints(entryPoints =>
            {
                entryPoints.Add<CharacterStateMachine>();
            });
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
            
            builder.Register<DialogueData>(Lifetime.Singleton);
            builder.Register<DialogueController>(Lifetime.Singleton);
            
            builder.UseEntryPoints(entryPoints =>
            {
                entryPoints.Add<DVariablesContainer>().AsSelf();
                
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
    }
}