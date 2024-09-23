using System;
using System.Drawing;
using System.Windows.Forms;
using Text_Library;


namespace SymbolicTextAnalysis
{
    public partial class Form1 : Form
    {
        private string fn = string.Empty; // имя файла   
        private bool docChanged = false; // true - в текст внесены изменения

        public Form1()
        {
            InitializeComponent();
            this.richTextBox1.Dock = DockStyle.Fill;
            richTextBox1.ScrollToCaret();
            richTextBox1.Text = string.Empty;
            richTextBox1.Font = new Font("Consolas", 12);//очистить текст

            this.Text = "Частотный анализ текста(символьный) - новый докуменит";      //заголовок формы
            // настройка компонента openDialog1
            openFileDialog1.DefaultExt = "txt";
            openFileDialog1.Filter = "текст|*.txt";
            openFileDialog1.Title = "Открыть документ";
            openFileDialog1.Multiselect = false;

            // настройка компонента saveDialog1
            saveFileDialog1.DefaultExt = "txt";
            saveFileDialog1.Filter = "текст|*.txt";
            saveFileDialog1.Title = "Сохранить документ";

        }

        // сохранить документ
        private bool SaveDocument()
        {
            bool result = false;
            if (fn == string.Empty)
            {
                // отобразить диалог Сохранить
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    // отобразить имя файла в заголовке окна
                    fn = saveFileDialog1.FileName;
                    this.Text = fn;                   
                }
            }
            // сохранить файл
            if (fn != string.Empty)
            {
                try
                {
                    Text_.Save(fn, richTextBox1.Text);
                    result = true;
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.ToString(), "Частотный анализ текста(символьный)",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                }
            }
            return result;

        }
        //Предложить сохранить документ при закритии формы
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (docChanged)
            {
                DialogResult dr;
                dr = MessageBox.Show("Сохранить документ?", "Частотный анализ текста(символьный)",
                                      MessageBoxButtons.YesNoCancel,
                                      MessageBoxIcon.Warning);
                switch (dr)
                {
                    case DialogResult.Yes:

                        if (SaveDocument())
                        {
                            richTextBox1.Clear();
                            docChanged = false;
                        }
                        else
                        {
                            e.Cancel = true;
                        }
                        break;
                    case DialogResult.No:
                        richTextBox1.Clear();
                        docChanged = false;
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }
        }

        //Отслеживает изменения в тексте
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            docChanged = true;
        }

        //Открыть форму настройки анализа
        private void провестиАнализToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Form2(richTextBox1.Text).ShowDialog();
        }

        //Сохранить текст в файл
        private void сохранитьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveDocument();
        }

        //Открыть текст из файла
        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = string.Empty;

            // отобразить диалог Открыть
            if (openFileDialog1.ShowDialog() ==
                                DialogResult.OK)
            {

                fn = openFileDialog1.FileName;

                // отобразить имя файла в заголовке окна
                this.Text = fn;
                try
                {
                    // считываем данные из файла
                    richTextBox1.Text = Text_.Open(fn);
                    richTextBox1.SelectionStart = richTextBox1.TextLength;
                }
                catch (Exception exc)
                {
                    MessageBox.Show("Ошибка чтения файла.\n" +
                        exc.ToString(), "Частотный анализ текста(символьный)",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        //Открыть форму справки
        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Справка().ShowDialog();
        }
    }
}
