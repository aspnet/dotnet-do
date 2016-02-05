namespace DotNetDo.Engine
{
    public interface ITaskManager
    {
        TaskResult ExecuteTask(string task);
    }
}