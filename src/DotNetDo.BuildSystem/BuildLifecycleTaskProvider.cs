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

    public static class BuildLifecycle
    {
        public const string Build = nameof(Build);
        public const string Rebuild = nameof(Rebuild);

        public const string Clean = nameof(Clean);
        public const string CleanCore = nameof(CleanCore);
        public const string PreClean = nameof(PreClean);
        public const string PostClean = nameof(PostClean);

        public const string Publish = nameof(Publish);
        public const string PublishCore = nameof(PublishCore);
        public const string PrePublish = nameof(PrePublish);
        public const string PostPublish = nameof(PostPublish);

        public const string Package = nameof(Package);
        public const string PackageCore = nameof(PackageCore);
        public const string PrePackage = nameof(PrePackage);
        public const string PostPackage = nameof(PostPackage);

        public const string Test = nameof(Test);
        public const string TestCore = nameof(TestCore);
        public const string PreTest = nameof(PreTest);
        public const string PostTest = nameof(PostTest);

        public const string Compile = nameof(Compile);
        public const string CompileCore = nameof(CompileCore);
        public const string PreCompile = nameof(PreCompile);
        public const string PostCompile = nameof(PostCompile);

        public const string Initialize = nameof(Initialize);
        public const string InitializeCore = nameof(InitializeCore);
        public const string PreInitialize = nameof(PreInitialize);
        public const string PostInitialize = nameof(PostInitialize);
    }

    public class BuildLifecycleTaskProvider : ITaskProvider
    {
        public IEnumerable<TaskDefinition> GetTasks()
        {
            yield return CreateStandardLifecycleTask("Default", BuildLifecycle.Build);
            yield return CreateStandardLifecycleTask(BuildLifecycle.Rebuild, BuildLifecycle.Clean, BuildLifecycle.Build);
            yield return CreateStandardLifecycleTask(BuildLifecycle.Build, BuildLifecycle.Initialize, BuildLifecycle.Compile, BuildLifecycle.Test, BuildLifecycle.Package, BuildLifecycle.Publish);

            yield return CreateStandardLifecycleTask(BuildLifecycle.Clean, BuildLifecycle.PreClean, BuildLifecycle.CleanCore, BuildLifecycle.PostClean);
            yield return CreateStandardLifecycleTask(BuildLifecycle.PreClean);
            yield return CreateStandardLifecycleTask(BuildLifecycle.CleanCore);
            yield return CreateStandardLifecycleTask(BuildLifecycle.PostClean);

            yield return CreateStandardLifecycleTask(BuildLifecycle.Publish, BuildLifecycle.PrePublish, BuildLifecycle.PublishCore, BuildLifecycle.PostPublish);
            yield return CreateStandardLifecycleTask(BuildLifecycle.PrePublish);
            yield return CreateStandardLifecycleTask(BuildLifecycle.PublishCore);
            yield return CreateStandardLifecycleTask(BuildLifecycle.PostPublish);

            yield return CreateStandardLifecycleTask(BuildLifecycle.Package, BuildLifecycle.PrePackage, BuildLifecycle.PackageCore, BuildLifecycle.PostPackage);
            yield return CreateStandardLifecycleTask(BuildLifecycle.PrePackage);
            yield return CreateStandardLifecycleTask(BuildLifecycle.PackageCore);
            yield return CreateStandardLifecycleTask(BuildLifecycle.PostPackage);

            yield return CreateStandardLifecycleTask(BuildLifecycle.Test, BuildLifecycle.PreTest, BuildLifecycle.TestCore, BuildLifecycle.PostTest);
            yield return CreateStandardLifecycleTask(BuildLifecycle.PreTest);
            yield return CreateStandardLifecycleTask(BuildLifecycle.TestCore);
            yield return CreateStandardLifecycleTask(BuildLifecycle.PostTest);

            yield return CreateStandardLifecycleTask(BuildLifecycle.Compile, BuildLifecycle.PreCompile, BuildLifecycle.CompileCore, BuildLifecycle.PostCompile);
            yield return CreateStandardLifecycleTask(BuildLifecycle.PreCompile);
            yield return CreateStandardLifecycleTask(BuildLifecycle.CompileCore);
            yield return CreateStandardLifecycleTask(BuildLifecycle.PostCompile);

            yield return CreateStandardLifecycleTask(BuildLifecycle.Initialize, BuildLifecycle.PreInitialize, BuildLifecycle.InitializeCore, BuildLifecycle.PostInitialize);
            yield return CreateStandardLifecycleTask(BuildLifecycle.PreInitialize);
            yield return CreateStandardLifecycleTask(BuildLifecycle.InitializeCore);
            yield return CreateStandardLifecycleTask(BuildLifecycle.PostInitialize);
        }

        private TaskDefinition CreateStandardLifecycleTask(string name, params string[] dependencies)
        {
            return new TaskDefinition(name, typeof(BuildLifecycleTaskProvider).FullName, dependencies, runBefore: null, implementation: null);
        }
    }
}