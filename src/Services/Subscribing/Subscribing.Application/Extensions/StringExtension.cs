namespace Subscribing.Application.Extensions
{
    public static class StringExtension
    {
        public static string ReplaceFirstOcurrence(this string source, string find, string replace)
        {
            int place = source.IndexOf(find);
            string result = source.Remove(place, find.Length).Insert(place, replace);
            return result;
        }
    }
}