namespace Forage.Common
{
    public static class Helpers
    {
        public static class Files
        {
            public static string ReadAllText(string name)
            {
                string fileContents = string.Empty;

                if (File.Exists(name))
                {
                    fileContents = File.ReadAllText(name);
                }

                return fileContents;
            }
        }
    }
}
