namespace Blog.Web.Helpers
{
    public static class StringExtensions
    {
        // Enable quick and more natural string.Format calls
        public static string Fmt(this string s, params object[] args)
        {
            return string.Format(s, args);
        }
    }
}