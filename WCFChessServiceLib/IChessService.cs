using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace WCFChessService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract(
        Name = "WCFChessServiceLib",
        Namespace = "WCFChessService",
        SessionMode = SessionMode.Required,
        CallbackContract = typeof(IChessServiceCallback)
        )]
    public interface IChessServiceInbound
    {
        [OperationContract]
        List<GameSession> GetGameSessions();

        [OperationContract]
        Guid CreateGameSession(string userName);

        [OperationContract]
        bool JoinGame(string userName, Guid sessionID);

        [OperationContract]
        void PerformGameTurn(byte[] GameData, Guid sessionID);

        [OperationContract]
        void LeaveGame(Guid sessionID);

        [OperationContract]
        PlayerEnum GetCurrentPlayer(Guid sessionID);
    }
    [ServiceContract(CallbackContract =typeof(IChessServiceCallback))]
    public interface IChessServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void NotifyUserJoinedGame(string userName, Guid sessionID);

        [OperationContract(IsOneWay = true)]
        void NotifyUserLeftGame(Guid sessionID);

        [OperationContract(IsOneWay = true)]
        void NotifyUserOfGameTurn(byte[] GameData, Guid sessionID);
    }
}
