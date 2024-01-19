//using System.Configuration;
//using System.Data;
//using System.Diagnostics;
//using System.Windows;

//namespace WpfAppErrorHandler
//{
//    /// <summary>
//    /// Interaction logic for App.xaml
//    /// </summary>
//    public partial class App : Application
//    {
//        public App() : base()
//        {
//            SetupUnhandledExceptionHandling();
//        }

//        private void SetupUnhandledExceptionHandling()
//        {
//            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
//                ShowUnhandledException(args.ExceptionObject as Exception, "AppDomain.CurrentDomain.UnhandledException", false);

//            TaskScheduler.UnobservedTaskException += (sender, args) =>
//                ShowUnhandledException(args.Exception, "TaskScheduler.UnobservedTaskException", false);

//            Dispatcher.UnhandledException += (sender, args) =>
//            {
//                if (!Debugger.IsAttached)
//                {
//                    args.Handled = true;
//                    ShowUnhandledException(args.Exception, "Dispatcher.UnhandledException", true);
//                }
//            };
//        }

//        void ShowUnhandledException(Exception e, string unhandledExceptionType, bool promptUserForShutdown)
//        {
//            var messageBoxTitle = $"Unexpected Error Occurred: {unhandledExceptionType}";
//            var messageBoxMessage = $"The following exception occurred:\n\n{e}";
//            var messageBoxButtons = MessageBoxButton.OK;

//            if (promptUserForShutdown)
//            {
//                messageBoxMessage += "\n\nNormally the app would die now. Should we let it die?";
//                messageBoxButtons = MessageBoxButton.YesNo;
//            }

//            if (MessageBox.Show(messageBoxMessage, messageBoxTitle, messageBoxButtons) == MessageBoxResult.Yes)
//            {
//                Application.Current.Shutdown();
//            }
//        }
//    }

//}
