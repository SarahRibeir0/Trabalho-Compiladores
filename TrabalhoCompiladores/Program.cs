// TrabalhoDeCompiladores: Este arquivo contém a função 'main'. A execução do programa começa e termina aqui.

using System;
using System.IO;

namespace TrabalhoDeCompiladores
{
    class Program
    {
        static void Main(string[] args)
        {
            int option = -1;
            bool isSymbolTableDisplayed = false;
            string fileName = "";
            PreCompiler preCompiler = new PreCompiler();
            Lexer scanner = new Lexer();
            Lexer lexer = new Lexer();
            lexer.CoutSymbolTable();

            while (fileName == "")
            {
                Console.WriteLine("******** TRABALHO DE COMPILADORES  ********");
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine("|  Informe o nome ou o caminho do arquivo:");
                Console.Write("|  ");
                fileName = Console.ReadLine();
                Console.WriteLine("--------------------------------------------------");
            }

            try
            {
                preCompiler.Scan(fileName);
                Console.WriteLine("|  Pre-compilador realizou o processo com sucesso.");
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine("|  Tokens gerados pelo Lexer:");
                Console.Write("|  ");
                scanner.Scan();
                Console.WriteLine();
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine("|  Lexer realizou o processo com sucesso.");
                Console.WriteLine("--------------------------------------------------");



                while (option != 0)
                {
                    if (!isSymbolTableDisplayed)
                    {
                        Console.WriteLine("|  Deseja exibir os tokens salvos na tabela de símbolos?");
                        Console.WriteLine("|  0- Sair    1- Exibir tokens");
                        Console.Write("|  Escolha: ");
                        option = Convert.ToInt32(Console.ReadLine());

                        if (option == 1)
                        {
                            Console.WriteLine("|  Tokens: ");
                            scanner.CoutSymbolTable();
                            isSymbolTableDisplayed = true;
                        }
                    }
                    else
                    {
                        Console.WriteLine("|  0- Sair");
                        Console.Write("|  Escolha: ");
                        option = Convert.ToInt32(Console.ReadLine());
                    }

                    Console.WriteLine("--------------------------------------------------");
                }
              
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("|  " + e.Message);
            }

            Console.WriteLine("Pressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }
}
