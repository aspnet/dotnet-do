namespace DotNetDo.BuildSystem
{
    public class Project
    {
        public string Name { get; }
        public string Path { get; }

        public Project(string name, string path)
        {
            Name = name;
            Path = path;
        }
    }
}