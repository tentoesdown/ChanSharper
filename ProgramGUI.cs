using System;
using System.Net;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Parser.Html;
using AngleSharp.Extensions;
using System.Windows.Forms;

namespace ChanScraper
{
    public partial class ProgramGUI : Form
    {
        HtmlParser parser = new HtmlParser();
        
        public ProgramGUI()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnScrape_Click(object sender, EventArgs e)
        {
            folderBrowser.ShowDialog();
            lblProgress.Text = folderBrowser.SelectedPath;
            Scrape(txtURL.Text);
        }

        private void Scrape(string url)
        {
            string page = DownloadString(url);
            var document = parser.Parse(page);
            var links = document.GetElementsByClassName("fileText");

            int current = 0, max = links.Length;

            foreach (var t in links)
            {
                lblProgress.Text = (++current).ToString() + "/" + max.ToString();
                lblProgress.Update();
                string imageLink = "http:" + t.FirstElementChild.GetAttribute("href");
                string imageOut = folderBrowser.SelectedPath+ "\\" + t.FirstElementChild.Text();
                Console.WriteLine("LINK: " + imageLink);
                Console.WriteLine("FILE: " + imageOut);
                DownloadFile(imageLink, imageOut);
            }
            lblProgress.Text = "Done!";
            lblProgress.Update();
        }

        static void DownloadFile(string url, string fileName)
        {
            using (WebClient client = new WebClient())
            {
                client.Headers.Add("User-Agent", "Mozilla/5.0");
                client.DownloadFile(url, fileName);
            }
        }

        static string DownloadString(string url)
        {
            string str;
            using (WebClient client = new WebClient())
            {
                client.Headers.Add("User-Agent", "Mozilla/5.0");
                str = client.DownloadString(url);
            }

            return str;
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }
    }
}
