using CsharpHelpers.Interops;
using System;
using System.Diagnostics;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace CsharpHelpers.Helpers
{

    public static class AppHelper
    {

        private static Mutex _mutexApplication;
        private static Mutex _mutexInstance;

        internal static int WM_SHOW_MAIN { get; private set; }


        /// <summary>
        /// AssemblyInfo that is made available throughout the application.
        /// </summary>
        private static AssemblyInfo _assemblyInfo = new AssemblyInfo(typeof(AppHelper).Assembly);
        public static AssemblyInfo AssemblyInfo
        {
            get { return _assemblyInfo; }
            set { _assemblyInfo = value ?? throw new ArgumentNullException(nameof(AssemblyInfo)); }
        }


        /// <summary>
        /// Gets a file path in a data directory.
        /// </summary>
        private static IDataDirectory _dataDirectory = new AppDataLocal(AssemblyInfo.Product);
        public static IDataDirectory DataDirectory
        {
            get { return _dataDirectory; }
            set { _dataDirectory = value ?? throw new ArgumentNullException(nameof(DataDirectory)); }
        }


        /// <summary>
        /// Writes log entry.
        /// </summary>
        private static ILogger _logger = new TextFileLogger(DataDirectory.GetFilePath($"{AssemblyInfo.FileName}.log"));
        public static ILogger Logger
        {
            get { return _logger; }
            set { _logger = value ?? throw new ArgumentNullException(nameof(Logger)); }
        }


        /// <summary>
        /// If set, the unhandled exception will be catched and logged before closing the
        /// application on a message box to inform the user an error occurred.
        /// </summary>
        private static bool _catchUnhandledException;
        public static bool CatchUnhandledException
        {
            get { return _catchUnhandledException; }
            set
            {
                if (value)
                {
                    Application.Current.Dispatcher.UnhandledException += ApplicationUnhandledException;
                    AppDomain.CurrentDomain.UnhandledException += AppDomainUnhandledException;
                    TaskScheduler.UnobservedTaskException += TaskSchedulerUnobservedTaskException;
                }
                else
                {
                    Application.Current.Dispatcher.UnhandledException -= ApplicationUnhandledException;
                    AppDomain.CurrentDomain.UnhandledException -= AppDomainUnhandledException;
                    TaskScheduler.UnobservedTaskException -= TaskSchedulerUnobservedTaskException;
                }

                _catchUnhandledException = value;
            }
        }


        /// <summary>
        /// Returns true if more than one instance of the application is running.
        /// </summary>
        public static bool AnotherInstanceRunning
        {
            get
            {
                var name = Process.GetCurrentProcess().ProcessName;
                var processes = Process.GetProcessesByName(name);
                return processes.Length > 1;
            }
        }


        /// <summary>
        /// This mutex is used to signal that the application is running. As an example, it can be
        /// useful when the AppMutex parameter of Inno Setup is set to the same mutex name.
        /// </summary>
        public static void SetAppMutex(string name)
        {
            if (_mutexApplication != null)
                throw new InvalidOperationException($"The AppMutex can only be set once.");

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Unnamed AppMutex.");

            _mutexApplication = new Mutex(false, name, out _, GetMutexSecurity());
        }


        /// <summary>
        /// This mutex is used for single instance application. It can be a single instance per
        /// user (Local\) or on the machine (Global\). The parameter sendShowMainMessage is used
        /// to bring the window of the already running instance to the foreground (WindowShowMain).
        /// </summary>
        public static void SetInstanceMutex(string name, bool sendShowMainMessage)
        {
            if (_mutexInstance != null)
                throw new InvalidOperationException($"The InstanceMutex can only be set once.");

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Unnamed InstanceMutex.");

            WM_SHOW_MAIN = NativeMethods.RegisterWindowMessage(name);

            _mutexInstance = new Mutex(false, name, out var createdNew, GetMutexSecurity());
            if (!createdNew)
            {
                if (sendShowMainMessage)
                    NativeMethods.PostMessage(NativeConstants.HWND_BROADCAST, WM_SHOW_MAIN, IntPtr.Zero, IntPtr.Zero);

                BeginInvokeShutdown();
            }
        }


        public static void BeginInvokeShutdown()
        {
            Application.Current.Dispatcher.BeginInvoke((Action)Application.Current.Shutdown);
        }


        public static void ExceptionHandler(Exception ex)
        {
            Logger.Write(ex?.ToString() ?? $"The exception parameter from {nameof(ExceptionHandler)} was null.");

            MessageBox.Show($"An unexpected error occurred.\n\n{ex?.Message}", AssemblyInfo.Title, MessageBoxButton.OK, MessageBoxImage.Error);

            BeginInvokeShutdown();
        }


        private static void ApplicationUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            ExceptionHandler(e.Exception);
        }


        private static void AppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (!(e.ExceptionObject is Exception ex))
                ex = new Exception($"Cannot cast {nameof(e.ExceptionObject)} as Exception ({nameof(AppDomainUnhandledException)}).");

            ExceptionHandler(ex);
        }


        private static void TaskSchedulerUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            e.SetObserved();
            ExceptionHandler(e.Exception);
        }


        private static MutexSecurity GetMutexSecurity()
        {
            var securityIdentifier = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
            var mutexAccessRule = new MutexAccessRule(securityIdentifier, MutexRights.FullControl, AccessControlType.Allow);

            var mutexSecurity = new MutexSecurity();
            mutexSecurity.AddAccessRule(mutexAccessRule);

            return mutexSecurity;
        }

    }

}
