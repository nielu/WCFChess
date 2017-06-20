using System;
using System.ServiceModel;
using WCFChess.WCFChessServiceReference;
using WCFChessService;
namespace WCFChess
{
    public class WCFCommunicationWrapper : IChessServiceCallback, WCFChessServiceLibCallback
    {
        private WCFChessServiceLibClient _client;
        private bool _update = false;
        private GameBoard _board;

        public event EventHandler BoardUpdate;



        private void log(string message, params object[] args)
        {
            this.log(string.Format(message, args));
        }

        private void log(string message)
        {
            MainWindow.LogMessageStatic(message);
        }
        
        public string Username
        {
            get;
            private set;
        }

        public Guid SessionGUID
        {
            get;
            private set;
        }

        public bool Connected
        {
            get;
            private set;
        }

        public bool ActionRequired
        {
            get
            {
                if (_update)
                {
                    _update = false;
                    return true; //do this only once each update
                }
                return false;
            }
            set
            {
                _update = value;
            }
        }

        public byte[] RecentGameData;

        public PlayerEnum CurrentPlayer
        {
            get
            {
                if (Connected)
                    return _client.GetCurrentPlayer(SessionGUID);
                return PlayerEnum.Empty;
            }
        }

        public WCFCommunicationWrapper(string userName)
        {
            _client = new WCFChessServiceLibClient(new InstanceContext(this), "WSDualHttpBinding_WCFChessServiceLib");
            Connected = false;
            Username = userName;
            SessionGUID = Guid.Empty;
        }

        public WCFCommunicationWrapper(string userName, Guid sessionID)
        {
            _client = new WCFChessServiceLibClient(new InstanceContext(this), "WSDualHttpBinding_WCFChessServiceLib");
            Connected = false;
            Username = userName;
            SessionGUID = sessionID;
        }

        public void SetupSession(GameBoard board)
        {
            log("_board init");
            _board = board;
            log(_board.ToString());
        }

        public void Connect()
        {
            if (Connected == true) return;
            _client.Open();
            Connected = true;
        }

        public void GameTurn(byte[] GameData)
        {
            _client.PerformGameTurn(GameData, SessionGUID);
        }

        public void NotifyUserJoinedGame(string userName, Guid sessionID)
        {
            if (userName == Username) return;
            log("User joined game!");
            //ActionRequired = true;
            return;
        }

        public void NotifyUserLeftGame(Guid sessionID)
        {
            //ActionRequired = true;
            log("User left game!");
            return;
        }

        public void NotifyUserOfGameTurn(byte[] GameData, Guid sessionID)
        {
            ActionRequired = true;
            log("your turn!");
            RecentGameData = GameData;
            GameController.GameBoardStatic.Board = GameBoard.Deserialize(GameData);
           _board.Board = GameBoard.Deserialize(GameData);

            BoardUpdate?.Invoke(this, null);
            return;
        }

        public GameSession[] GetAvailableSession()
        {
            return _client.GetGameSessions();
        }

        public bool HostGame()
        {
            var retVal = _client.CreateGameSession(Username);
            if (retVal == Guid.Empty)
                return false;
            SessionGUID = retVal;
            return true;
        }

        public bool JoinGame()
        {
            return _client.JoinGame(Username, SessionGUID);
        }

        public bool JoinGame(Guid sessionID)
        {
            if (_client.JoinGame(Username, sessionID))
            {
                SessionGUID = sessionID;
                return true;
            }
            return false;
        }

        
    }
}
