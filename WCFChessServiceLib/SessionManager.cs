using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WCFChessService
{
    /// <summary>
    /// It's ugly, it's singleton...
    /// but it works
    /// </summary>
    public class SessionManager
    {
        public static SessionManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_locker) //double check for null to avoid expenisve lock operation as often as possible
                    {
                        if (_instance == null)
                            _instance = new SessionManager();
                    }
                }
                return _instance;
            }
        }

        private static SessionManager _instance;
        private static object _locker = new object();

        public Dictionary<Guid, GameSession> Games;

        private SessionManager()
        {
            Games = new Dictionary<Guid, GameSession>();
        }
    }

    [DataContract]
    public class GameSession
    {
        public GameSession(string PlayerAName)
        {
            SessionID = Guid.NewGuid();
            PlayerA = PlayerAName;
            PlayerB = string.Empty;
            CurrentPlayer = PlayerEnum.Empty;
            Console.WriteLine("New game session {0} with player {1}", SessionID.ToString(), PlayerAName);
        }
        [DataMember]
        public Guid SessionID;

        [DataMember]
        public string PlayerA, PlayerB;

        public IChessServiceCallback PlayerACallback;
        public IChessServiceCallback PlayerBCallback;

        public PlayerEnum CurrentPlayer;

        

    }
}
