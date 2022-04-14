using PongServer.Manager;
using PongServer.MyEventArgs;
using PongServer.Util;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PongServer
{
    public partial class MainForm : Form
    {
        private Task task;
        private CancellationTokenSource cancellationTokenSource;

        private Server server;
        private ManagerLogger managerLogger;

        public LogCategory LogMode = LogCategory.Info;

        public MainForm()
        {
            managerLogger = new ManagerLogger(LogMode);
            managerLogger.NewLogMessageEvent += NewLogMessageEvent;
            InitializeComponent();
            server = new Server(managerLogger);
        }

        private void NewLogMessageEvent(object sender, LogMessageEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new EventHandler<LogMessageEventArgs>(NewLogMessageEvent), sender, e);
            }
            else
            {
                dgvServeurStatusLog.Rows.Add(new [] { e.LogMessage.LogCategory.Value, e.LogMessage.Id, e.LogMessage.Message });
            }
        }

        private void btnStartServer_Click(object sender, EventArgs e)
        {
            btnStartServer.Enabled = false;
            btnStopServer.Enabled = true;

            cancellationTokenSource = new CancellationTokenSource();
            task = new Task(server.Run, cancellationTokenSource.Token);
            task.Start();
        }

        private void btnStopServer_Click(object sender, EventArgs e)
        {
            if (task != null && cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                btnStartServer.Enabled = true;
                btnStopServer.Enabled = false;
            }
        }

        private void btnStartServer_Click_1(object sender, EventArgs e)
        {
            btnStartServer.Enabled = false;
            btnStopServer.Enabled = true;

            cancellationTokenSource = new CancellationTokenSource();
            task = new Task(server.Run, cancellationTokenSource.Token);
            task.Start();
        }

        private void btnStopServer_Click_1(object sender, EventArgs e)
        {

        }
    }
}
