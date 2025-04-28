using Quiz.Core;
using Quiz.Model;
using Quiz.Services;
using Quiz.Stores;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace Quiz.ViewModel
{
    public class EditQuestionViewModel : Core.ViewModel
    {

        // View model properties
        private string _windowTitle;
        public string WindowTitle
        {
            get { return _windowTitle; }
            set
            {
                _windowTitle = value;
                OnPropertyChanged();
            }
        }

        private Question _question;
        public Question Question
        {
            get { return _question; }
            set
            {
                _question = value;
                OnPropertyChanged();
            }
        }

        private string _saveButtonText;
        public string SaveButtonText
        {
            get { return _saveButtonText; }
            set
            {
                _saveButtonText = value;
                OnPropertyChanged();
            }
        }

        private bool _editingQuestion;
        public bool EditingQuestion
        {
            get { return _editingQuestion; }
            set
            {
                _editingQuestion = value;
                OnPropertyChanged();
            }
        }

        private bool _canMoveToPreviousQuestion;
        public bool CanMoveToPreviousQuestion
        {
            get { return _canMoveToPreviousQuestion; }
            set
            {
                _canMoveToPreviousQuestion = value;
                OnPropertyChanged();
            }
        }

        private bool _canMoveToNextQuestion;
        public bool CanMoveToNextQuestion
        {
            get { return _canMoveToNextQuestion; }
            set
            {
                _canMoveToNextQuestion = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<QuestionType> QuestionTypes { get; } = new ObservableCollection<QuestionType>(
            Enum.GetValues(typeof(QuestionType)).Cast<QuestionType>().ToList()
        );

        private INavigationService _navigation;
        public INavigationService Navigation
        {
            get { return _navigation; }
            set
            {
                _navigation = value;
                OnPropertyChanged();
            }
        }

        private QuizStore _quizStore;

        // View model commands
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand NextQuestionCommand { get; }
        public ICommand PreviousQuestionCommand { get; }

        public EditQuestionViewModel(INavigationService nav, QuizStore quizStore)
        {
            Trace.WriteLine(quizStore.CurrentQuestion.Title);
            Trace.WriteLine("Title");
            _navigation = nav;
            _quizStore = quizStore;

            Question = _quizStore.CurrentQuestion ?? new Question();

            if (!string.IsNullOrEmpty(Question.Title))
            {
                EditingQuestion = true;
                WindowTitle = "Editing Question";
                SaveButtonText = "Save";
            }
            else
            {
                EditingQuestion = false;
                WindowTitle = "Adding Question";
                SaveButtonText = "Add";
            }

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);

            NextQuestionCommand = new RelayCommand(NextQuestion);
            PreviousQuestionCommand = new RelayCommand(PreviousQuestion);
        }

        private void Save(object obj)
        {
            if (string.IsNullOrEmpty(Question.Title))
            {
                System.Windows.MessageBox.Show("Question text cannot be empty.", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }
            if (Question.Points <= 0)
            {
                System.Windows.MessageBox.Show("Points must be greater than 0.", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }
            if (Question.Answers.Any(a => string.IsNullOrEmpty(a.Text)))
            {

                System.Windows.MessageBox.Show("Answear text cannot be empty.", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }
            if (!Question.Answers.Any(a => a.IsCorrect == true))
            {
                System.Windows.MessageBox.Show("At least one answear must be correct.", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }
            if (Question.Type == QuestionType.SingleCorrect && Question.Answers.Where(a => a.IsCorrect == true).Count() > 1)
            {
                System.Windows.MessageBox.Show("Only one answear can be correct.", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }
            if (Question.Type == QuestionType.MultipleCorrect && Question.Answers.Where(a => a.IsCorrect == true).Count() < 2)
            {
                System.Windows.MessageBox.Show("At least two answears must be correct.", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }

            if (_editingQuestion)
            {
                _quizStore.EditQuestion(Question);
            } else
            {
                _quizStore.CreateQuestion(Question);
            }

            _navigation.NavigateTo<GeneratorViewModel>();
        }

        private void Cancel(object obj)
        {
            _navigation.NavigateTo<GeneratorViewModel>();
        }

        private void NextQuestion(object obj)
        {
        }

        private void PreviousQuestion(object obj)
        {
        }
    }
}
