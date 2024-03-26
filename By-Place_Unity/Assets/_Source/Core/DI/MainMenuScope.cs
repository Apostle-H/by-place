using MainMenu;
using UnityEngine.UIElements;
using Utils.Services;
using VContainer;
using VContainer.Unity;

namespace Core.DI
{
    public class MainMenuScope : LifetimeScope 
    {
        protected override void Configure(IContainerBuilder builder)
        {
            ConfigureServices(builder);
            ConfigureUI(builder);
            ConfigureMainMenu(builder);
        }

        private void ConfigureServices(IContainerBuilder builder)
        {
            builder.Register<SceneService>(Lifetime.Singleton);
        }

        private void ConfigureUI(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<UIDocument>();
        }

        private void ConfigureMainMenu(IContainerBuilder builder)
        {
            builder.UseEntryPoints(entryPoints =>
            {
                entryPoints.Add<MainMenuView>().AsSelf();
                entryPoints.Add<MainMenuController>();
            });
        }
    }
}