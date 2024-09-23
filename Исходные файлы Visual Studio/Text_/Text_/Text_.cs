using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Printing;

namespace Text_Library
{
    public class Text_
    {
        private string[] lines; //Массив строк в тексте
        private int currentLine; //Линия, на которой остановилась печать
        private Font printFont; //Шрифт, который используется для печати документа 
        // Метод печати файла
        // Входные параметры: передаются параметры шрифта
        // Результат: переданный текст выводится на печать, 
        //            если нет ошибки, передается результат true

        public static void Save(string path, string text)
        {
            FileInfo fi = new FileInfo(path);
            StreamWriter sw = fi.CreateText();
            sw.Write(text);
            sw.Close();
        }
        public static string Open(string path)
        {
            using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
            {
                return sr.ReadToEnd();
            }
        }

        //Метод для печати результата работы программы
        public bool Print(Font pF, string currentText)
        {
            lines = currentText.Split('\n'); //Массив линий - разделение текста по символу переноса строки
            currentLine = 0; //Текущая линия устанавливается на 0
            printFont = pF; //Установка шрифта для печати
            try
            {
                PrintDocument pd = new PrintDocument(); //Создание документа для печати
                //Указание того, что событие печати страницы будет вызывать метод pd_PrintPage
                pd.PrintPage += new PrintPageEventHandler(this.PrintPage);

                pd.Print(); //Печать документа
                return true; //Возвращение успешного результата печати
            }
            catch
            {
                return false;
            }
        }
        //Метод, который контролирует печать.
        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            float yPos = 0;//Позиция для следующей строки (считая от верха)
            int count = 0; //Количество напечатанных строк

            float leftMargin = ev.MarginBounds.Left; //Левый отступ при печати
            float topMargin = ev.MarginBounds.Top;  //Верхний отступ при печати

            int linesPerPage = (int)(ev.MarginBounds.Height / printFont.GetHeight(ev.Graphics)); //Количество линий на странице

            //Цикл начинается со значения поля currentLine, и заканчивается, когда достигает количества строк в тексте, или количества строк на страницу
            for (int i = currentLine; i < lines.Length; i++)
            {
                yPos = topMargin + (count * printFont.GetHeight(ev.Graphics)); //Рассчёт позиции для следующей строки (по вертикали)
                ev.Graphics.DrawString(lines[i], printFont, Brushes.Black, leftMargin, yPos, new StringFormat());
                count++; //Количество строк увеличивается с каждой итерацией цикла
                if (count == linesPerPage) break;
            }
            currentLine += count; //Смещение текущей строки на count

            //Если строки не закончились, распечатаем еще одну страницу
            if (currentLine < lines.Length)
                ev.HasMorePages = true;
            else
                ev.HasMorePages = false;
        }
    }
}
