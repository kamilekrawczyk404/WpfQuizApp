using Quiz.Core;

namespace Quiz.Services
{
    public interface INavigationService
    {
        Core.ViewModel CurrentView { get; }
        void NavigateTo<TViewModel>(params object[] parameters) where TViewModel : Core.ViewModel;
    }

    public class NavigationService : ObservableObject, INavigationService
    {
        private Core.ViewModel _currentView;
        private Func<Type, Core.ViewModel> _viewModelFactory;

        public Core.ViewModel CurrentView 
        { 
            get => _currentView; 
            set 
            { 
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public void NavigateTo<TViewModel>(params object[] parameters) where TViewModel : Core.ViewModel
        {

            Core.ViewModel viewModel = _viewModelFactory?.Invoke(typeof(TViewModel));
            CurrentView = viewModel;

        }

        public NavigationService(Func<Type, Core.ViewModel> viewModelFactory)
        {
            // Initialize the navigation service with a function to create view models
            _viewModelFactory = viewModelFactory;
        }
    }
}
