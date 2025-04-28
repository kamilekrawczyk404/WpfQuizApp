using Microsoft.Win32;
using Quiz.Core;
using Quiz.Model;
using Quiz.Security;
using Quiz.Services;
using Quiz.Stores;
using Quiz.View;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace Quiz.ViewModel
{
    public class GeneratorViewModel : Core.ViewModel
    {
        // commands used in the view model
        public ICommand AddQuestionCommand { get; set; }
        public ICommand RemoveQuestionCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand EditQuestionCommand { get; set; }
        public ICommand DecryptQuizCommand { get; set; }
        public ICommand EncryptQuizCommand { get; set; }

        // quiz properties 
        private QuizModel _quiz;
        public QuizModel Quiz
        {
            get { return _quiz; }
            set
            {
                _quiz = value;
                OnPropertyChanged();
            }
        }

        private Question _selectedQuestion;
        public Question SelectedQuestion
        {
            get { return _selectedQuestion; }
            set
            {
                _selectedQuestion = value;
                OnPropertyChanged();
            }
        }

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

        private readonly ObservableCollection<Question> _questions;
        public ObservableCollection<Question> Questions => _questions;
        
        private readonly string _aesPassword = "QuizPassword";

        private QuizStore _quizStore;
        
        public GeneratorViewModel(INavigationService nav, QuizStore quizStore)
        {
            _quiz = quizStore.CurrentQuiz;

            _navigation = nav;
            _questions = new ObservableCollection<Question>();

            _quizStore = quizStore;
            _quizStore.QuestionCreated += OnQuestionSaved;
            _quizStore.QuestionEdited += OnQuestionEdited;
            _quizStore.QuizCreated += OnQuizCreated;

            AddQuestionCommand = new RelayCommand(AddQuestion);
            RemoveQuestionCommand = new RelayCommand(RemoveQuestion);
            EditQuestionCommand = new RelayCommand(EditQuestion);
            EncryptQuizCommand = new RelayCommand(EncryptQuiz);
            DecryptQuizCommand = new RelayCommand(DecryptQuiz);
        }

        private void DecryptQuiz(object obj)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                byte[] encryptedData = QuizFileHandler.LoadQuizFromFile(openFileDialog.FileName);
                QuizModel quiz = AesEncryption.DecryptQuizModel(encryptedData, _aesPassword);

                _quizStore.CurrentQuiz = quiz;
                Quiz = quiz;
                _questions.Clear();

                foreach (var question in quiz.Questions)
                {
                    _questions.Add(new Question()
                    {
                        Title = question.Title,
                        Points = question.Points,
                        Type = question.Type,
                        Answers = question.Answers
                    });
                }

                MessageBox.Show("Quiz has been successfully loaded.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void EncryptQuiz(object obj)
        {
            if (Quiz.Title.Length == 0)
            {
               MessageBox.Show("Quiz title is required.", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }
            if (Questions.Count() < 1)
            {
                MessageBox.Show("At least one question is required.", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }

            _quizStore.CreateQuiz(Quiz);

            // quiz must be saved before encryption!
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Txt files (*.txt)|*.txt|All files (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == true)
            {
                byte[] encryptedData = AesEncryption.EncryptQuizModel(_quizStore.CurrentQuiz, _aesPassword);
                QuizFileHandler.SaveQuizToFile(saveFileDialog.FileName, encryptedData);
            }
        }

        private void OnQuizCreated(QuizModel quiz)
        {
            // add questions that are stored under Questions collection
            QuizModel savedQuiz = new QuizModel
            {
                Title = quiz.Title,
                Questions = Questions
            };

            _quizStore.CurrentQuiz = savedQuiz;
        }

        private void AddQuestion(object obj)
        {
            Question newQuestion = new Question();

            _quizStore.CurrentQuestion = newQuestion;
            _navigation.NavigateTo<EditQuestionViewModel>();
        }

        private void EditQuestion(object obj)
        {
            if (SelectedQuestion == null) return;

            _quizStore.CurrentQuestion = SelectedQuestion;
            _navigation.NavigateTo<EditQuestionViewModel>();
        }

        private void RemoveQuestion(object obj)
        {
            if (obj is Question questionToRemove)
            {
                _questions.Remove(SelectedQuestion);
                SelectedQuestion = null;
            }            
        }

        private void OnQuestionSaved(Question newQuestion)
        {
            _questions.Add(new Question()
            {
                Title = newQuestion.Title,
                Points = newQuestion.Points,
                Type = newQuestion.Type,
                Answers = newQuestion.Answers
            });

            SelectedQuestion = null;
        }

        private void OnQuestionEdited(Question editedQuestion)
        {
            if (SelectedQuestion == null) return;
            int index = _questions.IndexOf(SelectedQuestion);

            _questions[index] = editedQuestion;

            SelectedQuestion = null;
        }
    }
}
