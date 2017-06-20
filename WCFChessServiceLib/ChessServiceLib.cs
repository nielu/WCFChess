using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
namespace WCFChessService
{
    public class WCFChessServiceLib : IChessServiceInbound
    {
        private SessionManager _sm;

        public WCFChessServiceLib()
        {
            _sm = SessionManager.Instance;
        }

        public Guid CreateGameSession(string userName)
        {

            var s = new GameSession(userName);
            s.PlayerACallback = OperationContext.Current.GetCallbackChannel<IChessServiceCallback>();
            
            _sm.Games[s.SessionID] = s;
            return s.SessionID;
        }

        public PlayerEnum GetCurrentPlayer(Guid sessionID)
        {
            GameSession s;
            if (_sm.Games.TryGetValue(sessionID, out s))
            {
                if (s.CurrentPlayer == PlayerEnum.Empty)
                {
                    var r = new Random();
                    if (r.Next(100) <= 50)
                        s.CurrentPlayer = PlayerEnum.Player_B;
                    else
                        s.CurrentPlayer = PlayerEnum.Player_A;
                    Console.WriteLine("{0} starts {1} session", s.CurrentPlayer, sessionID);
                }
                return s.CurrentPlayer;
            }
            return PlayerEnum.Empty;
        }

        public List<GameSession> GetGameSessions()
        {
            Console.WriteLine("Returned available sessions");
            var s1 = _sm.Games.Select(kv => kv.Value);
            var s2 = s1.Where(s => s.PlayerB == string.Empty);
            return _sm.Games.Select(kv => kv.Value).Where(s => s.PlayerB == string.Empty).ToList();
        }

        public bool JoinGame(string userName, Guid sessionID)
        {
            GameSession s;
            if (_sm.Games.TryGetValue(sessionID, out s) == false || s.PlayerB != string.Empty)
            {
                Console.WriteLine("{0} tried to enter session witch doesnt exist or is full! - {1}", userName, sessionID.ToString());
                return false;
            }
            if (s.PlayerA == userName) // user tries to enter his own game, that's ok with us
            {
                Console.WriteLine("User {0} wanted to enter his own session {1}", userName, sessionID);
                return true;
            }
            s.PlayerB = userName;
            s.PlayerBCallback = OperationContext.Current.GetCallbackChannel<IChessServiceCallback>();

            s.PlayerACallback.NotifyUserJoinedGame(userName, sessionID);

            Console.WriteLine("{0} entered {1} session", userName, sessionID.ToString());
            return true;
        }

        public void LeaveGame(Guid sessionID)
        {
            GameSession s;

            if (_sm.Games.TryGetValue(sessionID, out s))
            {
                Console.WriteLine("{0} is destroyed");
                s.PlayerACallback.NotifyUserLeftGame(sessionID);
                if (s.PlayerBCallback != null) s.PlayerBCallback.NotifyUserLeftGame(sessionID);
                _sm.Games.Remove(sessionID);
            }
            else
                Console.WriteLine("There is no session to destroy! {0}", sessionID.ToString());
        }

        public void PerformGameTurn(byte[] GameData, Guid sessionID)
        {
            GameSession s;
            if (_sm.Games.TryGetValue(sessionID, out s) && s.PlayerBCallback != null)
            {
                Console.WriteLine("Move in {0}", sessionID.ToString());
                if (s.CurrentPlayer == PlayerEnum.Player_A)
                    s.PlayerBCallback.NotifyUserOfGameTurn(GameData, sessionID);
                else
                    s.PlayerACallback.NotifyUserOfGameTurn(GameData, sessionID);
                s.CurrentPlayer = (s.CurrentPlayer == PlayerEnum.Player_A) ? PlayerEnum.Player_B : PlayerEnum.Player_A;
            }
            else
                Console.WriteLine("Turn performed on non existing/empty session {0}", sessionID.ToString());
        }
    }
    public enum GameStateEnum
    {
        Stopped,
        Player_SelectFigure,
        Player_SelectMove,
        Player_Final,
        Final
    }

    public enum GameTypeEnum
    {
        Local,
        Remote_client,
        Remote_host
    }

    public enum PlayerEnum
    {
        Player_A,
        Player_B,
        Empty
    }
}
