using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
namespace games_cli
{
    public class cJogo
    {
        public string Imagem { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Link { get; set; }
        public string Trailer { get; set; }
        public string Download { get; set; }
        public string[] Categorias { get; set; }
        public cJogo() { }
        public cJogo(string n, string i, string d, string l, string[] c)
        {
            this.Imagem = i;
            this.Nome = n;
            this.Descricao = d;
            this.Link = l;
            this.Categorias = c;
        }
        public void Atualizar()
        {
            try
            {
                Download = "";
                Trailer = "";
                string Resp = cTools.GetResponse(this.Link);
                HtmlDocument doc = cTools.GetDoc(Resp);
                var ElementosButton = cTools.GetClassName2(doc, "btn with-icon btn-primary btn-lg btn-app");
                var ElementosTrailer = doc.Body.GetElementsByTagName("source");
                if (ElementosButton.Count() > 0)
                {
                    foreach (var e in ElementosButton)
                    {
                        if (e.InnerText.ToLower().Contains("baixar o jogo"))
                        {
                            this.Download = e.GetAttribute("href");
                            break;
                        }
                    }
                }
                if (ElementosTrailer.Count > 0)
                {
                    this.Trailer = ElementosTrailer[0].GetAttribute("src");
                }
            }
            catch { }

        }
        public static cJogo[] ProcurarJogo(string nome)
        {
            string Url = $"http://www.thepiratedownload.com/?s={nome.Replace(" ", "+")}";
            return PegarJogos(Url);
        }
        public static cJogo[] PegarPagina(int Pagina)
        {
            string Url = $"http://www.thepiratedownload.com/category/games/page/{Pagina}";
            return PegarJogos(Url);
        }
        public static cJogo[] PegarJogos(string url)
        {
            List<cJogo> Retorno = new List<cJogo>();

            string Response = cTools.GetResponse(url);
            if (Response.Contains("Erro 404 - Página não encontrada"))
                return Retorno.ToArray();

            HtmlDocument mainContent = cTools.GetDoc(Response);
            var ElementosImg = cTools.GetClassName(mainContent, "post-image");
            var ElementosTitulo = cTools.GetClassName(mainContent, "post-header");
            var ElementosInfo = cTools.GetClassName(mainContent, "post-info");
            var ElementosLink = cTools.GetClassName(mainContent, "post-footer");
            var ElementosArticle = mainContent.Body.GetElementsByTagName("article");
            for (int i = 0; i < ElementosArticle.Count; i++)
            {
                try
                {
                    string Imagem = ElementosImg[i].GetElementsByTagName("img")[0].GetAttribute("src");
                    string Titulo = ElementosTitulo[i].GetElementsByTagName("h3")[0].InnerText;
                    string Info = ElementosInfo[i].GetElementsByTagName("p")[0].InnerText;
                    string Link = ElementosLink[i].GetElementsByTagName("a")[0].GetAttribute("href");
                    string[] Categorias = cTools.GetCategorias(ElementosArticle[i].GetAttribute("className"));
                    Retorno.Add(new cJogo(Titulo, Imagem, Info, Link, Categorias));
                }
                catch
                {
                    continue;
                }
            }
            return Retorno.ToArray();
        }
        public static cJogo[] BuscarPorCategoria(cJogo[] db, string ca)
        {
            List<cJogo> Retorno = new List<cJogo>();
            foreach(cJogo j in db)
            {
                if (j.Categorias.Contains(ca))
                    Retorno.Add(j);
            }
            return Retorno.ToArray();
        }
        public static cJogo[] BuscarDB(cJogo[] db, string nome)
        {
            List<cJogo> Retorno = new List<cJogo>();
            foreach(cJogo j in db)
            {
                if (j.Nome.ToLower().Contains(nome.ToLower()))
                    Retorno.Add(j);
            }
            return Retorno.ToArray();
        }
        public static int BuscarPorLink(cJogo[] db, string link)
        {
            int count = 0;
            foreach(cJogo j in db)
            {
                if (j.Link == link)
                    return count;
                count++;
            }
            return -1;
        }
    }
}
