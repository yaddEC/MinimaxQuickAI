using System;
using System.Collections.Generic;


namespace TicTacToe
{

	public struct Move
	{
		public int Line;
		public int Column;
	}

    public enum Settings
    {
        Random = 0,
        MiniMax = 1,
        NegaMax = 2,
        AlphaBeta = 3,
    }

    public enum Player
    {
        Cross = 1,
        Circle = 2
    }

    public class GameMgr
    {
        bool isGameOver = false;
        Settings settings ;
        public bool IsGameOver { get { return isGameOver; } }
        Board mainBoard = new Board();

        public GameMgr()
        {
            InitSetting();
            mainBoard.Init();
            mainBoard.CurrentPlayer = Player.Cross;
        }

        bool IsPlayerTurn()
        {
            return mainBoard.CurrentPlayer == Player.Cross;
        }

        private int GetPlayerInput(bool isColumn)
        {
            Console.Write("\n{0} turn : enter {1} number\n", IsPlayerTurn() ? "Player" : "Computer", isColumn ? "column" : "line");
            ConsoleKeyInfo inputKey;
            int resNum = -1;
            while (resNum < 0 || resNum > 2)
            {
                inputKey = Console.ReadKey();
                int inputNum = -1;
                if (int.TryParse(inputKey.KeyChar.ToString(), out inputNum))
                    resNum = inputNum;
            }
            
            return resNum;
        }

        public void InitSetting()
        {

            ConsoleKeyInfo inputKey;
            int settingNum = -1;
            int inputNum = -1;
           

            while (settingNum < 0 || settingNum > 3)
            {
                Console.Clear();
                Console.Write("\nSet AI Algorithm :\n 0 = Random \n 1 = MiniMax\n 2 = NegaMax \n 3 = Alpha-Beta \n ");
                inputKey = Console.ReadKey();
                if (int.TryParse(inputKey.KeyChar.ToString(), out inputNum))
                    settingNum = inputNum;
            }

            switch (settingNum)
            {
                case 0: settings = Settings.Random; break;

                case 1: settings = Settings.MiniMax; break;

                case 2: settings = Settings.NegaMax; break;

                case 3: settings = Settings.AlphaBeta; break;
            }
            

        }



        public bool Update()
        {
            mainBoard.Draw();

            Move crtMove = new Move();
            if (IsPlayerTurn())
            {
                Console.Write("\nAI function called {0} time(s)\n\n\n", AI.recursivityCount);
                AI.recursivityCount = 0;
                crtMove.Column = GetPlayerInput(true);
                crtMove.Line = GetPlayerInput(false);
                if (mainBoard.BoardSquares[crtMove.Line, crtMove.Column] == 0)
                {
                    mainBoard.MakeMove(crtMove);
                }
            }
            else
            {
                ComputeAIMove();
            }

            if (mainBoard.IsGameOver())
            {
                mainBoard.Draw();
                Console.Write("game over - ");
                int result = mainBoard.Evaluate(Player.Cross);
                if (result == 100)
                    Console.Write("you win\n");
                else if (result == -100)
                    Console.Write("you lose\n");
                else
                    Console.Write("it's a draw!\n");

                Console.ReadKey();

                return false;
            }
            return true;
        }
        
        
        void ComputeAIMove()
        {
            switch (settings) 
            {
                case Settings.Random:    AI.Random(mainBoard);  break;
                case Settings.MiniMax:   AI.MiniMax(mainBoard); break;
                case Settings.NegaMax:   AI.NegaMax(mainBoard); break;
                case Settings.AlphaBeta: AI.AlphaBeta(mainBoard); break;

            }
            mainBoard.MakeMove(AI.bestMove);
            Console.Beep();
        }


    }
}

