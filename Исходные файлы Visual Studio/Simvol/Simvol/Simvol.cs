using System;
using System.Collections.Generic;
using System.Linq;

namespace Simvol_Library
{
    public class Simvol
    {
        public string Simv { get; set; } //Символ
        public int Code { get; private set; } //Код символа
        public int Count { get; private set; } //Количество этого символа в строке
        public double OtnCount { get; private set; } //Относительная частота использования этого символа
        public bool Reg { get; private set; } //True - регистр учитывается False - нет
        //Конструктор
        public Simvol(char simv, string text, bool reg)
        {
            Code = Convert.ToChar(simv);
            Reg = reg;
            if (Code < 32 || Code == 127)
            {
                Simv = " ";
            }
            else
            {
                if (!Reg && char.IsLetter(Convert.ToChar(Code)))
                {
                    Simv = Convert.ToString(simv).ToUpper() + Convert.ToString(simv).ToLower();
                }
                else
                {
                    Simv = Convert.ToString(simv);
                }
            }
            if (Reg || !char.IsLetter(Convert.ToChar(Code)))
            {
                Count = text.Length - text.Replace(Convert.ToChar(Code).ToString(), "").Length;
            }
            else
            {
                Count = (text.Length - text.Replace(Convert.ToChar(Code).ToString().ToLower(), "").Length) + (text.Length - text.Replace(Convert.ToChar(Code).ToString().ToUpper(), "").Length);
            }
            OtnCount = ((double)Count * 100.00) / (double)text.Length;
        }
        //Метод для частотного анализа для всех символов
        public static Simvol[] AnalysisForAll(string text, bool reg)
        {
            string ContSimv = "";
            List<Simvol> list = new List<Simvol>();
            //Выборка уникальных символов
            if (reg)
            {
                for (int i = 0; i < text.Length; i++)
                {
                    if (!ContSimv.Contains(text[i]))
                    {
                        ContSimv += text[i];
                    }
                }
            }
            else
            {
                for (int i = 0; i < text.Length; i++)
                {
                    if (!ContSimv.Contains(char.ToUpper(text[i])) && !ContSimv.Contains(char.ToLower(text[i])))
                    {
                        ContSimv += text[i];
                    }
                }
            }
            ContSimv = String.Concat(ContSimv.OrderByDescending(c => c)); //Сортировка символов
            foreach (char s in ContSimv)
            {
                list.Add(new Simvol(s, text, reg));
            }
            return list.ToArray();
        }
        //Метод для частотного анализа для выбранных символов
        public static Simvol[] AnalysisForAllInList(string text, bool reg, string simv_list)
        {
            string Simv_list = simv_list;
            List<Simvol> list = new List<Simvol>();
            Simv_list = String.Concat(Simv_list.OrderByDescending(c => c)); //Сортировка символов
            //Частотный анализ
            foreach (char s in simv_list)
            {
                list.Add(new Simvol(s, text, reg));
            }
            return list.ToArray();
        }
    }
}
