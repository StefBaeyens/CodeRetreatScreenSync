using System;
using CodeRetreatScreenSync.Services.Navigation;
using CodeRetreatScreenSync.ViewModels.Base;
using Microsoft.Extensions.DependencyInjection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CodeRetreatScreenSync
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }
        public App()
        {
            InitializeComponent();

            ConfigureServiceProvider();
        }

        private static void ConfigureServiceProvider()
        {
            var services = new ServiceCollection();

            services.AddSingleton<INavigationService, NavigationService>();
            //TODO
            //services.AddSingleton<IAnimationService, AnimationService>();

            services.Scan(x => x.FromAssemblyOf<App>()
            .AddClasses(f => f.AssignableTo(typeof(BaseViewModel)))
            .AsSelf().WithTransientLifetime()
            );

            services.Scan(x => x.FromAssemblyOf<NavigationService>()
            .AddClasses(f => f.AssignableTo(typeof(BaseViewModel)))
            .AsSelf().WithTransientLifetime()
            );

            ServiceProvider = services.BuildServiceProvider();
        }

        protected override async void OnStart()
        {
            var navigationService = ServiceProvider.GetRequiredService<INavigationService>();
            navigationService.Initialize();

            //TODO
            //await navigationService.NavigateToAsync<AnimationViewModel>(false);
        }
    }
}
