/*
 *	Program.cs
 *	Author: Antonio Hofmeister Richter
 *  Description: Programa para o processo seletivo da Klassmatt de um Jogo da Velha.
 *	Date: 18-10-2022
 *	Modified: 19-10-2022
 */


using System;

namespace JogoDaVelha
{
    class Program
    {
        // Lista 2d para armazenar as jogadas realizadas pelos jogadores.
        static List<string> Board = null;
        // Lista 2d para facilitar a verificação de vitória, utilizando a regra de magic square
        //jogar 1 e jogador 2 recebem respectivametne +1 e -1 nas posições jogadas
        //para vencer, os jogadores devem ter um total de abs(3), na vertical, horizontal ou diagonal.
        static List<int> PointBoard = null;
        // Boolean para controlar se o jogo está ocorrendo.
        static bool playing;
        // Integer par controlar o jogador que esta jogando e o número de jogadas feitas na pertida.
        //player MOD 2 == 0 -> player 1
        //player MOD 2 == 1 -> player 2
        static int player;

        static void Main(String[] args)
        {
            bool loop = true;
            while(loop)
            {
                // Menu no jogo
                Console.Clear();
                Console.WriteLine("//|||||||||||||||\\\\");
                Console.WriteLine("|| Jogo da Velha ||");
                Console.WriteLine("||---------------||");
                Console.WriteLine("|| 1) Regras     ||");
                Console.WriteLine("|| 2) Jogar      ||");
                Console.WriteLine("|| 3) Sair       ||");
                Console.WriteLine("\\\\|||||||||||||||//");
                Console.WriteLine();
                Console.WriteLine("Digite a opção desejada:");
                string option = Console.ReadLine();
                switch (option)
                {
                    // Regras.
                    case "1":
                        Console.WriteLine();
                        Console.WriteLine("No século XIX, no Reino Unido, era comum as senhoras se reunirem para jogar noughts and crosses -");
                        Console.WriteLine("zeros e cruzes, em uma tradução livre - enquanto bordavam e conversavam. Foi assim que o passatempo");
                        Console.WriteLine("virou “jogo das velhas” e depois simplificado para jogo da velha.  Em homenagem a esse jogo que até");
                        Console.WriteLine("hoje nos leva ao se século retrasado, vamos recriá-lo em uma versão moderna e simplificada.");
                        Console.WriteLine();
                        Console.WriteLine("Regras:");
                        Console.WriteLine("- O jogo ocorre em um tabuleiro 3x3");
                        Console.WriteLine("- O jogo será para duas pessoas jogarem, alternadamente");
                        Console.WriteLine("- O jogador 1, sempre será o X e sempre iniciará o jogo");
                        Console.WriteLine("- O jogador 2, sempre será a O e sempre será o segundo a jogar");
                        Console.WriteLine("- O jogo pode ter 3 resultados: vitória do jogador 1, vitória do jogador 2 ou empate");
                        Console.WriteLine(" - Ganha o jogador que primeiro formar uma reta na diagonal, vertical ou horizontal do tabuleiro.");
                        Console.WriteLine();
                        Console.WriteLine("Pressione qualquer tecla para continuar:");
                        Console.ReadKey();
                    break;

                    // Jogo.
                    case "2":
                        playing = true;
                        player = 0;
                        Board = new List<string> (new string[9]);
                        PointBoard = new List<int> (new int[9]);
                        Game();
                    break;

                    // Fechar o jogo.
                    case "3":
                        Console.Write("Saindo");
                        Thread.Sleep(200);
                        Console.Write(".");
                        Thread.Sleep(200);
                        Console.Write(".");
                        Thread.Sleep(200);
                        Console.Write(".");
                        Thread.Sleep(200);
                        loop = false;
                    break;

                    // Controle caso o usuáio digite uma opção inexistente.
                    default:
                        Console.WriteLine("Opção inválida!");
                        Console.WriteLine();
                        Console.WriteLine("Pressione qualquer tecla para continuar:");
                        Console.ReadKey();
                    break;
                }
            }
        }

        // Função principal de controle do jogo.
        private static void Game()
        {
            while(playing)
            {   
                // Print do tabuleiro na tela
                Console.Clear();
                CreateBoard();

                // Teste para ver se houve empate
                if(player >= 9)
                {
                    Console.WriteLine("Empate!");
                    Console.WriteLine("Nenhum jogador venceu.");
                    break;
                }
                Console.WriteLine("Vez do jogador {0} jogar.", player % 2 + 1);
                Console.WriteLine("Digite a posição onde deseja jogar, ela deve estar entre 1 e 9 e não pode estar ocupada:");
                string move = Console.ReadLine();
                // Try catch caso haja probelma no Parse int.
                try
                {
                    // Testa se a jogada é válida
                    if(CheckMove(int.Parse(move) - 1))
                    {
                        // Caso haja uma vitória, playing receberá falso
                        playing = !(CheckDiag() || CheckCol() || CheckRow());
                        player++;
                    }
                    else
                    {
                        Console.WriteLine("{0} é uma jogada inválida!",move);
                        Console.WriteLine("Por favor, digite o número da posição desejada, podendo ser entre 1 e 9, sabendo que ela não pode estar ocupada.");
                        Console.WriteLine("Ex.: 2");
                        Console.WriteLine();
                        Console.WriteLine("Pressione qualquer tecla para continuar:");
                        Console.ReadKey();
                    }
                }
                catch (System.Exception)
                {
                    Console.WriteLine("{0} é uma jogada inválida!",move);
                    Console.WriteLine("Por favor, digite o número da posição desejada, podendo ser entre 1 e 9, sabendo que ela não pode estar ocupada.");
                    Console.WriteLine("Ex.: 2");
                    Console.WriteLine();
                    Console.WriteLine("Pressione qualquer tecla para continuar:");
                    Console.ReadKey();
                }
            }
            Console.WriteLine();
            Console.WriteLine("Pressione qualquer tecla para continuar:");
            Console.ReadKey();
        }

        // Função para testar se a jogada é válida
        private static bool CheckMove(int move)
        {
            // Função que recebe um inteiro move e testa caso ele seja uam jogada válida.
            // Retorna true caso seja válido, false caso contrário.

            // Testa para ver se o move é uma posição existente no tabuleiro e se esta posição está vazia.
            if(move >= 0 && move <= 8 && System.String.IsNullOrEmpty(Board[move]))
            {
                // Caso seja o player 1, será adicionado um X e +1 na posição escolhida.
                if(player % 2 == 0)
                {
                    PointBoard[move] = 1;
                    Board[move] = "X";
                    return true;
                }
                // Caso seja o player 2, será adicionado um O e -1 na posição escolhida.
                else
                {
                    PointBoard[move] = -1;
                    Board[move] = "O";
                    return true;
                }
            }
            return false;
        }
        
         /* Funções para verificar se houve uma vitória*/
        
        // Função que verifica todas as linhas do tabuleiro.
        private static bool CheckRow()
        {
            // Função que soma todos os espaços de cada linha, e caso o valor absoluto da soma seja igual a 3,
            //retorna true, pois houve uma vitória, retorna false caso contrário.
            if(Math.Abs(PointBoard[0] + PointBoard[1] + PointBoard[2]) == 3 
                || Math.Abs(PointBoard[3] + PointBoard[4] + PointBoard[5]) == 3
                || Math.Abs(PointBoard[6] + PointBoard[7] + PointBoard[8]) == 3)
            {
                CreateBoard();
                Console.WriteLine("Jogador {0} ganhou horizontalmente!",player % 2 + 1);
                return true;
            }
            return false;
        }

        // Função que verifica todas as colunas do tabuleiro.
        private static bool CheckCol()
        {
            // Função que soma todos os espaços de cada coluna, e caso o valor absoluto da soma seja igual a 3,
            //retorna true, pois houve uma vitória, retorna false caso contrário.
             if(Math.Abs(PointBoard[0] + PointBoard[3] + PointBoard[6]) == 3 
                || Math.Abs(PointBoard[1] + PointBoard[4] + PointBoard[7]) == 3
                || Math.Abs(PointBoard[2] + PointBoard[5] + PointBoard[8]) == 3)
            {
                CreateBoard();
                Console.WriteLine("Jogador {0} ganhou verticalmente!",player % 2 + 1);
                return true;
            }
            return false;
        }


        // Função que verifica todas as diagonais do tabuleiro.
        private static bool CheckDiag()
        {
            // Função que soma todos os espaços de cada diagonal, e caso o valor absoluto da soma seja igual a 3,
            //retorna true, pois houve uma vitória, retorna false caso contrário.
            if(Math.Abs(PointBoard[0] + PointBoard[4] + PointBoard[8]) == 3 
                || Math.Abs(PointBoard[2] + PointBoard[4] + PointBoard[6]) == 3)
            {
                CreateBoard();
                Console.WriteLine("Jogador {0} ganhou diagonalmente!",player % 2 + 1);
                return true;
            }
            return false;
        }

        // Função para printar na tela o tabuleiro.
        private static void CreateBoard()
        {
            Console.Clear();
            Console.WriteLine("   |   |   ");
            // Caso não haja nada nas posições, printado um espaço no lugar, para que o tabuleiro seja printado corretamente.
            Console.WriteLine(" {0} | {1} | {2} ", System.String.IsNullOrEmpty(Board[0]) ?  " " : Board[0],  
                    System.String.IsNullOrEmpty(Board[1]) ?  " " : Board[1],  System.String.IsNullOrEmpty(Board[2]) ?  " " : Board[2]);
            Console.WriteLine("___|___|____");
            Console.WriteLine("   |   |   ");
            Console.WriteLine(" {0} | {1} | {2} ", System.String.IsNullOrEmpty(Board[3]) ?  " " : Board[3],  
                    System.String.IsNullOrEmpty(Board[4]) ?  " " : Board[4],  System.String.IsNullOrEmpty(Board[5]) ?  " " : Board[5]);
            Console.WriteLine("___|___|____");
            Console.WriteLine("   |   |   ");
            Console.WriteLine(" {0} | {1} | {2} ", System.String.IsNullOrEmpty(Board[6]) ?  " " : Board[6],  
                    System.String.IsNullOrEmpty(Board[7]) ?  " " : Board[7],  System.String.IsNullOrEmpty(Board[8]) ?  " " : Board[8]);
            Console.WriteLine("   |   |   ");
        }
    }
}