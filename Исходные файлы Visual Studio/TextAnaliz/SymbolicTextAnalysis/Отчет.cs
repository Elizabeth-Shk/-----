using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using Simvol_Library;
using Text_Library;

namespace SymbolicTextAnalysis
{
    public partial class Отчет : Form
    {
        private string fn = string.Empty;
        private Simvol[] Arr = null;
        private string strToPrint_Save = "        Результаты частотного анализа текста(символьный)\n"+'\n'+"|Символ|Код символа|Абсолютная частота|Относительная частота, %|" + '\n';
        public Отчет(Simvol[] arr)
        {
            InitializeComponent();
            Arr = arr;
            dataGridView1.RowHeadersVisible = false;
            for (int i = 1; i < Arr.Length; i++)
            {
                dataGridView1.Rows.Add();
            }
            for (int i = 0; i < Arr.Length; i++)
            {
                dataGridView1.Rows[i].SetValues(Arr[i].Simv, Arr[i].Code, Arr[i].Count.ToString(), Arr[i].OtnCount.ToString("F3"));
            }
            if (Arr.Length > 18)
            {
                dataGridView1.Size = new Size(485, 422);
                this.MaximumSize = new Size(522, 500);
                this.MinimumSize = new Size(522, 500);
            }
            else
            {
                dataGridView1.Size = new Size(468, 23+22* Arr.Length); 
                this.MaximumSize = new Size(507, 23 + 22 * Arr.Length + 80); 
                this.MinimumSize = new Size(507, 23 + 22 * Arr.Length + 80);
            }

            // настройка компонента saveDialog1
            saveFileDialog1.DefaultExt = "txt";
            saveFileDialog1.Filter = "текст|*.txt";
            saveFileDialog1.Title = "Сохранить документ";
        }

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
                    //string strToPrint_Save = "Результаты частотного анализа текста(символьный)\n" + "Символ|Код символа|Абсолютная частота|Относительная частота" + '\n';
                    if (strToPrint_Save == "        Результаты частотного анализа текста(символьный)\n" + '\n' + "|Символ|Код символа|Абсолютная частота|Относительная частота, %|" + '\n')
                    {
                        for (int i = 0; i < Arr.Length; i++)
                        {
                            strToPrint_Save += '|' + Arr[i].Simv.PadRight("Символ".Length) + '|' + Arr[i].Code.ToString().PadLeft("Код символа".Length) + '|' + Arr[i].Count.ToString().PadLeft("Абсолютная частота".Length) + '|' + Arr[i].OtnCount.ToString("F3").PadLeft("Относительная частота, %".Length) + '|' + '\n';
                        }
                    }
                    Text_.Save(fn, strToPrint_Save);
                    result = true;
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.ToString(), "Частотный анализ текста(символьный)",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                }
            }
            if (result)
            {
                MessageBox.Show("Таблица сохранена в формате txt. Развелителем является символ "+"'|'"+" (вертикальная строка).", "Частотный анализ текста(символьный)");
            }
            return result;
        }
        private void сохранитьToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            SaveDocument();
        }

        private void печатьToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            Font font = new Font("Consolas", 12);
            if(strToPrint_Save == "        Результаты частотного анализа текста(символьный)\n" + '\n' + "|Символ|Код символа|Абсолютная частота|Относительная частота, %|" + '\n')
            {
                for (int i = 0; i < Arr.Length; i++)
                {
                    //string OtC = Arr[i].OtnCount.ToString("F3") + "%";
                    strToPrint_Save += '|' + Arr[i].Simv.PadRight("Символ".Length) + '|' + Arr[i].Code.ToString().PadLeft("Код символа".Length) + '|' + Arr[i].Count.ToString().PadLeft("Абсолютная частота".Length) + '|' + Arr[i].OtnCount.ToString("F3").PadLeft("Относительная частота, %".Length) + '|' + '\n';
                }
            }
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                // Получение выбранных настроек печати и файла для печати
                PrinterSettings printerSettings = printDialog1.PrinterSettings;
                string filePath = fn;

                // Код для передачи файла на печать
                using (PrintDocument printDocument = new PrintDocument())
                {
                    printDocument.PrinterSettings = printerSettings;
                    Text_ a = new Text_();
                    a.Print(font, strToPrint_Save);
                }

            }

        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
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
                        dataGridView1.Rows.Clear();
                     }
                     else
                     {
                        e.Cancel = true;
                     }
                     break;
                case DialogResult.No:
                    dataGridView1.Rows.Clear();
                    break;
                case DialogResult.Cancel:
                    e.Cancel = true;
                    break;
            }
        }
    }
}