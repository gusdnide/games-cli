using System;
using System.Windows.Forms;

namespace games_cli
{
    public partial class frmResultCate : Form
    {
        public frmResultCate()
        {
            InitializeComponent();
           
        }
        public cJogo[] Lista;
        private void frmResultCate_Load(object sender, EventArgs e)
        {
            int id = 0;
            foreach (cJogo c in Lista)
            {
                string[] row = { id.ToString(), c.Nome, c.Descricao };
                var listViewItem = new ListViewItem(row);
                this.Invoke((MethodInvoker)(() => listView1.Items.Add(listViewItem)));
                id++;

            }
        }
    }
}
