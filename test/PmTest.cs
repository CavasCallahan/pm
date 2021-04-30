using Xunit;
using System;
using pm.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.Diagnostics;
using System.IO;

namespace Test
{
    public class DependencySetupFixture
    {
        static string GetBasePath()
        {
            using var processModule = Process.GetCurrentProcess().MainModule;
            return Path.GetDirectoryName(processModule?.FileName);
        }
        void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(GetBasePath())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
            .AddEnvironmentVariables();
            FileSystem.ChDir(GetBasePath());
        }
        public DependencySetupFixture()
        {
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);

            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>();
            services.AddTransient<ProjectHandler>();
            services.AddSingleton<SettingsHandler>();

            ServiceProvider = services.BuildServiceProvider();
        }

        public ServiceProvider ServiceProvider { get; private set; }
    }


    public class ProjectHandlerTest : IClassFixture<DependencySetupFixture>
    {
        private ServiceProvider service;

        public ProjectHandler ProjectHandler { get; set; }

        public ProjectHandlerTest(DependencySetupFixture fixture)
        {
            this.service = fixture.ServiceProvider;
        }

        [Fact]
        public void CreateProject()
        {
            ProjectHandler handler = (ProjectHandler)service.GetService(typeof(ProjectHandler));
            handler.OpenCurrentEditor();
        }
    }
}