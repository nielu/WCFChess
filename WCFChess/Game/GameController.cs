using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using WCFChessService;

namespace WCFChess
{
    public class GameController
    {
        //public GameBoard Board
        //{
        //    get
        //    {
        //        return GameBoardStatic;
        //    }
        //}\
        public GameBoard Board;
        public ChessFigure SelectedFigure;
        public PlayerEnum CurrentPlayer;
        public List<Move> PossibleMoves;
        public GameStateEnum GameState;
        public PlayerEnum Player;
        private GameTypeEnum gameType;

        public static GameBoard GameBoardStatic = new GameBoard();

        private WCFCommunicationWrapper _wcf;
        private PlayerEnum Enemy;
        private ChessPanel lastPanel = new ChessPanel();
        public GameController()
        {
            init();
            gameType = GameTypeEnum.Local;
        }

        public GameController(string username, Guid sessionID, GameTypeEnum type, WCFCommunicationWrapper wcf)
        {
            init();
            _wcf = wcf;
            gameType = type;

            log("Initalizing connection to WCF service");
            _wcf.Connect();

            CurrentPlayer = _wcf.CurrentPlayer;
            if (type == GameTypeEnum.Remote_client)
                Player = PlayerEnum.Player_B;
            else
                Player = PlayerEnum.Player_A;
            Enemy = (Player == PlayerEnum.Player_A) ? PlayerEnum.Player_B : PlayerEnum.Player_A;

            log("You are playing {0} vs {1}", Player, Enemy);
        }

        private void init()
        {
            GameState = GameStateEnum.Stopped;
            Board = new GameBoard();
            PossibleMoves = new List<Move>();
            CurrentPlayer = PlayerEnum.Empty;
        }

        private void log(string msg, params object[] args)
        {
            MainWindow.LogMessageStatic(msg, args);
        }

        public ChessFigure GetFigure(int x, int y)
        {
            return Board.GetField(x, y);
        }

        public bool InRange(ChessFigure f1, ChessFigure f2)
        {
            var p2 = f2.Position;
            var moves = f1.GetMoves(ref Board);
            return moves.Count(m => m.Destination == p2) > 0;
        }

        public void GameTick(ChessPanel selectedPanel)
        {
            var figure =  selectedPanel.Figure;
            CurrentPlayer = _wcf.CurrentPlayer;
            if (gameType != GameTypeEnum.Local && CurrentPlayer == Enemy) return;
            switch (GameState)
            {
                case GameStateEnum.Stopped:
                    return;
                case GameStateEnum.Player_SelectFigure:

                    if (figure.Owner != CurrentPlayer || figure.FigureType == FigureTypeEnum.Empty)
                        return;
                    SelectedFigure = figure;
                    PossibleMoves = figure.GetMoves(ref Board);
                    GameState = GameStateEnum.Player_SelectMove;
                    log("Selected {0}", figure);
                    break;
                case GameStateEnum.Player_SelectMove:
                    var selectedMove = PossibleMoves.FirstOrDefault(m => m.Destination == figure.Position);
                    if (figure.Owner == CurrentPlayer)
                    {
                        log("Changing selection");
                        GameState = GameStateEnum.Player_SelectFigure;
                        GameTick(selectedPanel);
                        return;
                    }
                    if (selectedMove == null) return;
                    if (InRange(SelectedFigure, figure) == false) return;
                    selectedMove.Execute(ref Board);
                    PossibleMoves.Clear();
                    GameState = GameStateEnum.Player_SelectFigure;
                    NextPlayer();
                    break;
                case GameStateEnum.Player_Final:
                    break;

            }

            lastPanel = selectedPanel;
        }

        public void NextPlayer()
        {
            if (gameType == GameTypeEnum.Local)
            {
                CurrentPlayer = (CurrentPlayer == PlayerEnum.Player_A) ? PlayerEnum.Player_B : PlayerEnum.Player_A;
                log("Next player");
            }
            else
            { 
                log("Sending update to other player");
                _wcf.GameTurn(Board.Serialize());
                CurrentPlayer = _wcf.CurrentPlayer;
            }
        }

        public void StartGame()
        {
            GameBoardStatic.InitBoard();
            if (gameType != GameTypeEnum.Local)
            {
                _wcf.SetupSession(Board);
                var t = new Thread(new ThreadStart(turnThread));
                //t.Start();
            }
            else
                CurrentPlayer = PlayerEnum.Player_A;
            GameState = GameStateEnum.Player_SelectFigure;
            Board.InitBoard();
        }

        private void turnThread()
        {
            
            while (true)
            {
                if (_wcf.Connected)
                {

                    if (_wcf.ActionRequired)
                    {
                        log("Update!");
                        this.CurrentPlayer = _wcf.CurrentPlayer;
                    }
                    //else
                        //log("notihing to do");
                    
                }
                Thread.Sleep(50);
            }
        }
    }
}
