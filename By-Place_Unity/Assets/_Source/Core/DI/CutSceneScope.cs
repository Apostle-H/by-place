using Popup;
using Popup.Data;
using Popup.Pool;
using Popup.Timeline;
using UnityEngine;
using Utils.Pooling;
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
            ConfigurePopup(builder);
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