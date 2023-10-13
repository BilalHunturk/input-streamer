using MouseTracer.Palettes;
using MouseTracer.WindowService;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MouseTracer
{
    public partial class MainWindow : Form
    {
        private Tracer art;
        private StatCollector stats;

        private bool running = false;
        private bool unsavedChanges = false;
        private ColorPalette currentPalette;
        WindowManager windowManager;

        public MainWindow()
        {
            InitializeComponent();

            windowManager = new WindowManager();
            windowManager.SetIdToWındow();
            windowManager.getWindowsCurrentPosition();
            dataGridView.DataSource = StaticWindowConfig.Windows;

            //windowListBox.Items.AddRange(StaticWindowConfig.Windows.ToArray());
            Task.Run(() => SetStatusBarForStatic());
            ResetTrace();
        }

        private void ResetTrace()
        {
            SetRunning(false);
            unsavedChanges = false;

            art?.Dispose();
            art = new Tracer();
            stats?.Dispose();
            stats = new StatCollector();

            Refresh();
        }

        private void SetRunning(bool run)
        {
            art?.SetRunning(run);
            stats?.SetRunning(run);
            if (run)
            {
                unsavedChanges = true;
            }
            running = run;
            startToolStripMenuItem.Enabled = !run;
            pauseToolStripMenuItem.Enabled = run;
            
        }


        protected override void OnPaint(PaintEventArgs e)
        {
        }

		private void redrawTimer_Tick(object sender, EventArgs e)
		{
			if (running && WindowState != FormWindowState.Minimized)
			{
				Refresh();
			}
		}

		private void TrayIcon_MouseClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void MainWindow_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                trayIcon.Visible = true;
                Hide();
            }
            else
            {
                trayIcon.Visible = false;
                Show();
            }
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView.CurrentCell = null;
            if (StaticWindowConfig.Windows.FindAll(window => window.IsMain).Count > 1)
            {
                MessageBox.Show("There should not be more then 1 Main Window\nPlease Just choose one Main Window");
                return;
            }
            if (StaticWindowConfig.GetWindowsIfToggled().Count == 0)
            {
                MessageBox.Show("You should choose at least 1 window to work this bot. For that you should click one of the windows IsToggled.\n If you dont know how to work with this bot please contact with the creator.");
                return;
            }
            windowManager.getWindowsCurrentPosition();
            SetRunning(true);
            SetStatusBar(true, true);
            // var firstWindow = StaticWindowConfig.Windows.Find(window => !window.IsToggled).IsMain = true;

        }

        private void SetStatusBar(bool mouseStatus,bool keyboardStatus)
        {
                SetMouseStatusBar(mouseStatus);
                SetKeyboardStatusBar(keyboardStatus);
        }
        private void SetStatusBarForStatic( )
        {
            try
            {
                while (true)
                {
                    SetMouseStatusBar(StaticWindowConfig.MouseWork);
                    SetKeyboardStatusBar(StaticWindowConfig.KeyboardWork);
                    Thread.Sleep(1000);
                    Console.WriteLine(StaticWindowConfig.MouseWork+" is mouse work\n"+StaticWindowConfig.KeyboardWork+" is Keyboard");
                }

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }

        }

        // If the checkbox is toggled, do it true.
        private void IsCheckBoxToggled(Window window, object item)
        {
            if (window.Id == (int)item)
                window.IsToggled = true;
        }
        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetRunning(false);
            SetStatusBar(false,false);
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private DialogResult ConfirmSaveDialog(string reason, string title)
        {
            if (!unsavedChanges)
            {
                return DialogResult.OK;
            }
            var choice = MessageBox.Show(reason, title, MessageBoxButtons.YesNoCancel);
            if (choice == DialogResult.Yes)
            {
                var didSave = ShowFileSaveDialog();
                if (didSave == DialogResult.OK)
                {
                    return DialogResult.OK;
                }
            }
            else if (choice == DialogResult.No)
            {
                return DialogResult.OK;
            }
            return DialogResult.Cancel;
        }

        private DialogResult ShowFileSaveDialog()
        {
            var saveDlg = new SaveFileDialog
            {
                FileName = string.Format("Trace ({0:%h} hours {0:%m} minutes {0:%s} seconds).png", stats.TimeTracing),
                Filter = "PNG Image|*.png",
                AddExtension = true
            };

            var dlgResult = saveDlg.ShowDialog();
            if (dlgResult == DialogResult.OK)
            {
                art.Image.Save(saveDlg.FileName);
                if (!running)
                {
                    unsavedChanges = false;
                }
            }
            return dlgResult;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowFileSaveDialog();
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
               
            }
        }

        private void statisticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stats.DisplayStats();
        }

        private void drawClicksToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void drawPathToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        public void SetMouseStatusBar(bool run)
        {
            if (!run)
            {
                MouseStatusLabel.Text = "Stopped";
                MouseStatusLabel.ForeColor = Color.Red;
                StaticWindowConfig.MouseWork = false;
                return;
            }
            MouseStatusLabel.Text = "Working";
            MouseStatusLabel.ForeColor = Color.Green;
            StaticWindowConfig.MouseWork = true;
        }

        public void SetKeyboardStatusBar(bool run)
        {
            if (!run)
            {
                keyboardStatusLabel.Text = "Stopped";
                keyboardStatusLabel.ForeColor = Color.Red;
                StaticWindowConfig.KeyboardWork = false;
                return;
            }
            keyboardStatusLabel.Text = "Working";
            keyboardStatusLabel.ForeColor = Color.Green;
            StaticWindowConfig.KeyboardWork = true;
        }

        private void windowListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        
    }
}
