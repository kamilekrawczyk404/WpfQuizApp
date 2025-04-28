using System.Diagnostics;
using Quiz.Core;
using Quiz.Services;
using Quiz.Stores;

namespace Quiz.ViewModel
{
    public class MainViewModel : Core.ViewModel
    {
        private INavigationService _navigation;

        public INavigationService Navigation
        {
            get => _navigation;
            set
            {
                _navigation = value;
                OnPropertyChanged();
            }
        }

        private QuizStore _quizStore;
        public RelayCommand NavigateGeneratorCommand { get; }
        public RelayCommand NavigateQuizzesCommand { get; }

        public MainViewModel(INavigationService navService, QuizStore quizStore) 
        {
            _navigation = navService;
            _quizStore = quizStore;

            // default view model
            _navigation.NavigateTo<GeneratorViewModel>();

            NavigateGeneratorCommand = new RelayCommand(o => { Navigation.NavigateTo<GeneratorViewModel>(); });
            NavigateQuizzesCommand = new RelayCommand(o => { Navigation.NavigateTo<QuizViewModel>(); });
        }
    }
}
