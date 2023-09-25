using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    internal class AI
    {
        public const int DEPTH = 8;
        public static Move bestMove;
        public static int recursivityCount = 0;

        public static void Random(Board board)
        {
            recursivityCount++;
            Random rand = new Random();
            List<Move> moves = board.GetAvailableMoves();
            Move randMove = moves[rand.Next(moves.Count - 1)];
            bestMove = randMove;
        }

        public static int MiniMax(Board board,bool isMaximising = true, int depth = DEPTH)
        {
            recursivityCount++;
            if (board.IsGameOver() || depth == 0)
                return board.Evaluate(Player.Circle);

            Board prediction;
            Move localBestMove = new Move();
            int bestVal;
            if (isMaximising)
                bestVal = int.MinValue;
            else
                bestVal = int.MaxValue;

            List<Move> moves = board.GetAvailableMoves();

            foreach (var move in moves)
            {
                int oldval = bestVal;

                prediction = board.Clone();

                prediction.MakeMove(move);

                if (isMaximising)
                    bestVal = Math.Max(bestVal, MiniMax(prediction,false, depth - 1));
                else
                    bestVal = Math.Min(bestVal, MiniMax(prediction,true, depth - 1));

                if(oldval!=bestVal)
                    localBestMove = move;
            }
            bestMove = localBestMove;
            return bestVal;
        }

        public static int NegaMax(Board board, int depth = DEPTH)
        {
            recursivityCount++;
            if (board.IsGameOver() || depth == 0)
                return board.Evaluate();

            int bestValue = int.MinValue;
            Move localBestMove = new Move();

            List<Move> moves = board.GetAvailableMoves();

            foreach (var move in moves)
            {
                Board prediction = board.Clone();
                prediction.MakeMove(move);

                int value = -NegaMax(prediction, depth - 1);

                if (value > bestValue)
                {
                    bestValue = value;
                    localBestMove = move;
                }
            }

            bestMove = localBestMove;
            return bestValue;
        }

        public static int AlphaBeta(Board board, int depth = DEPTH, int alpha = int.MinValue, int beta = int.MaxValue, bool isMaximizingPlayer = true)
        {
            recursivityCount++;
            if (board.IsGameOver() || depth == 0)
                return board.Evaluate(Player.Circle);

            int bestValue;
            if (isMaximizingPlayer)
                bestValue = int.MinValue;
            else
                bestValue = int.MaxValue;

            Move localBestMove = new Move();
            List<Move> moves = board.GetAvailableMoves();

            foreach (var move in moves)
            {
                Board prediction = board.Clone();
                prediction.MakeMove(move);

                int value = AlphaBeta(prediction, depth - 1, alpha, beta, !isMaximizingPlayer);

                if (isMaximizingPlayer)
                {
                    if (value > bestValue)
                    {
                        bestValue = value;
                        localBestMove = move;
                    }
                    alpha = Math.Max(alpha, value);
                }
                else
                {
                    if (value < bestValue)
                    {
                        bestValue = value;
                        localBestMove = move;
                    }
                    beta = Math.Min(beta, value);
                }

                if (beta <= alpha)
                    break; 
            }

            bestMove = localBestMove;
            return bestValue;
        }


    }
}
