using BuildSchoolBizApp.Services;
using BuildSchoolBizApp.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BuildSchoolBizApp
{
    public partial class AddSalesManForm : Form
    {
        public AddSalesManForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("欄位不得為空");
            }
            else
            {
                SalesManViewModel viewmodel = new SalesManViewModel();
                viewmodel.Name = textBox1.Text.Trim();

                SalesManService service = new SalesManService();
                var result = service.Create(viewmodel);
                if (result.IsSuccessful)
                {
                    MessageBox.Show("Success");
                }
                else
                {
                    var path = result.WriteLog();
                    MessageBox.Show($"Wrong, 參考 {path}");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
