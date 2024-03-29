﻿using System;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ProjectManager.Commands;
using ProjectManager.Helpers;
using System.Diagnostics;
using Microsoft.VisualBasic;
using pm.Helpers;
using ProjectManager.Graphics.Pages;

namespace ProjectManager
{
    class Program
    {
        public static ExtentionHelper Helper { get; set; }

        public delegate IServiceProvider ServiceResolver(string key);

        static void Main(string[] args)
        {
            var root = Directory.GetCurrentDirectory();

            var builder = new ConfigurationBuilder();
            BuildConfig(builder);

            var host = Host.CreateDefaultBuilder()
            .ConfigureServices((Context, services) => {
                services.AddTransient<StartService>();
                services.AddTransient<ProjectHandler>();
                services.AddSingleton<SettingsHandler>();
                services.AddSingleton<ExtentionHelper>();
                
                //Commands
                services.AddTransient<Create>();
                services.AddTransient<Init>();
                services.AddTransient<Open>();
                services.AddTransient<Editor>();
                services.AddTransient<Remove>();
                services.AddTransient<Run>();
                services.AddTransient<Settings>();
                services.AddTransient<Execute>();
                services.AddTransient<GraphInterfaceCommand>();

                //Pages
                services.AddTransient<StartPage>();
            })
            .Build();
            
            var svc = ActivatorUtilities.CreateInstance<StartService>(host.Services);
            svc.Run(args,root);
        }  

        public static string GetBasePath()
        {
            using var processModule = Process.GetCurrentProcess().MainModule;
            return Path.GetDirectoryName(processModule?.FileName);
        }
        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(GetBasePath())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
            .AddEnvironmentVariables();
            FileSystem.ChDir(GetBasePath());
        }

    }
}
