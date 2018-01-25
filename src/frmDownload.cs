using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
namespace games_cli
{
    public partial class frmDownload : Form
    {
        public frmDownload()
        {
            InitializeComponent();
        }
        public cJogo Jogo;
        private void frmDownload_Load(object sender, EventArgs e)
        {
            pictureBox1.ImageLocation = Jogo.Imagem;
            label1.Text = Jogo.Nome;
            textBox1.Text = Jogo.Descricao;
            this.Text = Jogo.Nome;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(Jogo.Download);
            }
            catch
            {
                Process.Start(Jogo.Link);
            }
        }
    }
}
