using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfAppErrorHandler
{
    public enum ExceptionCategory
    {
        General,
        Input,
    }

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HandleButtonClick();
            }
            catch (InvalidOperationException ex)
            {
                HandleException(ex, ExceptionCategory.Input, "Button_Click");
            }
        }

        private void HandleButtonClick()
        {
            string inputText = MyTextBox.Text;

            if (string.IsNullOrEmpty(inputText))
            {
                throw new InvalidOperationException("Input text cannot be empty.");
            }
            if (inputText.Contains(".") || inputText.Contains(","))
            {
                throw new InvalidOperationException("Input text must be a valid numeric value.");
            }

            if (inputText.Any(c => !char.IsDigit(c) && c != '.' && c != ','))
            {
                throw new InvalidOperationException("Input text must contain only numeric characters.");
            }

            int result = 0;
            ExceptionCategory category = ExceptionCategory.General;
            try
            {
                if (true)
                {
                    var random = new Random();
                    int randomNumber = random.Next(0, 100);
                    result = int.Parse((inputText) + randomNumber) / 2;

                }
                ThrowOkRequestException(result.ToString(), category);
            }
            catch
            {
                Label_Results.Content = result.ToString();
            }
        }

        private void HandleException(Exception e, ExceptionCategory category, string source)
        {
            try
            {
                string generalMessage = GetGeneralMessage(e.Message);

                if (category == ExceptionCategory.Input)
                {
                    ThrowBadRequestException(generalMessage);
                }
                //else if (category == ExceptionCategory.General)
                //{
                //    ThrowOkRequestException(generalMessage);
                //}

            }
            catch (HttpRequestException ex)
            {
                HandleHttpRequestException(ex);
            }
        }

        private void ThrowBadRequestException(string generalMessage)
        {
            var httpException = new HttpRequestException(generalMessage);
            httpException.Data["HttpStatusCode"] = System.Net.HttpStatusCode.BadRequest;
            throw httpException;
        }
        private void ThrowOkRequestException(string generalMessage, ExceptionCategory category)
        {
            if (category == ExceptionCategory.General)
            {
                var httpException = new HttpRequestException(generalMessage);
                httpException.Data["HttpStatusCode"] = System.Net.HttpStatusCode.OK;
                throw httpException;
            }
        }

        private void HandleHttpRequestException(HttpRequestException ex)
        {
            if (ex.Data.Contains("HttpStatusCode"))
            {
                var statusCode = (System.Net.HttpStatusCode)ex.Data["HttpStatusCode"]!;
                MessageBox.Show($"Status Code: {statusCode}", "Unexpected Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show($"HttpRequestException: {ex.Message}", "Unexpected Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string GetGeneralMessage(string exceptionMessage)
        {
            if (exceptionMessage == "Input text cannot be empty.")
            {
                var httpException = new HttpRequestException(exceptionMessage);
                httpException.Data["HttpStatusCode"] = System.Net.HttpStatusCode.BadRequest;
                throw httpException;
            }

            if (exceptionMessage == "Input text must contain only numeric characters.")
            {
                var httpException = new HttpRequestException(exceptionMessage);
                httpException.Data["HttpStatusCode"] = System.Net.HttpStatusCode.NoContent;
                throw httpException;
            }

            if (exceptionMessage == "Input text must be a valid numeric value.")
            {
                var httpException = new HttpRequestException(exceptionMessage);
                httpException.Data["HttpStatusCode"] = System.Net.HttpStatusCode.BadRequest;
                throw httpException;
            }
            return exceptionMessage;
        }

        private void MyTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Your existing TextChanged event handling code
            e.Handled = true;
        }
    }
}
