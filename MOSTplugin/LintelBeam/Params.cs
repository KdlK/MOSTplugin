using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOSTplugin.LintelBeam
{
    public static class Params
    {


        public static Dictionary<string, TableFormatSizes> FormatSizes = new Dictionary<string, TableFormatSizes>() { 
            { "Мелкие", new TableFormatSizes(50,100) },
            { "Средние", new TableFormatSizes(60,120) },
            { "Большие", new TableFormatSizes(150,300) },
            



        };
        public static string selectedImageSize = null;
        
        

        
        
        
        

        public static string MarkParameterName = "ADSK_Марка";//Параметр, в котором хранится марка перемычки
        public static string CodeParameterName = "Комментарии"; //Параметр, в котором хранится код перемычки
        public static string PMotherParameterName = "Группа модели"; //Название параметра, который отличает Составную перемычку от вложенных перемычек
        public static string PMotherParameterValue = "Составная перемычка"; // Значение параметра "Группа модели". Этот параметр необходим, что бы собирать перемычки.
        public static string Sub1ParameterName = "М_Субкомпонент_Тип_1"; //Название параметра, в котором хранится марка субкомпонента1 Составной перемычки
        public static string Sub2ParameterName = "М_Субкомпонент_Тип_2"; //Название параметра, в котором хранится марка субкомпонента2 Составной перемычки
        public static string Sub3ParameterName = "М_Субкомпонент_Тип_3"; //Название параметра, в котором хранится марка субкомпонента3 Составной перемычки
        public static string Sub4ParameterName = "М_Субкомпонент_Тип_4"; //Название параметра, в котором хранится марка субкомпонента4 Составной перемычки
        public static string Sub5ParameterName = "М_Субкомпонент_Тип_5"; //Название параметра, в котором хранится марка субкомпонента5 Составной перемычки
        public static string Sub6ParameterName = "М_Субкомпонент_Тип_6"; //Название параметра, в котором хранится марка субкомпонента6 Составной перемычки
        public static string WidthParameterName = "ADSK_Размер_Ширина";
        public static string LenghtParameterName = "ADSK_Размер_Длина";
        public static string IndentParameterName = "М_Перемычка_Отступ_фасадный";
        public static string OffsetParameterName = "М_Четверть_Разрез_Размер";
        public static string ViewSectionTemplate = "Р_Р_Разрез перемычек";
    }

    public struct TableFormatSizes
    {
        public int height;
        public int width;
        public TableFormatSizes(int height, int width)
        {
            this.height = height;
            this.width = width;
        }
    }
}
