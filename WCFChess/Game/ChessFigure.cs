using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using WCFChessService;

namespace WCFChess
{
    [DataContract]
    [Serializable]
    public class ChessFigure
    {
        [DataMember]
        public int X, Y;
        [DataMember]
        public PlayerEnum Owner;
        [DataMember]
        public FigureTypeEnum FigureType;
        public ChessFigure(int x, int y, PlayerEnum owner)
        {
            X = x;
            Y = y;
            Owner = owner;
            FigureType = FigureTypeEnum.Empty;
        }

        public virtual List<Move> GetMoves(ref GameBoard b)
        {
            return new List<Move>();
        }

        public Position Position
        {
            get
            {
                return new Position(X, Y);
            }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }
        protected void log(string msg, params object[] args)
        {
            MainWindow.LogMessageStatic(msg, args);
        }

        public override string ToString()
        {
            return string.Empty;
        }

        public static ChessFigure Empty()
        {
            return new ChessFigure(0, 0, PlayerEnum.Empty);
        }
    }
    [Serializable]
    public class Pawn : ChessFigure
    {
        public Pawn(int x, int y, PlayerEnum owner) : base(x, y, owner)
        {
            FigureType = FigureTypeEnum.Pawn;
        }

        public override List<Move> GetMoves(ref GameBoard b)
        {
            var retVal = new List<Move>();
            var dir = (Owner == PlayerEnum.Player_A) ? 1 : -1;
            var uL = b.GetField(X - 1, Y - 1 * dir);
            var uR = b.GetField(X + 1, Y - 1 * dir);
            var u = b.GetField(X, Y - 1 * dir);
            if (u != null && u.FigureType == FigureTypeEnum.Empty)
            {
                retVal.Add(new Move
                {
                    Origin = Position,
                    Destination = new Position(X, Y - 1 * dir)

                });
                if ((Y == 6 || Y == 1) && b.GetField(X, Y - 2 * dir) != null && b.GetField(X, Y - 2 * dir).FigureType == FigureTypeEnum.Empty)
                {
                    retVal.Add(new Move
                    {
                        Origin = Position,
                        Destination = new Position(X, Y - 2 * dir)
                    });
                }
            }
            if (uL != null && uL.Owner != Owner && uL.FigureType != FigureTypeEnum.Empty)
            {
                retVal.Add(new Attack
                {
                    Origin = Position,
                    Destination = new Position(X - 1, Y - 1 * dir)
                });
            }
            if (uR != null && uR.Owner != Owner && uR.FigureType != FigureTypeEnum.Empty)
            {
                retVal.Add(new Attack
                {
                    Origin = Position,
                    Destination = new Position(X + 1, Y - 1 * dir)
                });
            }
            return retVal;
        }

        public override string ToString()
        {
            return (Owner == PlayerEnum.Player_A) ? "♙" : "♟";
        }
    }
    [Serializable]
    public class Rook : ChessFigure
    {
        public Rook(int x, int y, PlayerEnum owner) : base(x, y, owner)
        {
            FigureType = FigureTypeEnum.Rook;
        }

        public override List<Move> GetMoves(ref GameBoard b)
        {
            var retVal = new List<Move>();
            for (int x = -1; x < 2; x++)
            {
                if (x == 0) continue;
                for (int i = 1; i < 8; i++)
                {
                    var p = b.GetField(X + i * x, Y);
                    if (p == null) break;
                    if (p.FigureType == FigureTypeEnum.Empty)
                        retVal.Add(new Move
                        {
                            Origin = Position,
                            Destination = new Position(X + i * x, Y)
                        });
                    else if (p.Owner != Owner)
                    {
                        retVal.Add(new Attack
                        {
                            Origin = Position,
                            Destination = new Position(X + i * x, Y)
                        });
                        break;
                    }
                    else
                        break;
                }
            }
            for (int yMul = -1; yMul < 2; yMul++)
            {
                if (yMul == 0) continue;
                for (int i = 1; i < 8; i++)
                {
                    var p = b.GetField(X, Y + i * yMul);
                    if (p == null) break;
                    if (p.FigureType == FigureTypeEnum.Empty)
                        retVal.Add(new Move
                        {
                            Origin = Position,
                            Destination = new Position(X, Y + i * yMul)
                        });
                    else if (p.Owner != Owner)
                    {
                        retVal.Add(new Attack
                        {
                            Origin = Position,
                            Destination = new Position(X, Y + i * yMul)
                        });
                        break;
                    }

                    else
                        break;
                }
            }
            return retVal;
        }

        public override string ToString()
        {
            return (Owner == PlayerEnum.Player_A) ? "♖" : "♜";
        }
    }
    [Serializable]
    public class Knight : ChessFigure
    {
        public Knight(int x, int y, PlayerEnum owner) : base(x, y, owner)
        {
            FigureType = FigureTypeEnum.Knight;
        }

        public override List<Move> GetMoves(ref GameBoard b)
        {
            var possibleMoves = new List<ChessFigure>
            {
                b.GetField(X + 2, Y - 1),
                b.GetField(X + 2, Y + 1),
                b.GetField(X + 1, Y + 2),
                b.GetField(X + 1, Y - 2),
                b.GetField(X - 1, Y + 2),
                b.GetField(X - 1, Y - 2),
                b.GetField(X - 2, Y + 1),
                b.GetField(X - 2, Y - 1)
            };

            var retVal = new List<Move>();

            foreach (var m in possibleMoves)
            {
                if (m == null) continue;
                if (m.FigureType == FigureTypeEnum.Empty)
                    retVal.Add(new Move
                    {
                        Origin = Position,
                        Destination = new Position(m.X, m.Y)
                    });
                else if (m.Owner != Owner)
                    retVal.Add(new Attack
                    {
                        Origin = Position,
                        Destination = new Position(m.X, m.Y)
                    });
            }
            return retVal;
        }

        public override string ToString()
        {
            return (Owner == PlayerEnum.Player_A) ? "♘" : "♞";
        }
    }
    [Serializable]
    public class Bishop : ChessFigure
    {
        public Bishop(int x, int y, PlayerEnum owner) : base(x, y, owner)
        {
            FigureType = FigureTypeEnum.Bishop;
        }

        public override List<Move> GetMoves(ref GameBoard b)
        {
            var retVal = new List<Move>();
            for (int x = -1; x < 2; x++)
            {
                if (x == 0) continue;
                for (int y = -1; y < 2; y++)
                {
                    if (y == 0) continue;
                    for (int i = 1; i < 8; i++)
                    {
                        var p = b.GetField(X + x * i, Y + y * i);
                        if (p == null)
                            break;
                        if (p.FigureType == FigureTypeEnum.Empty)
                            retVal.Add(new Move
                            {
                                Origin = Position,
                                Destination = new Position(X + x * i, Y + y * i)
                            });
                        else if (p.Owner != Owner)
                        {
                            retVal.Add(new Attack
                            {
                                Origin = Position,
                                Destination = new Position(X + x * i, Y + y * i)
                            });
                            break;
                        }
                        else
                            break;
                    }
                }
            }
            return retVal;
        }

        public override string ToString()
        {
            return (Owner == PlayerEnum.Player_A) ? "♗" : "♝";
        }
    }
    [Serializable]
    public class King : ChessFigure
    {
        public King(int x, int y, PlayerEnum owner) : base(x, y, owner)
        {
            FigureType = FigureTypeEnum.Knight;
        }

        public override List<Move> GetMoves(ref GameBoard b)
        {
            var retVal = new List<Move>();
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if (x == 0 && y == 0) continue;
                    var p = b.GetField(X + x, Y + y);
                    if (p != null)
                    {
                        if (p.FigureType == FigureTypeEnum.Empty)
                            retVal.Add(new Move
                            {
                                Origin = Position,
                                Destination = new Position(X + x, Y + y)
                            });
                        else if (p.Owner != Owner)
                            retVal.Add(new Attack
                            {
                                Origin = Position,
                                Destination = new Position(X + x, Y + y)
                            });
                    }
                }
            }
            return retVal;
        }

        public override string ToString()
        {
            return (Owner == PlayerEnum.Player_A) ? "♔" : "♚";
        }
    }
    [Serializable]
    public class Queen : ChessFigure
    {
        public Queen(int x, int y, PlayerEnum owner) : base(x, y, owner)
        {
            FigureType = FigureTypeEnum.Queen;
        }

        public override List<Move> GetMoves(ref GameBoard b)
        {
            var retVal = new List<Move>();
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    for (int i = 1; i < 8; i++)
                    {
                        var p = b.GetField(X + x * i, Y + y * i);
                        if (p == null)
                            break;
                        if (p.FigureType == FigureTypeEnum.Empty)
                            retVal.Add(new Move
                            {
                                Origin = Position,
                                Destination = new Position(X + x * i, Y + y * i)
                            });
                        else if (p.Owner != Owner)
                        {
                            retVal.Add(new Attack
                            {
                                Origin = Position,
                                Destination = new Position(X + x * i, Y + y * i)
                            });
                            break;
                        }
                        else
                            break;
                    }
                }
            }
            return retVal;
        }

        public override string ToString()
        {
            return (Owner == PlayerEnum.Player_A) ? "♕" : "♛";
        }
    }


    public enum FigureTypeEnum
    {
        Empty,
        Pawn,
        Rook,
        Knight,
        Bishop,
        King,
        Queen
    }

}
