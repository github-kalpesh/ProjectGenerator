namespace ProjectGenerator.Executor
{
    public class Configuration
    {
        public Configuration() { }

        public string ProjectName { get; set; }

        /// <summary>
        /// Override all file's with new
        /// </summary>
        public bool IsOverride { get; set; }
        public string ConnectionString { get; set; }
        public string OutputDirectory { get; set; }
        public List<PageDetail> Pages { get; set; }
        
        public string RepositoryDirectory { get { return Path.Combine(OutputDirectory, "Repository"); } }
        public string APIControllerDirectory { get { return Path.Combine(OutputDirectory, "API"); } }
        public string ServicesDirectory { get { return Path.Combine(OutputDirectory, "Services"); } }
        public string ModelDirectory { get { return Path.Combine(OutputDirectory, "Models"); } }
        public string MappingDirectory { get { return Path.Combine(OutputDirectory, "Mapping"); } }
        public string MVCControllerDirectory { get { return Path.Combine(OutputDirectory, "Controller"); } }
        public string ScriptDirectory { get { return Path.Combine(OutputDirectory, "Scripts/Controller"); } }
    }

    public class PageDetail
    {
        public string TableName { get; set; }
        public string PageName { get; set; }
        public string DisplayPageName {
            get 
            {
                if (string.IsNullOrWhiteSpace(PageName))
                {
                    return TableName;
                }
                return PageName;
            }
        }
        public bool IsGenerateFrontEnd { get; set; }
        public bool IsGenerateBackEnd { get; set; }
    }
}
