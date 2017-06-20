using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.Serialization;
using WCFChessService;

namespace WCFChess
{
    public class ChessPanel : Panel
    {
        public ChessFigure Figure;
        public int PosX;
        public int PosY;
        public Color OriginalBackground;

        public Label figureNameLabel;
        public Label figureSymbolLabel;

        public ChessPanel()
        {
            figureNameLabel = new Label
            {
                Text = string.Empty,
                Visible = true,
                Location = new Point(0, 65),
            };
            figureSymbolLabel = new Label
            {
                Text = string.Empty,
                Visible = true,
                Location = new Point(8, 10),
                Font = new Font("Arial", 36f),
                AutoSize = true
            };
            this.Controls.Add(figureSymbolLabel);
            this.Controls.Add(figureNameLabel);
        }

        public void UpdatePanel()
        {
            if (Figure != null && Figure.FigureType != FigureTypeEnum.Empty)
            {
                figureNameLabel.Text = Enum.GetName(typeof(FigureTypeEnum), Figure.FigureType);
                figureNameLabel.ForeColor = (Figure.Owner == PlayerEnum.Player_A) ? Color.White : Color.Black;
                figureSymbolLabel.Text = Figure.ToString();
            }
            else
            {
                figureNameLabel.Text = string.Empty;
                figureSymbolLabel.Text = string.Empty;
            }
        }

        private void log(string msg, params object[] args)
        {
            MainWindow.LogMessageStatic(msg, args);
        }

    }



    [DataContract]
    public class Move
    {
        [DataMember]
        public Position Origin;
        [DataMember]
        public Position Destination;

        public void Execute(ref GameBoard b)
        {
            var temp = b.Board[Origin.X, Origin.Y];
            temp.Position = Destination;
            b.Board[Origin.X, Origin.Y] = new ChessFigure(Origin.X, Origin.Y, PlayerEnum.Empty);
            b.Board[Destination.X, Destination.Y] = temp;

            if (this.GetType() == typeof(Attack))
                log("BOOM\nANDHISNAMEISJOHNCEEEENA");
            else
                log("Moved from {0} to {1}", Origin, Destination);
        }

        protected void log(string msg, params object[] args)
        {
            MainWindow.LogMessageStatic(msg, args);
        }


        public virtual Color MoveColor
        {
            get
            {
                return Color.LightGreen;
            }
        }
    }

    public class Attack : Move
    {
        public override Color MoveColor
        {
            get
            {
                return Color.PaleVioletRed;
            }
        }
    }

    public class Position
    {
        public int X;
        public int Y;
        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            var let = "ABCDEFGH";
            return String.Format("{0}{1}", let[Y], X + 1);
        }

        public static bool operator ==(Position p1, Position p2)
        {
            return p1.X == p2.X && p1.Y == p2.Y;
        }

        public static bool operator !=(Position p1, Position p2)
        {
            return p1.X != p2.X || p1.Y != p2.Y;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
