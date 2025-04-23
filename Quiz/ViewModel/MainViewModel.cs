using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quiz.Core;

namespace Quiz.ViewModel
{
    class MainViewModel : ObservableObject
    {
        public RelayCommand GeneratorViewCommand { get; set; }
        public RelayCommand QuizViewCommand { get; set; }
        public GeneratorViewModel GeneratorVM{ get; set; }
        public QuizViewModel QuizVM{ get; set; }
        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
        public MainViewModel() {
            GeneratorVM = new GeneratorViewModel();
            QuizVM = new QuizViewModel();

            CurrentView = GeneratorVM;

            GeneratorViewCommand = new RelayCommand(o =>
            {
                CurrentView = GeneratorVM;
            });

            QuizViewCommand = new RelayCommand(o =>
            {
                CurrentView = QuizVM;
            });
        }
    }
}
