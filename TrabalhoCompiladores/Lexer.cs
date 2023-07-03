namespace TrabalhoDeCompiladores
{
    internal class Lexer
    {

        private Dictionary<string, Token> symbol_table; // Tabela hash de símbolos.

        public Lexer()
        {
            symbol_table = new Dictionary<string, Token>();
        }

        private const string outputFile = "outputFile.txt"; // Variável com o nome fixo do arquivo de saída do pré-compilador e de entrada para o lexer.

        // retorna tokens para o analisador sintático
        public void Scan()
        {
            // Verifica se o arquivo existe.
            if (!File.Exists(outputFile))
            {
                string error = $"Falha na abertura do arquivo {outputFile} para leitura.";
                throw new ArgumentException(error);
            }

            Console.WriteLine();
            Console.Write("| ");

            // Lê o conteúdo do arquivo linha por linha.
            foreach (string line in File.ReadAllLines(outputFile))
            {
                // Remove os espaços em branco no início e no fim da linha.
                string trimmedLine = line.Trim();

                // Verifica se a linha está vazia ou é um comentário.
                if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith("//"))
                {
                    // Ignora a linha vazia ou de comentário.
                    continue;
                }

                // Divide a linha em palavras separadas por espaços em branco.
                string[] words = trimmedLine.Split(' ');

                // Loop através das palavras da linha.
                foreach (string word in words)
                {
                    // Remove os espaços em branco no início e no fim da palavra.
                    string trimmedWord = word.Trim();

                    // Verifica se a palavra é um número.
                    if (double.TryParse(trimmedWord, out double number))
                    {
                        Console.Write($"<NUMBER, {number}> ");
                    }
                    // Verifica se a palavra é uma palavra reservada ou um identificador.
                    else if (IsReservedWord(trimmedWord))
                    {
                        Console.Write($"<{trimmedWord.ToUpper()}, {trimmedWord}> ");
                    }
                    // Verifica se a palavra é um literal entre aspas duplas.
                    else if (trimmedWord.StartsWith("\"") && trimmedWord.EndsWith("\""))
                    {
                        Console.Write($"<LITERAL, {trimmedWord}> ");
                    }
                    // Trata os demais tokens (símbolos).
                    else
                    {
                        foreach (char symbol in trimmedWord)
                        {
                            Console.Write($"<{symbol}, > ");
                        }
                    }

                    // Adiciona o token à tabela de símbolos
                    Token token = new Token { type = GetTokenType(trimmedWord), value = trimmedWord };
                    AddTokenToSymbolTable(token);
                }
            }

        }

        // Obtém o tipo do token com base nas regras do lexer.
        private string GetTokenType(string word)
        {
            if (double.TryParse(word, out _))
            {
                return "NUMBER";
            }
            else if (IsReservedWord(word))
            {
                return word.ToUpper();
            }
            else if (word.StartsWith("\"") && word.EndsWith("\""))
            {
                return "LITERAL";
            }
            else
            {
                return word;
            }
        }


        // Adiciona o token à tabela de símbolos.
        private void AddTokenToSymbolTable(Token token)
        {
            // Verifica se o token já existe na tabela de símbolos antes de adicioná-lo.
            if (!symbol_table.ContainsKey(token.type))
            {
                symbol_table[token.type] = token;
            }
        }
        // Verifica se a palavra é uma palavra reservada.
        private bool IsReservedWord(string word)
        {
            string[] reservedWords = { "break", "char", "const", "continue", "default", "do", "double",
                               "else", "float", "for", "if", "int", "long", "return", "void", "while" };

            return reservedWords.Contains(word.ToLower());
        }


        internal void CoutSymbolTable()
        {
            if (symbol_table.Count == 0)
            {
                Console.WriteLine("A tabela de símbolos está vazia.");
            }
            else
            {
                Console.WriteLine("Tokens salvos na tabela de símbolos:");
                foreach (var entry in symbol_table)
                {
                    Console.WriteLine($"|    <{entry.Value.type}, {entry.Value.value}>");
                }
            }
        }
    }

    // Estrutura do token, ele terá um tipo e um valor associado.
    public class Token
    {
        public string? type { get; set; }
        public string? value { get; set; }
    }
}