using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CodeRetreatScreenSync.ViewModels.Base;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CodeRetreatScreenSync.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        public BaseViewModel PreviousPageViewModel
        {
            get
            {
                var mainPage = Application.Current.MainPage as MainPage;
                var viewModel = mainPage?.Navigation.NavigationStack[mainPage.Navigation.NavigationStack.Count - 2].BindingContext;
                return viewModel as BaseViewModel;
            }
        }

        public void Initialize()
        {
        }

        public async Task NavigateToAsync<TViewModel>(bool keepLastFromBackStack) where TViewModel : BaseViewModel
        {
            await NavigateToAsync<TViewModel>(keepLastFromBackStack, null);
        }
        public async Task NavigateToAsync<TViewModel>(bool keepLastFromBackStack, object parameter) where TViewModel : BaseViewModel
        {
            await InternalNavigateToAsync(typeof(TViewModel), parameter);
            if (!keepLastFromBackStack)
            {
                await RemoveLastFromBackStackAsync();
            }
        }

        public async Task GoBack()
        {
            if (Application.Current.MainPage is NavigationPage mainPage)
            {
                if (MainThread.IsMainThread)
                {
                    await mainPage.Navigation.PopAsync();
                }
                else
                {
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await mainPage.Navigation.PopAsync();
                    });
                }
            }
        }

        public Task RemoveLastFromBackStackAsync()
        {
            if (Application.Current.MainPage is NavigationPage mainPage)
            {
                if (MainThread.IsMainThread)
                {
                    if (mainPage.Navigation.NavigationStack.Count > 1)
                        mainPage.Navigation.RemovePage(mainPage.Navigation.NavigationStack[mainPage.Navigation.NavigationStack.Count - 2]);
                }
                else
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        if (mainPage.Navigation.NavigationStack.Count > 1)
                            mainPage.Navigation.RemovePage(mainPage.Navigation.NavigationStack[mainPage.Navigation.NavigationStack.Count - 2]);
                    });
                }
            }

            return Task.FromResult(true);
        }

        public Task RemoveBackStackAsync()
        {
            if (Application.Current.MainPage is NavigationPage mainPage)
            {
                int stackCount = mainPage.Navigation.NavigationStack.Count;
                for (var i = 0; i < stackCount - 1; i++)
                {
                    var page = mainPage.Navigation.NavigationStack[0];
                    mainPage.Navigation.RemovePage(page);
                }
            }

            return Task.FromResult(true);
        }

        public async Task PopToRootAsync()
        {
            if (Application.Current.MainPage is NavigationPage mainPage)
            {
                if (MainThread.IsMainThread)
                {
                    await mainPage.Navigation.PopToRootAsync();
                }
                else
                {
                    await MainThread.InvokeOnMainThreadAsync(async () =>
                    {
                        await mainPage.Navigation.PopToRootAsync();
                    });
                }
            }
        }

        private async Task InternalNavigateToAsync(Type viewModelType, object parameter)
        {
            var page = CreatePage(viewModelType, parameter);
            if (Application.Current.MainPage is NavigationPage navigationPage)
            {
                if (MainThread.IsMainThread)
                {
                    await navigationPage.PushAsync(page);
                }
                else
                {
                    await MainThread.InvokeOnMainThreadAsync(async () =>
                    {
                        await navigationPage.PushAsync(page);
                    });
                }
            }
            else
            {
                Application.Current.MainPage = new NavigationPage(page);
            }
        }

        public bool DoesNavStackContainViewModel(Type viewModelType)
        {
            var pageType = GetPageTypeForViewModel(viewModelType);
            if (Application.Current.MainPage?.Navigation?.NavigationStack == null) return false;
            return Application.Current.MainPage.Navigation.NavigationStack.Any(p => p.GetType() == pageType);
        }

        private Type GetPageTypeForViewModel(Type viewModelType)
        {

            var viewName = viewModelType?.FullName?.Replace("Model", string.Empty);
            var viewModelAssemblyName = viewModelType.GetTypeInfo().Assembly.GetName();
            var viewAssemblyName = string.Format(CultureInfo.InvariantCulture, "{0}, {1}", viewName, viewModelAssemblyName);
            var viewType = Type.GetType(viewAssemblyName);
            return viewType;
        }

        private Page CreatePage(Type viewModelType, object parameter)
        {
            Type pageType = GetPageTypeForViewModel(viewModelType);
            if (pageType == null)
            {
                throw new Exception($"Cannot locate page type for {viewModelType}");
            }

            var page = Activator.CreateInstance(pageType) as Page;

            if (parameter != null)
            {
                NavigationContext.SetParam(page, parameter);
            }

            return page;
        }

    }
}
