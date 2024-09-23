using System;
using System.Drawing;
using System.Windows.Forms;
using Simvol_Library;

namespace SymbolicTextAnalysis
{
    public partial class Form2 : Form
    {
        private string TextToAnalysis;
        public Form2(string s)
        {
            InitializeComponent();
            label1.Text = "Введите нужные символы без пробелов " + '\n' + " и разделяющих знаков:";
            TextToAnalysis = s;
            label1.Visible = false;
            textBox1.Visible = false;
            radioButton4.Checked = true;
        }
        //передача результата анализа форме отчет
        private void button1_Click(object sender, EventArgs e)
        {
            Simvol[] arr;
            if (radioButton1.Checked)
            {
               arr  = Simvol.AnalysisForAll(TextToAnalysis,radioButton3.Checked);
            }
            else
            {
                arr = Simvol.AnalysisForAllInList(TextToAnalysis, radioButton3.Checked, textBox1.Text);
            }
            Hide();
            new Отчет(arr).ShowDialog();            
        }

        //Открытие поля для ввода сымволод для которых проводить анализ
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                this.MaximumSize = new Size(280, 310);
                this.MinimumSize = new Size(280, 310);
                groupBox1.Size = new Size(242, 130);
                groupBox2.Location = new Point(11, 152);
                button1.Location = new Point(64, 240);
                label1.Visible = true;
                textBox1.Visible = true;
            }
            else
            {               
                label1.Visible = false;
                textBox1.Visible = false;
                groupBox1.Size = new Size(242, 70);
                groupBox2.Location = new Point(11, 93);
                button1.Location = new Point(64, 180);
                this.MaximumSize = new Size(280, 250);
                this.MinimumSize = new Size(280, 250);
            }

        }
    }
}