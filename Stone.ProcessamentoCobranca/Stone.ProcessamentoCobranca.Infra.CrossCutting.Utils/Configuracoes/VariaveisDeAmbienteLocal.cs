using DotNetEnv;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Stone.ProcessamentoCobranca.Infra.CrossCutting.Utils.Configuracoes
{
    public static class VariaveisDeAmbienteLocal
    {
        public static void CarregarVariaveis()
        {
            const string nomeDoArquivo = ".env";

            ConfigurarVariaveisDeAmbienteStone(nomeDoArquivo);
            var diretorio = AppDomain.CurrentDomain.BaseDirectory;
            var arquivo = Path.Combine(diretorio, nomeDoArquivo);
            if (File.Exists(arquivo))
            {
                Env.Load(arquivo);
            }
        }

        private static void ConfigurarVariaveisDeAmbienteStone(in string nomeDoArquivo)
        {
            var caminho = ObterDiretorio(nomeDoArquivo)?.FullName ?? string.Empty;
            var arquivo = Path.Combine(caminho, nomeDoArquivo);

            var diretorioDestino = AppDomain.CurrentDomain.BaseDirectory;
            var diretorioArquivo = Path.Combine(diretorioDestino, nomeDoArquivo);

            if (!File.Exists(arquivo))
                return;

            var streamReader = File.OpenText(arquivo);
            string arquivoEmTexto = streamReader.ReadToEnd();
            streamReader.Close();


            var infoArquivo = new FileInfo(diretorioArquivo);
            var streamWriter = infoArquivo.CreateText();
            streamWriter.Write(arquivoEmTexto);
            streamWriter.Close();
        }

        public static DirectoryInfo ObterDiretorio(in string nomeDoArquivo)
        {
            var diretorio = Directory.GetParent(Directory.GetCurrentDirectory());
            while (diretorio != null && !diretorio.GetFiles(nomeDoArquivo).Any())
            {
                diretorio = diretorio.Parent;
            }

            return diretorio;
        }
    }
}
