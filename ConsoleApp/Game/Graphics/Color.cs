namespace ConsoleApp.Graphics {
    
    // Class for changing background or text color, google ANSI-Coloring if interested
    public static class Color {
        
        public static string Reset => "\x1b[0m";
        public static string BlackText => "\x1b[30m";
        public static string BlackBackground => "\x1b[40m";
        public static string RedText => "\x1b[31m";
        public static string RedBackground => "\x1b[41m";
        public static string GreenText => "\x1b[32m";
        public static string GreenBackground => "\x1b[42m";
        public static string YellowText => "\x1b[33m";
        public static string YellowBackground => "\x1b[43m";
        public static string BlueText => "\x1b[34m";
        public static string BlueBackground => "\x1b[44m";
        public static string PurpleText => "\x1b[35m";
        public static string PurpleBackground => "\x1b[45m";
        public static string CyanText => "\x1b[36m";
        public static string CyanBackground => "\x1b[46m";
        public static string GrayLightText => "\x1b[37m";
        public static string GrayLightBackground => "\x1b[47m";
        public static string GrayDarkText => "\x1b[90m";
        public static string GrayDarkBackground => "\x1b[100m";
        public static string RedLightText => "\x1b[91m";
        public static string RedLightBackground => "\x1b[101m";
        public static string GreenLightText => "\x1b[92m";
        public static string GreenLightBackground => "\x1b[102m";
        public static string YellowLightText => "\x1b[93m";
        public static string YellowLightBackground => "\x1b[103m";
        public static string BlueLightText => "\x1b[94m";
        public static string BlueLightBackground => "\x1b[104m";
        public static string PinkText => "\x1b[95m";
        public static string PinkBackground => "\x1b[105m";
        public static string CyanLightText => "\x1b[96m";
        public static string CyanLightBackground => "\x1b[106m";
        public static string WhiteText => "\x1b[97m";
        public static string WhiteBackground => "\x1b[107m";
    }
}