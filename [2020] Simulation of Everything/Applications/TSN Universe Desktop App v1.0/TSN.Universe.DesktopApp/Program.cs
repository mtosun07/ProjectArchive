using System;
using System.Windows.Forms;

namespace TSN.Universe.DesktopApp
{
    public static class Program
    {
        static Program()
        {
            _locker = new object();
            _mainFormInstance = new Lazy<MainForm>(() => new MainForm(), true);
            _applicationModel = new ApplicationModel();
            Application.ApplicationExit += Application_ApplicationExit;
        }


        private static readonly object _locker;
        private static readonly Lazy<MainForm> _mainFormInstance;
        private static ApplicationModel _applicationModel;

        internal static MainForm MainFormInstance => _mainFormInstance.Value;
        internal static ApplicationModel ApplicationModel
        {
            get => _applicationModel;
            set
            {
                lock (_locker)
                {
                    if (value == null)
                        throw new NullReferenceException();
                    _applicationModel.Universe = null;
                    _applicationModel = value;
                    GC.Collect();
                    ApplicationModelChanged?.Invoke(null, EventArgs.Empty);
                }
            }
        }

        public static EventHandler<EventArgs> ApplicationModelChanged;



        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread] private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(MainFormInstance);
        }

        private static void Application_ApplicationExit(object sender, EventArgs e) => _applicationModel.Universe = null;
    }
}