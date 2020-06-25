using System;
using System.Threading.Tasks;
using CodeRetreatScreenSync.ViewModels.Base;

namespace CodeRetreatScreenSync.Services.Navigation
{
    public interface INavigationService
    {
        BaseViewModel PreviousPageViewModel { get; }
        void Initialize();
        Task NavigateToAsync<TViewModel>(bool keepLastFromBackStack) where TViewModel : BaseViewModel;
        Task NavigateToAsync<TViewModel>(bool keepLastFromBackStack, object parameter) where TViewModel : BaseViewModel;
        Task RemoveLastFromBackStackAsync();
        Task RemoveBackStackAsync();
        Task PopToRootAsync();
        bool DoesNavStackContainViewModel(Type viewModelType);
        Task GoBack();
    }
}
