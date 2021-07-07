using Microsoft.VisualBasic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.Json;
using pm.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Xml;

namespace pm.Helpers
{
    public enum ProjectType{
        Csharp,
        Python,
        Java,
        Simple,
        Command,
        Plugin
    }

    public class ProjectHandler
    {
        public ProjectHandler(SettingsHandler settings)
        {
            Settings = settings;
        }

        public string TemplatePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),"Template");

        public IConfiguration Configuration { get; }
        private SettingsHandler Settings { get; }

        public SettingsModel GetInfoSettingsPmFile(string projectPath)
        {
            var files = Directory.GetFiles(projectPath);

            foreach (var file in files)
            {   
                var filename = Path.GetFileName(file);

                if (filename == "settings.pm.json")
                {
                    var text = File.ReadAllText(file);
                    var settings = JsonSerializer.Deserialize<SettingsModel>(text);
                    return settings;
                }
            }

           throw new FileNotFoundException();
        }

        public void GetCommandInfoByXmlFile(string projectPath, string nameCommand)
        {
            var path = Directory.GetFiles($"{ projectPath }\\Commands");

            foreach (string command in path)
            {
                if (Path.GetFileName(command) == $"{nameCommand}.xml")
                {
                    var reader = new XmlTextReader(command);

                    while (reader.Read())
                    {   
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Before")
                        {
                            if (!reader.IsEmptyElement)
                            {
                                var i = 0;
                                while(reader.ReadToFollowing("run"))
                                {
                                    i = i + 1;
                                    var run = reader.ReadElementContentAsString();
                                    new MessagesHandler($"Running => Process { i }", MessageType.Information);

                                    var runs = run.Split("|");

                                    FileSystem.ChDir(projectPath);

                                    //Executes the pipeline
                                    Process process = new Process();
                                    process.StartInfo.FileName = runs[0];
                                    process.StartInfo.Arguments = runs[1];
                                    process.Start();
                                    process.WaitForExit();
                                    process.Kill();

                                    if(process.ExitCode > 0)
                                    {
                                        System.Console.WriteLine();
                                        new MessagesHandler($"Process {i} have Fail", MessageType.Error);
                                        return;
                                    }

                                    System.Console.WriteLine();
                                    new MessagesHandler("Passed!", MessageType.Information);
                                }

                                System.Console.WriteLine();
                                new MessagesHandler("Completed!", MessageType.Information);
                            }
                        }
                    }   
                }
            }
        }

        public void OpenXmlFileInProject(string nameCommand)
        {
            var rootPath = Settings.Location;
            GetCommandInfoByXmlFile(rootPath, nameCommand);
            // var settings = GetCommandInfoByXmlFile(rootPath);
            

        }

        //Handler's Of Commands

        #region Creation and Removing Project's
        public void CreateProject(string projectPath, ProjectType type, string command = null)
        {
            switch (type)
            {
                case ProjectType.Csharp:
                    CreateTemplate("CSharp", projectPath);
                break;
                case ProjectType.Python:
                    CreateTemplate("Python", projectPath);
                break;
                case ProjectType.Simple:
                    CreateTemplate(null, projectPath);
                break;
                case ProjectType.Plugin:
                    CreateTemplate("Plugin", projectPath);
                break;
                case ProjectType.Command:
                    CreateCommand(command, projectPath);
                break;
            }
        }
        
        private void CreateCommand(string commandName, string projectPath)
        {
            var commandFolder = $@"{Settings.Location}\Commands";
            System.Console.WriteLine(commandFolder);

            if (File.Exists(commandFolder))
            {
                System.Console.WriteLine("Exits");
            }
        }

        private void CreateTemplate(string Type, string projectPath)
        {
            var template = Settings.GetTemplatePath();
            var root = Settings.GetValue<string>("ProjectPath");

            if (!Directory.Exists($"{ root }/{ projectPath }"))
            { 
                FileSystem.ChDir(root);
                Directory.CreateDirectory(projectPath);

                if (Type == null)
                {
                    FileSystem.ChDir(projectPath);
                    InitializeProject($"{root}/{projectPath}", projectPath);
                    OpenCurrentEditor();
                    return;
                }

                string[] files = Directory.GetFiles($@"{ template }\{ Type }");
            
                foreach (var file in files)
                {
                    var filename = Path.GetFileName(file);
                    var destFile = Path.Combine($@"{ root }\{ projectPath }", filename);
                    File.Copy(file, destFile, true);
                }

                FileSystem.ChDir(projectPath);
                OpenCurrentEditor();
                InitializeProject($"{root}/{projectPath}", projectPath);

                new MessagesHandler($"The project { projectPath } was created! \n Happy hacking :)!", MessageType.Information);
            }
            else
            {
                new MessagesHandler($"The project { projectPath } is already exists!", MessageType.Error);
            }

        }

        public void InitializeProject(string projectPath, string projectName)
        {
            var directoryFolder = Path.GetDirectoryName(projectPath);
            var path = Settings.GetValue<string>("ProjectPath");

            var redirectFile = $"{ Settings.PathToPmDirectory }/redirect.json";

            if(File.Exists($"{ projectPath }/settings.pm.json"))
            {
                new MessagesHandler("The Project is already initialize", MessageType.Normal);
            }
            else
            {
                if (path != $@"{directoryFolder}\")
                {   
                    if (File.Exists(redirectFile))
                    {
                        List<string> redirectList = new List<string>();
                        var jsonString = File.ReadAllText(redirectFile);
                        var redirectObject = JsonSerializer.Deserialize<RedirectModel>(jsonString);

                        //Adds all to the list
                        redirectObject.Redirect.ForEach(d => redirectList.Add(d));
                        

                        //Adds path of file
                        redirectList.Add(projectPath);

                        //Creates object with all built in
                        var data = new RedirectModel{
                            Redirect = redirectList,
                        };

                        JsonSerializerOptions options = new JsonSerializerOptions{
                            WriteIndented = true,
                        };

                        jsonString = JsonSerializer.Serialize<RedirectModel>(data, options);

                        File.WriteAllText(redirectFile, jsonString);
                    }
                }

                using(StreamWriter setings = File.CreateText($"{ projectPath }/settings.pm.json"))
                {
                    var settings = new SettingsModel{
                        ProjectName = projectName,
                        Description = "write the description of the project",
                        Editor = "your editor..."
                    };

                    var options = new JsonSerializerOptions{
                        WriteIndented = true,
                    };

                    var json = JsonSerializer.Serialize(settings, options);
                    setings.Write(json);

                    new MessagesHandler("The project was initialize!", MessageType.Information);
                }
            }
        }

        public ProjectModel[] ListProjects()
        {
            ProjectModel[] result = {};
            List<ProjectModel> list = new List<ProjectModel>();

            var root = Settings.GetValue<string>("ProjectPath");
            var jsonString = File.ReadAllText(Path.Combine(Settings.PathToPmDirectory, "redirect.json"));
            var redirectObject = JsonSerializer.Deserialize<RedirectModel>(jsonString);

            var directories = Directory.GetDirectories(root);

            foreach (var dir in directories)
            {
                var project = new ProjectModel{
                    Title = Path.GetFileName(dir),
                    Path = dir
                };

                var directoryInfo = new DirectoryInfo(project.Path);

                if (!directoryInfo.Attributes.HasFlag(FileAttributes.Hidden))
                {
                    list.Add(project);
                }
            }

            foreach (var dir in redirectObject.Redirect)
            {
                var project = new ProjectModel{
                    Title = Path.GetFileName(dir),
                    Path = dir
                };

                list.Add(project);
            }

            result = list.ToArray();

            return result;
        }

        public void RemoveProject(string projectName)
        {
            var root = Settings.GetValue<string>("ProjectPath");
            
            var jsonString = File.ReadAllText(Path.Combine(Settings.PathToPmDirectory, "redirect.json"));
            var redirectObject = JsonSerializer.Deserialize<RedirectModel>(jsonString);

            var directories = Directory.GetDirectories(root);

            try
            {
                foreach (var dir in directories)
                {
                    var dirName = Path.GetDirectoryName(dir);
                    var folder = @$"{dirName}\{projectName}";
                    if (dir == folder)
                    {
                        var directory = new DirectoryInfo(folder);
                        directory.Delete(true);
                        new MessagesHandler($"{ projectName } was deleted!", MessageType.Information);
                        return;
                    }
                }

                foreach (var dir in redirectObject.Redirect)
                {
                    var dirName = Path.GetDirectoryName(dir);
                    var folder = @$"{dirName}\{projectName}";
                    if (dir == folder)
                    {
                        var directory = new DirectoryInfo(folder);
                        directory.Delete(true);

                        Settings.DeleteFromRedirectFile(folder);

                        new MessagesHandler($"{ projectName } was deleted!", MessageType.Information);
                        return;
                    }
                }

                new MessagesHandler($"Pm didn't find any project with the name { projectName }", MessageType.Normal);
            }
            catch (System.Exception)
            {
                new MessagesHandler("Something went wrong!", MessageType.Error);
            }
        }

        #endregion

        #region Open Project's
        public void OpenProjectHandler(string projectName)
        {
            var root = Settings.GetValue<string>("ProjectPath");

            var directories = Directory.GetDirectories(root);

            try
            {
                foreach (var dir in directories)
                {
                    var dirName = Path.GetDirectoryName(dir);
                    var folder = @$"{dirName}\{projectName}";
                    if (dir == folder)
                    {
                        FileSystem.ChDir(folder);
                        OpenCurrentEditor();
                        return;
                    }
                }

                var jsonString = File.ReadAllText(Path.Combine(Settings.PathToPmDirectory, "redirect.json"));
                var redirectObject = JsonSerializer.Deserialize<RedirectModel>(jsonString);

                foreach (var file in redirectObject.Redirect)
                {
                    var folderName = Path.GetFileName(file);

                    if (folderName == projectName)
                    {
                        FileSystem.ChDir(file);
                        OpenCurrentEditor();
                        return;
                    }
                }

                new MessagesHandler($"Pm didn't find any project with the name { projectName }", MessageType.Normal);
            }
            catch(System.Exception)
            {
                new MessagesHandler("Something went wrong!", MessageType.Error);
            }
        }

        public void OpenCurrentEditor(string path = null, string fileName = null)
        {
            var editor = Settings.GetCurrentEditor<string>();

            if (path != null)
            {
                FileSystem.ChDir(path);
                try
                {
                    Process process = new Process();
                    process.StartInfo.FileName = editor;
                    process.StartInfo.Arguments = fileName;
                    process.Start();
                }
                catch (System.Exception)
                {
                    new MessagesHandler("Something went wrong!", MessageType.Error);
                }

                return;
            }

            try
            {
                Process process = new Process();
                process.StartInfo.FileName = editor;
                process.StartInfo.Arguments = ".";
                process.Start();
            }
            catch (System.Exception)
            {
                new MessagesHandler("Something went wrong!", MessageType.Error);
            }
        }

        #endregion

        #region Editor Releted
        public void ChangeEditor(string editor)
        {
            if(editor.Length > 0)
            {
                try
                {
                    Settings.setValue(editor);
                    new MessagesHandler($"The current editor was change!, for { editor }", MessageType.Information);
                }
                catch (System.Exception)
                {
                    new MessagesHandler("Something went wrong!", MessageType.Error);
                }
            }
            else
            {
                new MessagesHandler("The editor name must have a name", MessageType.Information);
            }
        }

        public void CreateEditor(EditorModel editor)
        {
            Settings.setValue(editorPath: editor);
        }

        public object ListEditors()
        {
            return Settings.ListEditors();
        }
        #endregion
       
        #region ProjectSettings releated
           
        public void RunProjectCommand(string command)
        {
            var root = Settings.Location;
            var settings = Settings.ReadSettings(root);
            
            if (settings.Scripts != null)
            {
                var scripts = settings.Scripts.ToArray();

                foreach (var script in scripts)
                {
                    if(command == script.Name)
                    {
                        var words = script.Command.Split('|');

                        try
                        {   
                            FileSystem.ChDir(root);

                            Process process = new Process();
                            process.StartInfo.FileName = words[0];
                            process.StartInfo.Arguments = words[1];
                            process.Start();
                            process.WaitForExit();
                            process.Kill();
                        }
                        catch (System.Exception)
                        {
                            new MessagesHandler("Something went wrong!", MessageType.Error);
                        }
                    }
                }

                return;
            }

            new MessagesHandler("Pm didn't find any script with that name", MessageType.Normal);
        }

       #endregion
    }
}