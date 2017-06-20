using System;
using System.Threading;
using System.Windows.Forms;
using WCFChessService;
namespace WCFChess
{
    public partial class GameSessionSelect : Form
    {
        private WCFCommunicationWrapper _wcf;


        public GameSessionSelect()
        {
            InitializeComponent();
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            if (_wcf == null)
            {
                _wcf = new WCFCommunicationWrapper(nickTextBox.Text);
                _wcf.Connect();
            }

            var s = _wcf.GetAvailableSession();
            foreach (var session in s)
            {
                var listItem = new ListViewItem(session.SessionID.ToString());
                listItem.SubItems.Add(session.PlayerA);
                avaliableSessionsView.Items.Add(listItem);
            }

            hostButton.Enabled = true;
            playRemoteButton.Enabled = true;

            connectButton.Text = "Refresh";

        }

        private void playLocalButton_Click(object sender, EventArgs e)
        {
            var gameWindow = new MainWindow();
            gameWindow.Show();

            this.Hide();
        }

        private void hostButton_Click(object sender, EventArgs e)
        {
            if (_wcf.HostGame())
            {
                var gameWindow = new MainWindow(nickTextBox.Text, _wcf.SessionGUID, GameTypeEnum.Remote_host, _wcf);
                gameWindow.Show();

                this.Hide();
            }
            else
                MessageBox.Show("Failed to host new game session!");
        }

        private void playRemoteButton_Click(object sender, EventArgs e)
        {
            if (avaliableSessionsView.SelectedItems.Count == 0)
                return;

            var selectedItem = avaliableSessionsView.SelectedItems[0];
            var guid = Guid.Parse(selectedItem.Text);


            if (_wcf.JoinGame(guid))
            {
                var gamewindow = new MainWindow(nickTextBox.Text, guid, GameTypeEnum.Remote_client, _wcf);
                gamewindow.Show();

                this.Hide();
            }
            else
                Console.WriteLine("Unable to join game!");
        }
    }
}
