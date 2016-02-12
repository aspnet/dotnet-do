namespace DotNetDo
{
    internal class TaskRunnerActivity
    {
        public string Type { get; }
        public string Name { get; }
        public string Conclusion { get; set; }
        public bool Success { get; set; }

        public TaskRunnerActivity(string type, string name)
        {
            Type = type;
            Name = name;
        }
    }
}