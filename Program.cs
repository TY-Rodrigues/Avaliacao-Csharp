using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace ConsultaBancosBrasileiros
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }

    public partial class Form1 : Form
    {
        private DataGridView dataGridView;

        public Form1()
        {
            InitializeDataGridView();
            LoadData();
        }

        private void InitializeDataGridView()
        {
            dataGridView = new DataGridView();
            dataGridView.Dock = DockStyle.Fill;
            
            dataGridView.Columns.Add("ISPB", "ISPB");
            dataGridView.Columns.Add("Code", "Código");
            dataGridView.Columns.Add("Name", "Nome");
            dataGridView.Columns.Add("FullName", "Nome Completo");

            Controls.Add(dataGridView);
        }

        private async void LoadData()
        {
            string url = "https://brasilapi.com.br/api/banks/v1";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    List<Banco> bancos = JsonConvert.DeserializeObject<List<Banco>>(jsonResponse);

                    foreach (Banco banco in bancos)
                    {
                        dataGridView.Rows.Add(banco.ISPB, banco.Code, banco.Name, banco.FullName);
                    }
                }
                else
                {
                    MessageBox.Show("Erro ao consultar a lista de bancos.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }

    class Banco
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string ISPB { get; set; }
        public string FullName { get; set; }
    }
}
