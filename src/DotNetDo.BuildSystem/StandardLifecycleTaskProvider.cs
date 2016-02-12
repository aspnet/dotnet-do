using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetDo.BuildSystem
{
    // Standard build workflow
    // 
    // Clean -> PreClean CleanCore PostClean
    // Default -> Initialize Compile Test Package Publish
    // N -> PreN NCore PostN

    public static class StandardTasks
    {
        public static readonly string Default = nameof(Default);

        public static readonly string Clean = nameof(Clean);
        public static readonly string CleanCore = nameof(CleanCore);
        public static readonly string PreClean = nameof(PreClean);
        public static readonly string PostClean = nameof(PostClean);

        public static readonly string Publish = nameof(Publish);
        public static readonly string PublishCore = nameof(PublishCore);
        public static readonly string PrePublish = nameof(PrePublish);
        public static readonly string PostPublish = nameof(PostPublish);

        public static readonly string Package = nameof(Package);
        public static readonly string PackageCore = nameof(PackageCore);
        public static readonly string PrePackage = nameof(PrePackage);
        public static readonly string PostPackage = nameof(PostPackage);

        public static readonly string Test = nameof(Test);
        public static readonly string TestCore = nameof(TestCore);
        public static readonly string PreTest = nameof(PreTest);
        public static readonly string PostTest = nameof(PostTest);

        public static readonly string Compile = nameof(Compile);
        public static readonly string CompileCore = nameof(CompileCore);
        public static readonly string PreCompile = nameof(PreCompile);
        public static readonly string PostCompile = nameof(PostCompile);

        public static readonly string Initialize = nameof(Initialize);
        public static readonly string InitializeCore = nameof(InitializeCore);
        public static readonly string PreInitialize = nameof(PreInitialize);
        public static readonly string PostInitialize = nameof(PostInitialize);
    }

    public class StandardLifecycleTaskProvider : ITaskProvider
    {
        public IEnumerable<TaskDefinition> GetTasks()
        {
            yield return CreateStandardLifecycleTask(StandardTasks.Default, StandardTasks.Initialize, StandardTasks.Compile, StandardTasks.Test, StandardTasks.Package, StandardTasks.Publish);
            yield return CreateStandardLifecycleTask(StandardTasks.Clean, StandardTasks.PreClean, StandardTasks.CleanCore, StandardTasks.PostClean);
            yield return CreateStandardLifecycleTask(StandardTasks.Publish, StandardTasks.PrePublish, StandardTasks.PublishCore, StandardTasks.PostPublish);
            yield return CreateStandardLifecycleTask(StandardTasks.Package, StandardTasks.PrePackage, StandardTasks.PackageCore, StandardTasks.PostPackage);
            yield return CreateStandardLifecycleTask(StandardTasks.Test, StandardTasks.PreTest, StandardTasks.TestCore, StandardTasks.PostTest);
            yield return CreateStandardLifecycleTask(StandardTasks.Compile, StandardTasks.PreCompile, StandardTasks.CompileCore, StandardTasks.PostCompile);
            yield return CreateStandardLifecycleTask(StandardTasks.Initialize, StandardTasks.PreInitialize, StandardTasks.InitializeCore, StandardTasks.PostInitialize);
        }

        private TaskDefinition CreateStandardLifecycleTask(string name, params string[] dependencies)
        {
            return new TaskDefinition(name, typeof(StandardLifecycleTaskProvider).FullName, dependencies, runBefore: null, implementation: DefaultImplementation);
        }

        private TaskResult DefaultImplementation(TaskInvocation arg) => new TaskResult(arg.Task, success: true, returnValue: null, exception: null);
    }
}