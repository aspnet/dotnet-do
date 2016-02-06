# Random Idea dumping ground

## Project layout when using `dotnet-do`

```
[Repo Root]/
    tasks/
        project.json
        Program.cs
        SomeOfMyTasks.cs
        MoreOfMyTasks.cs
        ...
    src/
        ...
    test/
        ...
    ...
```

## The dotnet-do command

```
dotnet do [--debug] [task]
```

Walks up the file system from the current dir, searching for `tasks/project.json` (maybe have a marker field in project.json to make sure we're getting a tasks project? `dotnet-do-project: true`?). When it finds it, it runs `dotnet run [args]` it in the current directory (passing along all the args given to `dotnet do`).

If `--debug` is specified, it is passed along to the tasks app, which (by using the default Program.Main) will automatically print the PID to the console and wait for a debugger to attach. Maybe also have a `--debug-launch` switch that launches the debugger immediately on Windows?

If `[task]` is not specified, the task named `Default` is run.

If `[task]` is the special task name `init`, then a new `dotnet do` project structure is created in `[current directory]/tasks`

## "Helpers"

We can provide a bunch of helpers in some static types and use `using static` to import them!

```csharp
namespace DotNetDo
{
    public static class Helpers
    {
        public void Exec(string command, params string[] args) {...}
        public void Call(string task) {...}
        public string Env(string variableName) {...}
        public void Env(string variableName, string newValue) {...}
    }
}

// in a user's tasks project ...

using static DotNetDo.Helpers;
namespace MyTasks
{
    public class MyTasks: TaskCollection
    {
        [Task]
        public void Thingy()
        {
            Exec("chmod", "a+x", "frob");
            Call("AnotherTask");
            if(Env("PATH").Contains("...")) {
            }
        }
    }
}
```
