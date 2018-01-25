#pragma warning disable CS0618 // Type or member is obsolete
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.Threading;
namespace games_cli
{
    class Program
    {
        private static List<cJogo> JogosSalvo;
        private static List<string> Categorias;
        private static int MaxItensLista = 20;
        private static string Resp = "";
        private static int Local = 0;
        private static string Logo = @"%3
   __                        
   \ \  ___   __ _  ___  ___ 
    \ \/ _ \ / _` |/ _ \/ __|
 /\_/ / (_) | (_| | (_) \__ \
 \___/ \___/ \__, |\___/|___/
             |___/           
    %2Criado por: %4gusdnide
    %2GitHub : %0https://github.com/gusdnide
    %2ThePirateGames : %0thepiratedownload.com
";
        [STAThread]
        static void Main(string[] args)
        {

            CarregarJson();
            Menu();
        }

        static void Menu()
        {

            while (true)
            {
                Console.Clear();
                Console.Title = $"Jogos API 1.0 {JogosSalvo.Count} jogos no banco!";
                cTools.Escrever(Logo);
                switch (Local)
                {
                    case 0:
                        Principal();
                        break;
                    default:
                        Local = 0;
                        Principal();
                        break;

                }


            }
        }
        static bool SimOuNao(string Texto)
        {
            cTools.Escrever(Texto);
            cTools.Escrever("%2Digite %3sim%2 ou %1nao?");
            string R = Console.ReadLine().ToLower();
            if (R == "sim" || R == "s" || R == "y" || R == "yes")
                return true;
            else
                return false;
        }
        static void Download(cJogo j)
        {
            j.Atualizar();
            frmDownload frm = new frmDownload();
            frm.Jogo = j;
            Thread t = new Thread(() => tDownload(frm));
           // t.ApartmentState = ApartmentState.STA;
            t.Start();
        }
        static void tDownload(frmDownload f)        {

            Application.EnableVisualStyles();
            Application.Run(f); // or whatever
        }
        static void tAbrirForm(frmResultCate frm)
        {
            Thread t = new Thread(() => AbrirForm(frm));
            t.Start();
        }
        static void AbrirForm(frmResultCate frm)
        {
            Application.EnableVisualStyles();
            Application.Run(frm); // or whatever
        }
        static void Principal()
        {
            cTools.Escrever(@"
%3 /atualizar -%4 Atualizar banco de dados.
%3 /pesquisarpag -%4 Pega os %2jogos%4 apartir de uma das paginas.
%3 /pesquisar -%4 Pesquisa algum %2jogo%4 apartir do servidor.
%3 /pesquisarloc -%4 Pesquisa algum %2jogo%4 apartir do arquivo local.
%3 /listacate -%4 Mostra a lista de %2jogos%4 apartir de uma categoria.
%3 /lista -%4 Mostra a lista de %2jogos%4.
%3 /sair -%4 Para sair.
%3 /selecione -%4 Seleciona o jogo para baixar %2jogo%4.");
            Resp = Console.ReadLine().ToLower();
            int iRet = -1;
            cJogo[] reBusca;
            switch (Resp.ToLower())
            {
                case "/atualizar":
                    if (SimOuNao("%2Voce deseja atualizar o banco de dados?"))
                        AtualizarDB();
                    break;
                case "/listacate":

                    cTools.Escrever("%2ID\t\tCategoria");
                    for (int i = 0; i < Categorias.Count; i++)
                        cTools.Escrever($"{i}\t\t{Categorias[i]}");
                    cTools.Escrever("%4Selecione uma das %2categorias%4.");
                    cTools.Escrever("Digite a id:");
                    Resp = Console.ReadLine();
                    if (int.TryParse(Resp, out iRet))
                    {
                        if (iRet >= 0 && iRet < Categorias.Count)
                        {
                            try
                            {
                                reBusca = cJogo.BuscarPorCategoria(JogosSalvo.ToArray(), Categorias[iRet]);
                                cTools.Escrever($"%2{reBusca.Length} %1Jogos %2Jogos encontrados!");
                                if (reBusca == null && reBusca.Length <= 0)
                                    break;
                                else
                                if (reBusca != null && reBusca.Length > 0)
                                {
                                    frmResultCate frm = new frmResultCate();
                                    frm.Lista = reBusca;
                                    tAbrirForm(frm);
                                    if (SimOuNao("%2Deseja abrir baixar algum dos jogos?"))
                                    {
                                        cTools.Escrever("Digite o ID: ");
                                        Resp = Console.ReadLine();
                                        if (int.TryParse(Resp, out iRet))
                                        {
                                            if (iRet > -1 && iRet < reBusca.Length)
                                            {
                                                Download(reBusca[iRet]);
                                            }
                                        }
                                    }
                                }else
                                {
                                    cTools.Escrever("Nao existe jogo com estas categoria.", ConsoleColor.Red);
                                }
                                Console.ReadLine();
                            }
                            catch
                            {
                                cTools.Escrever("Error ", ConsoleColor.Red);
                            }
                        }
                        else
                        {
                            cTools.Escrever("Nao existe esse ID", ConsoleColor.Red);
                        }
                    }
                    break;
                case "/selecione":
                    cTools.Escrever("Digite o ID do jogo: (para saber o ID do jogo digite /lista");
                    Resp = Console.ReadLine();
                    if(int.TryParse(Resp, out iRet))
                    {
                        if(iRet > 0 && iRet <= JogosSalvo.Count)
                        {
                            Download(JogosSalvo[iRet]);
                        }else
                        {
                            cTools.Escrever("ID Invalido!", ConsoleColor.Red);
                        }
                    }
                    break;                   
                case "/pesquisarpag":
                    cTools.Escrever("Digite o numero da pagina:");
                    Resp = Console.ReadLine();
                    if (int.TryParse(Resp, out iRet))
                    {
                        if (iRet > -1 && iRet < 200)
                        {
                            try
                            {
                                reBusca = cJogo.PegarPagina(iRet);
                                if (reBusca != null && reBusca.Length > 0)
                                {
                                    cTools.Escrever($"%2{reBusca.Length} %1Jogos %2Jogos encontrados!");
                                    frmResultCate frm = new frmResultCate();
                                    frm.Lista = reBusca;
                                    tAbrirForm(frm);
                                    if (SimOuNao("%2Deseja salvar estes jogos ao banco?"))
                                    {
                                        JogosSalvo.AddRange(reBusca);
                                        RemoverDuplicados();
                                    }
                                    if (SimOuNao("%2Deseja abrir baixar algum dos jogos?"))
                                    {
                                        cTools.Escrever("Digite o ID: ");
                                        Resp = Console.ReadLine();
                                        if (int.TryParse(Resp, out iRet))
                                        {
                                            if (iRet > -1 && iRet < reBusca.Length)
                                            {
                                                Download(reBusca[iRet]);
                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    cTools.Escrever("Pagina nao existe ou nao tem jogos", ConsoleColor.Red);
                                }
                            }
                            catch
                            {
                                cTools.Escrever("Pagina nao existe ou nao tem jogos", ConsoleColor.Red);
                            }
                        }
                        else
                        {
                            cTools.Escrever("Pagina nao existe", ConsoleColor.Red);
                        }

                    }
                    break;
                case "/lista":
                    int MaxPaginas = JogosSalvo.Count / MaxItensLista;
                    cTools.Escrever($"Digite a pagina Min:0 Max:{MaxPaginas}");
                    Resp = Console.ReadLine();

                    if (int.TryParse(Resp, out iRet) && iRet > -1 && iRet <= MaxPaginas)
                    {
                        cTools.Escrever("%2ID\tNome\t\t\tDescricao");
                        for (int i = iRet * MaxItensLista; i <= (iRet * MaxItensLista) + MaxItensLista; i++)
                        {
                            cJogo j = JogosSalvo[i];
                            cTools.EscreverMod($"{i}%5\t{PegarTexto(j.Nome, 20)}");
                            cTools.EscreverMod($"\t%4{PegarTexto(j.Descricao, 40)}");
                            cTools.Escrever("");
                        }
                        Console.ReadLine();
                    }
                    else
                    {
                        cTools.Escrever("Pagina inexistente!", ConsoleColor.Red);
                    }
                    break;
                case "/pesquisarloc":
                    cTools.Escrever("%4Digite o nome do %2jogo%4.");
                    Resp = Console.ReadLine();
                    reBusca = cJogo.BuscarDB(JogosSalvo.ToArray(), Resp);
                    if(reBusca != null && reBusca.Length >= 0)
                    {
                        frmResultCate frm = new frmResultCate();
                        frm.Lista = reBusca;
                        tAbrirForm(frm);
                        if (SimOuNao("%2Deseja abrir baixar algum dos jogos?"))
                        {
                            cTools.Escrever("Digite o ID: ");
                            Resp = Console.ReadLine();
                            if (int.TryParse(Resp, out iRet))
                            {
                                if (iRet > -1 && iRet < reBusca.Length)
                                {
                                    Download(reBusca[iRet]);
                                }
                            }
                            else
                            {
                                cTools.Escrever("ID Inexistente.", ConsoleColor.Red);
                            }
                        }
                    }
                    break;
                case "/pesquisar":
                    try
                    {
                        cTools.Escrever("Digite o nome do jogo a pesquisar:");
                        Resp = Console.ReadLine().ToLower();
                        reBusca = cJogo.ProcurarJogo(Resp);
                        cTools.Escrever($"%2{reBusca.Length} %1Jogos %2Jogos encontrados!");
                        if (reBusca == null && reBusca.Length <= 0)
                            break;
                        frmResultCate frm = new frmResultCate();
                        frm.Lista = reBusca;
                        tAbrirForm(frm);
                        if (SimOuNao("%2Deseja salvar estes jogos ao banco?"))
                        {
                            JogosSalvo.AddRange(reBusca);
                            RemoverDuplicados();
                        }
                        if (SimOuNao("%2Deseja abrir baixar algum dos jogos?"))
                        {
                            cTools.Escrever("Digite o ID: ");
                            Resp = Console.ReadLine();
                            if (int.TryParse(Resp, out iRet))
                            {
                                if (iRet > -1 && iRet < reBusca.Length)
                                {
                                    Download(reBusca[iRet]);
                                }
                            }
                            else
                            {
                                cTools.Escrever("ID Inexistente.", ConsoleColor.Red);
                            }
                        }
                    }
                    catch
                    {

                        cTools.Escrever("Error ao pesquisar jogo", ConsoleColor.Red);
                        break;
                    }
                    break;
                case "/sair":
                    if (SimOuNao("%2Voce deseja realmente sair?"))
                    {
                        cTools.EscreverRainbow("Ate a proxima.");
                        Thread.Sleep(1000);
                        Environment.Exit(1);
                    }
                    break;
                default:
                    break;
            }
        }
        static void AtualizarDB()
        {
            Console.Clear();
            int Quantidade = JogosSalvo.Count;
            for (int i = 1; i < 10000; i++)
            {
                try
                {

                    Console.Title = $"{Quantidade} jogos encontrados.";
                    cTools.Escrever($"Buscando na pagina {i}.", ConsoleColor.Green);
                    cJogo[] Busca = cJogo.PegarPagina(i);
                    JogosSalvo.AddRange(Busca);
                    Quantidade = JogosSalvo.Count;
                    cTools.Escrever($"%2Removendo itens duplicados.");
                    RemoverDuplicados();
                    cTools.Escrever($"%1{Quantidade - JogosSalvo.Count} %2Itens removidos.");
                }
                catch { break; }
            }


        }
        static void RemoverDuplicados()
        {
            for (int i = JogosSalvo.Count - 1; i > 0; i--)
            {
                cJogo jogo = JogosSalvo[i];
                bool Remover = false;
                foreach (cJogo j in JogosSalvo)
                {
                    if (j == jogo)
                        continue;
                    if (jogo.Link == j.Link)
                    {

                        Remover = true;
                        break;
                    }
                }
                if (Remover)
                    JogosSalvo.RemoveAt(i);
            }
            SalvarJson();
        }
        static void NovoJson()
        {
            try
            {
                JogosSalvo = new List<cJogo>();
                File.WriteAllText("jogos.json", JsonConvert.SerializeObject(JogosSalvo.ToArray()));
            }
            catch
            {
                cTools.Escrever("Error ao iniciar novo json", ConsoleColor.Red);
                Console.ReadLine();
                Environment.Exit(1);
            }
        }
        static void CarregarJson()
        {
            try
            {
                if (!File.Exists("jogos.json"))
                    NovoJson();

                string Response = File.ReadAllText("jogos.json");
                JogosSalvo = new List<cJogo>();
                Categorias = new List<string>();
                foreach (cJogo Jogo in JsonConvert.DeserializeObject<cJogo[]>(Response))
                {
                    JogosSalvo.Add(Jogo);
                    foreach (string c in Jogo.Categorias)
                    {
                        string c2 = c.Replace("-", " ");
                        if (!Categorias.Contains(c2))
                        {
                            Categorias.Add(c2);
                        }
                    }
                }

            }
            catch
            {
                cTools.Escrever("Error ao carregar json", ConsoleColor.Red);
                Console.ReadLine();
                Environment.Exit(1);
            }


        }
        static void SalvarJson()
        {
            try
            {
                string Response = JsonConvert.SerializeObject(JogosSalvo.ToArray());
                File.WriteAllText("jogos.json", Response);
            }
            catch
            {
                cTools.Escrever("Error ao salvar json", ConsoleColor.Red);
                Console.ReadLine();
                Environment.Exit(1);
            }
        }
        public static string PegarTexto(string Text, int Max)
        {
            string Novo = "";
            for (int i = 0; i < Max; i++)
            {
                if (i < Text.Length)
                    Novo += Text[i];
                else
                    Novo += " ";
            }
            return Novo;
        }

    }


}
