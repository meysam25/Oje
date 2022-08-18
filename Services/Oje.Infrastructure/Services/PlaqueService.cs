namespace Oje.Infrastructure.Services
{
    public class PlaqueService
    {
        public static int? ConvertStringToPlaque(string input)
        {
            input = input + "";
            input = input.Trim();

            switch (input)
            {
                case "الف":
                    return 1;
                case "ب":
                    return 2;
                case "ت":
                    return 3;
                case "ج":
                    return 4;
                case "د":
                    return 5;
                case "س":
                    return 6;
                case "ص":
                    return 7;
                case "ط":
                    return 8;
                case "ع":
                    return 9;
                case "ق":
                    return 10;
                case "ل":
                    return 11;
                case "م":
                    return 12;
                case "ن":
                    return 13;
                case "و":
                    return 14;
                case "ه":
                    return 15;
                case "ی":
                    return 16;
                case "ر":
                    return 17;
                case "ک":
                    return 18;
                case "ژ":
                    return 19;
                case "پ":
                    return 20;
                default:
                    return null;
            }
        }
    }
}
