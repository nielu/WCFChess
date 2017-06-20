using System;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using WCFChessService;

namespace WCFChess
{
    [DataContract]
    [Serializable]
    public class GameBoard
    {
        [DataMember]
        public ChessFigure[,] Board;

        public ChessFigure GetField(int x, int y)
        {
            if (x < 0 || x >= Board.GetLength(0)) return null;
            if (y < 0 || y >= Board.GetLength(1)) return null;
            return Board[x, y];
        }

        public ChessFigure GetField(Point p)
        {
            return GetField(p.X, p.Y);
        }

        public GameBoard()
        {
            InitBoard();
        }

        public void InitBoard()
        {
            Board = new ChessFigure[8, 8];
            for (int i = 0; i < 8; i++)
            {
                Board[i, 1] = new Pawn(i, 1, PlayerEnum.Player_B);
                Board[i, 6] = new Pawn(i, 6, PlayerEnum.Player_A);
            }
            Board[0, 0] = new Rook(0, 0, PlayerEnum.Player_B);
            Board[1, 0] = new Knight(1, 0, PlayerEnum.Player_B);
            Board[2, 0] = new Bishop(2, 0, PlayerEnum.Player_B);
            Board[3, 0] = new Queen(3, 0, PlayerEnum.Player_B);
            Board[4, 0] = new King(4, 0, PlayerEnum.Player_B);
            Board[5, 0] = new Bishop(5, 0, PlayerEnum.Player_B);
            Board[6, 0] = new Knight(6, 0, PlayerEnum.Player_B);
            Board[7, 0] = new Rook(7, 0, PlayerEnum.Player_B);
            Board[0, 7] = new Rook(0, 7, PlayerEnum.Player_A);
            Board[1, 7] = new Knight(1, 7, PlayerEnum.Player_A);
            Board[2, 7] = new Bishop(2, 7, PlayerEnum.Player_A);
            Board[3, 7] = new King(3, 7, PlayerEnum.Player_A);
            Board[4, 7] = new Queen(4, 7, PlayerEnum.Player_A);
            Board[5, 7] = new Bishop(5, 7, PlayerEnum.Player_A);
            Board[6, 7] = new Knight(6, 7, PlayerEnum.Player_A);
            Board[7, 7] = new Rook(7, 7, PlayerEnum.Player_A);

            for (int x = 0; x < 8; x++)
                for (int y = 0; y < 8; y++)
                    if (Board[x, y] == null) Board[x, y] = new ChessFigure(x, y, PlayerEnum.Empty);
        }

        public byte[] Serialize()
        {
            var bf = new BinaryFormatter();
            var ms = new MemoryStream();

            bf.Serialize(ms, Board);

            return ms.ToArray();
        }

        public static ChessFigure[,] Deserialize(byte[] board)
        {
            ChessFigure[,] b;
            using (var ms = new MemoryStream(board))
            {
                var f = new BinaryFormatter();
                b = (ChessFigure[,])f.Deserialize(ms);
            }
            return b;
        }
    }
}
