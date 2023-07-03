namespace TrabalhoDeCompiladores
{
    internal class PreCompiler
    {
        private Dictionary<string, string>? define_table;
        private const string outputFile = "outputFile.txt";
        private string? error;
        internal void Scan(string fileName)
        {
            // Remover arquivo de saída, se existir
            if (File.Exists(outputFile))
            {
                File.Delete(outputFile);
            }

            using (StreamReader readFile = new StreamReader(fileName))
            {
                // Verifica se o arquivo foi aberto corretamente.
                if (!readFile.BaseStream.CanRead)
                {
                    string error = "Falha na abertura do arquivo " + fileName + " para leitura.";
                    throw new ArgumentException(error);
                }
                using (StreamWriter writeFile = new StreamWriter(outputFile, true))
                {
                    // Verifica se o arquivo foi aberto ou criado corretamente.
                    if (!writeFile.BaseStream.CanWrite)
                    {
                        string error = "Falha na abertura do arquivo " + outputFile + " para escrita.";
                        throw new ArgumentException(error);
                    }

                    // Obtendo um caractere do arquivo.
                    int letter = readFile.Read();

                    // Variável para controle de qual linha estamos no arquivo.
                    int lineNumber = 1;

                    // Loop até chegar no fim do arquivo.
                    while (letter != -1)
                    {
                        // Tratativa de espaços, tabulações e quebras de linha.
                        if (letter == ' ' || letter == '\t' || letter == '\n')
                        {
                            writeFile.Write((char)letter); // Escrevendo o caractere no arquivo de saída.
                                                           // Se for quebra de linha, incrementamos para saber onde estamos no arquivo.
                            if (letter == '\n')
                            {
                                lineNumber++;
                            }
                        }
                        // Tratativa de comentários.
                        else if (letter == '/')
                        {
                            writeFile.Write((char)letter); // Escrevendo o caractere no arquivo de saída.
                            letter = readFile.Read(); // Pegando o próximo caractere do arquivo.

                            // Se for quebra de linha, incrementamos para saber onde estamos no arquivo.
                            if (letter == '\n')
                            {
                                lineNumber++;
                            }

                            /* Se for '/', significa que é um comentário de uma linha.
                               Lemos até o final da linha, escrevemos no arquivo de saída e incrementamos a quantidade de linhas,
                               se houver quebra de linha.
                             */
                            if (letter == '/')
                            {
                                do
                                {
                                    writeFile.Write((char)letter);
                                    letter = readFile.Read();

                                    if (letter == '\n')
                                    {
                                        lineNumber++;
                                    }

                                } while (letter != '\n' && letter != -1);
                            }
                            // Se for '*', significa que é um comentário de múltiplas linhas.
                            // Lemos até encontrar uma sequência de fechamento '*/', escrevemos no arquivo de saída 
                            // e incrementamos a quantidade de linhas, se houver quebra de linha.
                            else if (letter == '*')
                            {
                                letter = readFile.Read();

                                bool aux1 = false;
                                bool aux2 = false;
                                do
                                {
                                    writeFile.Write((char)letter);

                                    if (letter == '\n')
                                    {
                                        lineNumber++;
                                    }

                                    if (letter == '*')
                                    {
                                        aux1 = true;
                                        letter = readFile.Read();
                                        writeFile.Write((char)letter);

                                        if (letter == '\n')
                                        {
                                            lineNumber++;
                                        }
                                    }
                                    if (aux1)
                                    {
                                        if (letter == '/')
                                        {
                                            aux2 = true;
                                        }
                                        else
                                        {
                                            aux1 = false;
                                        }
                                    }
                                    letter = readFile.Read();

                                    // Chegou ao fim do arquivo e não houve fechamento do comentário?
                                    if ((!aux1 || !aux2) && letter == -1)
                                    {
                                        string error = "Chegou ao fim do arquivo e não houve fechamento do comentário de múltiplas linhas.";
                                        throw new ArgumentException(error);
                                    }
                                } while (!(aux1 && aux2) && letter != -1);
                            }
                            else
                            {
                                // Se não for nenhum dos casos acima, escrevemos o caractere no arquivo de saída.
                                writeFile.Write((char)letter);
                            }
                        }
                        else
                        {
                            // Se não for nenhum dos casos acima, escrevemos o caractere no arquivo de saída.
                            writeFile.Write((char)letter);
                        }

                        // Pegando o próximo caractere do arquivo.
                        letter = readFile.Read();
                    }

                    // Fechando os arquivos.
                    readFile.Close();
                    writeFile.Close();
                }
            }
        }
    }
}