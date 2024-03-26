using Animate.Resolve;
using Popup;
using Popup.Data;
using Popup.Pool;
using Popup.Timeline;
using Sound;
using UnityEngine;
using UnityEngine.UIElements;
using Utils.Pooling;
using Utils.Services;
using VContainer;
using VContainer.Unity;

namespace Core.DI
{
    public class CutSceneScope : LifetimeScope
    {
        [Header("Popup")] 
        [SerializeField] private PopupConfigSO popupConfigSO;
        
        protected override void Configure(IContainerBuilder builder)
        {
            ConfigureServices(builder);
            ConfigureUI(builder);
            ConfigureAudio(builder);
            ConfigureAnimate(builder);
            ConfigurePopup(builder);
        }

        private void ConfigureServices(IContainerBuilder builder)
        {
            builder.Register<SceneService>(Lifetime.Singleton);
        }
        
        private void ConfigureUI(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<UIDocument>();
        }
        
        private void ConfigureAudio(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<SourcesCollector>();
            
            builder.Register<AudioPlayer>(Lifetime.Singleton);
        }
        
        private void ConfigureAnimate(IContainerBuilder builder)
        {
            builder.Register<AnimationResolver>(Lifetime.Singleton);
        }
        
        private void ConfigurePopup(IContainerBuilder builder)
        {
            builder.RegisterInstance(popupConfigSO);
            
            builder.RegisterComponentInHierarchy<TimelinePopupBehaviorsInjector>();
            
            builder.Register<PopupManagersResolver>(Lifetime.Singleton);
            
            builder.Register<PoolConfig<PopupElement>, PopupPoolConfig>(Lifetime.Singleton);
            builder.Register<DefaultPool<PopupElement>>(Lifetime.Singleton);
        }
    }
}