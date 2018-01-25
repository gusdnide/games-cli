using System.Collections.Generic;
using System.Text;
using System.Net;
using System;
using System.Windows.Forms;
namespace games_cli
{
    class cTools
    {
        public static void Escrever(string Texto)
        {
            ConsoleColor corPad = Console.ForegroundColor;
            bool cor = false;
            for (int i = 0; i < Texto.Length; i++)
            {

                if ((Texto[i] == '%' && i + 1 < Texto.Length) || cor)
                {

                    if (!cor)
                    {
                        int Saida = -1;
                        if (int.TryParse(Texto[i + 1].ToString(), out Saida))
                        {
                            if (Saida > -1 && Saida <= 5)
                            {
                                switch (Saida)
                                {
                                    case 0:
                                        Console.ForegroundColor = ConsoleColor.Blue;
                                        break;
                                    case 1:
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        break;
                                    case 2:
                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        break;
                                    case 3:
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        break;
                                    case 4:
                                        Console.ForegroundColor = ConsoleColor.White;
                                        break;
                                    case 5:
                                        Console.ForegroundColor = ConsoleColor.Cyan;
                                        break;
                                    default:
                                        Console.ForegroundColor = ConsoleColor.Gray;
                                        break;
                                }
                            }
                        }
                    }
                    cor = !cor;
                    continue;
                }
                Console.Write(Texto[i]);
            }
            Console.WriteLine();
            Console.ForegroundColor = corPad;
        }
        public static void EscreverMod(string Texto)
        {
            ConsoleColor corPad = Console.ForegroundColor;
            bool cor = false;
            for (int i = 0; i < Texto.Length; i++)
            {

                if ((Texto[i] == '%' && i + 1 < Texto.Length) || cor)
                {

                    if (!cor)
                    {
                        int Saida = -1;
                        if (int.TryParse(Texto[i + 1].ToString(), out Saida))
                        {
                            if (Saida > -1 && Saida <= 5)
                            {
                                switch (Saida)
                                {
                                    case 0:
                                        Console.ForegroundColor = ConsoleColor.Blue;
                                        break;
                                    case 1:
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        break;
                                    case 2:
                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        break;
                                    case 3:
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        break;
                                    case 4:
                                        Console.ForegroundColor = ConsoleColor.White;
                                        break;
                                    case 5:
                                        Console.ForegroundColor = ConsoleColor.Cyan;
                                        break;
                                    default:
                                        Console.ForegroundColor = ConsoleColor.Gray;
                                        break;
                                }
                            }
                        }
                    }
                    cor = !cor;
                    continue;
                }
                Console.Write(Texto[i]);
            }
            Console.ForegroundColor = corPad;
        }
        public static void Escrever(string texto, ConsoleColor cor = ConsoleColor.Gray)
        {
            ConsoleColor corPad = Console.ForegroundColor;
            Console.ForegroundColor = cor;
            Console.WriteLine(texto);
            Console.ForegroundColor = corPad;
        }
        public static void EscreverRainbow(string texto)
        {
            ConsoleColor corPad = Console.ForegroundColor;
            var rnd = new Random();
            foreach (char c in texto)
            {

                Console.ForegroundColor = (ConsoleColor)rnd.Next(Enum.GetNames(typeof(ConsoleColor)).Length);
                Console.Write(c);
            }
            Console.WriteLine();
            Console.ForegroundColor = corPad;
        }
        public static string[] GetCategorias(string classes)
        {
            List<string> ret = new List<string>();
            string[] div = classes.Split(' ');
            if (div.Length > 0)
            {
                foreach (string s in div)
                {
                    if (s.Contains("category"))
                    {
                        ret.Add(s.Replace("category-", ""));
                    }
                }
            }
            return ret.ToArray();
        }
        public static HtmlElement[] GetClassName(HtmlDocument doc, string className)
        {
            List<HtmlElement> el = new List<HtmlElement>();
            foreach (HtmlElement e in doc.Body.All)
            {
                if (e.GetAttribute("className") == (className))
                {
                    el.Add(e);
                }
            }
            return el.ToArray();
        }
        public static HtmlElement[] GetClassName2(HtmlDocument doc, string className)
        {
            List<HtmlElement> el = new List<HtmlElement>();
            foreach (HtmlElement e in doc.Body.All)
            {
                if (e.GetAttribute("className").Contains(className))
                {
                    el.Add(e);
                }
            }
            return el.ToArray();

        }
        public static HtmlDocument GetDoc(string html)
        {
            WebBrowser browser = new WebBrowser();
            browser.ScriptErrorsSuppressed = true;
            browser.DocumentText = html;
            browser.Document.OpenNew(true);
            browser.Document.Write(html);
            browser.Refresh();
            return browser.Document;
        }
        public static string GetResponse(string url)
        {
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            return wc.DownloadString(url);
        }
    }
}
