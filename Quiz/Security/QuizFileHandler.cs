using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Quiz.Security
{
    public static class QuizFileHandler
    {
        public static void SaveQuizToFile(string filePath, byte[] encryptedData)
        {
            if (string.IsNullOrEmpty(filePath) || encryptedData == null || encryptedData.Length == 0)
            {
                throw new ArgumentNullException("File path or encrypted data cannot be null or empty.");
            }

            try
            {
                File.WriteAllBytes(filePath, encryptedData);
                MessageBox.Show("Quiz saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            } catch (Exception ex)
            {
                MessageBox.Show($"Error saving quiz: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        public static byte[] LoadQuizFromFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException("File path cannot be null or empty.");
            }

            try
            {
                if (!File.Exists(filePath))
                {
                    MessageBox.Show("File not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
                byte[] encryptedData = File.ReadAllBytes(filePath);
                return encryptedData;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading quiz: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
    }
}
