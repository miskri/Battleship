using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Text.RegularExpressions;
using ConsoleApp.Graphics;

namespace ConsoleApp.Utils {
    
    // Class contains some design elements, settings for JSON and 2 functions: object deep cloning and array add function.
    public static class DataUtils {
        
        public static string BattleMessageMiss => $"{Color.WhiteText}miss{Color.Reset}";
        public static string BattleMessageHit => $"{Color.RedText}ship damaged{Color.Reset}";
        public static string Default => Color.BlackBackground + Color.YellowText;
        public static string Menu => Color.BlackBackground + Color.WhiteText;
        public static string System => Color.BlackBackground + Color.GreenText;
        public static string FieldCell => Color.BlueBackground + "   " + Color.Reset;
        public static string ShipCell => Color.GrayDarkBackground + "   " + Color.Reset;
        public static string BorderCell => Color.GrayLightBackground + "   " + Color.Reset;
        public static string ValidTemplateCell => Color.GreenLightBackground + "   " + Color.Reset;
        public static string InvalidTemplateCell => Color.RedLightBackground + "   " + Color.Reset;
        public static string CollisionCell => Color.RedBackground + Color.BlackText + " X " + Color.Reset;
        public static string MissCell => Color.BlueLightBackground + Color.BlackText + " ♦ " + Color.Reset;

        public static string Path => Regex.Replace(Environment.CurrentDirectory, @"bin[\\\/]Debug[\\\/]netcoreapp3.1", "");
        public static char[] Alphabet => new[] {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
            'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
        };
        public static JsonSerializerOptions JsonOptions => new JsonSerializerOptions(){WriteIndented = true};

        public static T DeepClone<T>(this T obj) {
            using MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, obj);
            stream.Position = 0;

            return (T) formatter.Deserialize(stream);
        }
        
        public static T[] Add<T>(this T[] target, T item)
        {
            T[] result = new T[target.Length + 1];
            target.CopyTo(result, 1);
            result[0] = item;
            return result;
        }
    }
}