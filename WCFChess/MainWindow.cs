using System;
using System.Drawing;
using System.Windows.Forms;
using WCFChessService;

namespace WCFChess
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
            LogTextBoxInstance = logTextBox;
            gameType = GameTypeEnum.Local;
            Controller = new GameController();
        }

        public MainWindow(string userName, Guid SessionID, GameTypeEnum type, WCFCommunicationWrapper wcf)
        {
            InitializeComponent();
            LogTextBoxInstance = logTextBox;
            gameType = type;
            wcf.BoardUpdate += Wcf_BoardUpdate;
            Controller = new GameController(userName, SessionID, type, wcf);
            
        }

        private void Wcf_BoardUpdate(object sender, EventArgs e)
        {
            UpdateBoard();
            UpdateUI();
        }

        public void LogMessage(string message)
        {
            var msg = String.Format("{0}: {1}\n", DateTime.Now.ToShortTimeString(), message);
            logTextBox.Text = logTextBox.Text.Insert(0, msg);
        }
        public void LogMessage(string message, params object[] args)
        {
            LogMessage(String.Format(message, args));
        }

        public ChessPanel[,] BoardUI = new ChessPanel[8, 8];
        public GameController Controller;

        private Color backColorLight = Color.LightGray, backColorDark = Color.DarkGray, figureToSelect = Color.LightGreen;
        private GameTypeEnum gameType;

        private GameStateEnum State { get { return Controller.GameState; } }

        delegate void StringArgReturningVoidDelegate(string text);
        public static void LogMessageStatic(string msg)
        {
            if (LogTextBoxInstance.InvokeRequired)
            {
                var d = new StringArgReturningVoidDelegate(LogMessageStatic);
                LogTextBoxInstance.Invoke(d, new object[] { msg });
            }
            else
            {
                var text = String.Format("{0}: {1}\n", DateTime.Now.ToShortTimeString(), msg);
                LogTextBoxInstance.Text = LogTextBoxInstance.Text.Insert(0, text);
            }
        }
        public static void LogMessageStatic(string msg, params object[] args)
        {
            LogMessageStatic(string.Format(msg, args));
        }



        private static RichTextBox LogTextBoxInstance;

        private void Form1_Load(object sender, EventArgs e)
        {
            LogMessage("App started");
            CreateBoard();
            UpdateBoard();
            UpdateUI();
        }

        private void GameState(ChessPanel SelectedPanel)
        {
            Controller.GameTick(SelectedPanel);
            UpdateBoard();
            UpdateUI();
        }

        private void UpdateUI()
        {
            gameStateLabel.Text = Enum.GetName(typeof(GameStateEnum), State);
            currentPlayerLabel.Text = Enum.GetName(typeof(PlayerEnum), Controller.CurrentPlayer);

            if (State == GameStateEnum.Player_SelectFigure)
            {
                foreach (var f in Controller.Board.Board)
                {

                    if (gameType == GameTypeEnum.Local)
                        if (f.Owner == Controller.CurrentPlayer)
                            BoardUI[f.X, f.Y].BackColor = figureToSelect;
                    else
                        if (f.Owner == Controller.CurrentPlayer && f.Owner == Controller.Player)
                            BoardUI[f.X, f.Y].BackColor = figureToSelect;
                }
            }
            else if (State == GameStateEnum.Player_SelectMove)
            {
                foreach (var m in Controller.PossibleMoves)
                    BoardUI[m.Destination.X, m.Destination.Y].BackColor = m.MoveColor;
            }
            if (Controller.SelectedFigure != null)
                BoardUI[Controller.SelectedFigure.X, Controller.SelectedFigure.Y].BackColor = figureToSelect;
        }

        private void UpdateBoard()
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    BoardUI[x, y].BackColor = BoardUI[x, y].OriginalBackground;
                    BoardUI[x, y].Figure = Controller.GetFigure(x, y);
                    BoardUI[x, y].UpdatePanel();
                }
            }
        }

        private void CreateBoard()
        {
            var let = "ABCDEFGH";
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {

                    var panel = new ChessPanel()
                    {
                        Name = String.Format("{0}{1}", let[y], x + 1),
                        BackColor = setBackColor(x, y),
                        OriginalBackground = setBackColor(x, y),
                        Location = new Point(x * 80, y * 80),
                        Size = new Size(80, 80),
                        Visible = true,
                        BorderStyle = BorderStyle.FixedSingle,
                        PosX = x,
                        PosY = y,
                        Figure = Controller.GetFigure(x, y)
                    };
                    panel.MouseClick += Panel_MouseClick;
                    panel.figureSymbolLabel.MouseClick += FigureSymbolLabel_MouseClick;
                    panel.figureNameLabel.MouseClick += FigureSymbolLabel_MouseClick;
                    BoardUI[x, y] = panel;

                    gamePanel.Controls.Add(panel);
                }
            }
            LogMessage("Created game board UI");
        }

        private void FigureSymbolLabel_MouseClick(object sender, MouseEventArgs e)
        {
            var p = (Label)sender;
            Panel_MouseClick(p.Parent, e);
        }

        private void Panel_MouseClick(object sender, MouseEventArgs e)
        {

            var p = (ChessPanel)sender;
            if (e.Button == MouseButtons.Right)
            {
                LogMessage("X:{0}, Y{1} - {2}", p.PosX, p.PosY, p.Figure);
            }
            else
            {
                GameState(p);
            }
        }

        private void startGameButtonClick(object sender, EventArgs e)
        {

            startGameButton.Enabled = false;
            switch (gameType)
            {
                case GameTypeEnum.Local:
                    LogMessage("Started local game!");
                    break;
                case GameTypeEnum.Remote_client:
                case GameTypeEnum.Remote_host:
                    LogMessage("Started remote game!");
                    break;
            }
            Controller.StartGame();
            UpdateBoard();
            UpdateUI();
        }

        private Color setBackColor(int x, int y)
        {
            return ((x + y) % 2 == 0) ? backColorLight : backColorDark;
        }

    }

}
