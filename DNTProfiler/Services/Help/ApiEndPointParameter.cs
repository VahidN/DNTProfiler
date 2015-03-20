namespace DNTProfiler.Services.Help
{
    public class ApiEndPointParameter
    {
        public string Name { get; set; }
        public string Documentation { get; private set; }
        public string Source { get; private set; }

        public ApiEndPointParameter(string name, string documentation, string source)
        {
            Name = name;
            Documentation = documentation;
            Source = source;
        }
    }
}