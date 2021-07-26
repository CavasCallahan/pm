using Microsoft.VisualBasic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.Json;
using pm.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace pm.Helpers
{
    public class Book //Eliminate this
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }

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

        private IConfiguration Configuration { get; }
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

            return null;
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
                                    MessagesHandler.Message($"Running => Process { i }", MessageType.Information);

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
                                        MessagesHandler.Message($"Process {i} have Fail", MessageType.Error);
                                        return;
                                    }

                                    System.Console.WriteLine();
                                    MessagesHandler.Message("Passed!", MessageType.Information);
                                }

                                System.Console.WriteLine();
                                MessagesHandler.Message("Completed!", MessageType.Information);
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
            
            return;
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
            var commandFolder = Path.Join(Settings.Location, ".pmt", "commands");

            if (!File.Exists(commandFolder))
            {
                Directory.CreateDirectory(commandFolder);
                FileSystem.ChDir(commandFolder);

                //Create the XML file
                XmlSerializer serializer = new XmlSerializer(typeof(ExternalCommand));
                var book = new ExternalCommand { Before = new Before{ run = "dotnet|test" } }; // create's the file structure
                serializer.Serialize(File.Create($"{commandName}.xml"), book); // create's the final file
            }
        }

        private void CreateTemplate(string Type, string projectPath)
        {
            var template = Settings.GetTemplatePath();
            var root = Settings.GetValue<string>("ProjectPath");

            if (!Directory.Exists(Path.Join(root, projectPath)))
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

                MessagesHandler.Message($"The project { projectPath } was created! \n Happy hacking :)!", MessageType.Information);
            }
            else
            {
                MessagesHandler.Message($"The project { projectPath } already exists!", MessageType.Error);
            }

        }

        public void InitializeProject(string projectPath, string projectName)
        {
            var directoryFolder = Path.GetDirectoryName(projectPath);
            var path = Settings.GetValue<string>("ProjectPath");
            var current_editor = Settings.GetValue<string>("CurrentEditor");


            var redirectFile = $"{ Settings.PathToPmDirectory }/redirect.json";

            if(File.Exists(Path.Join(projectPath, ".pmt", "settings.pm.json")))
            {
                MessagesHandler.Message("The Project is already initialize", MessageType.Normal);
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

                DirectoryInfo pmtFolder = new DirectoryInfo(Path.Join(projectPath, ".pmt"));
                pmtFolder.Create(); // Creates the .pmt folder holder

                using(StreamWriter settings_file = File.CreateText(Path.Join(projectPath, ".pmt", "settings.pm.json")))
                {
                    var settings = new SettingsModel{
                        ProjectName = projectName,
                        Editor = current_editor,
                        Description = "My project power by pmt",
                        Scripts = new List<Script>()
                    };

                    var options = new JsonSerializerOptions{
                        WriteIndented = true,
                    };

                    var json = JsonSerializer.Serialize(settings, options);
                    settings_file.Write(json);

                    MessagesHandler.Message("The project was initialize!", MessageType.Information);
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
                //Read settings.pm.json file to have acess
                var pmt_folder = Path.Join(dir, ".pmt");
                
                if (Directory.Exists(pmt_folder))
                {
                    var settings = GetInfoSettingsPmFile(pmt_folder);
                    var editor_of_project = GetTheProjectEditor(pmt_folder);

                    var project = new ProjectModel{
                    Title = Path.GetFileName(dir),
                    Path = dir,
                    Description = settings == null ? "This Project has no Description" : settings.Description,
                    Editor = editor_of_project.editor_name
                    };

                    var directoryInfo = new DirectoryInfo(project.Path);

                    if (!directoryInfo.Attributes.HasFlag(FileAttributes.Hidden))
                    {
                        list.Add(project);
                    }
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
                        MessagesHandler.Message($"{ projectName } was deleted!", MessageType.Information);
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

                        MessagesHandler.Message($"{ projectName } was deleted!", MessageType.Information);
                        return;
                    }
                }

                MessagesHandler.Message($"Pm didn't find any project with the name { projectName }", MessageType.Normal);
            }
            catch (System.Exception)
            {
                MessagesHandler.Message("Something went wrong!", MessageType.Error);
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

                MessagesHandler.Message($"Pm didn't find any project with the name { projectName }", MessageType.Normal);
            }
            catch(System.Exception)
            {
                MessagesHandler.Message("Something went wrong!", MessageType.Error);
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
                    process.WaitForExit();
                    process.Kill();
                }
                catch (System.Exception)
                {
                    MessagesHandler.Message("Something went wrong!", MessageType.Error);
                }

                return;
            }

            try
            {
                Process process = new Process();
                process.StartInfo.FileName = editor;
                process.StartInfo.Arguments = ".";
                process.Start();
                process.WaitForExit();
                process.Kill();
            }
            catch (System.Exception)
            {
                MessagesHandler.Message("Something went wrong!", MessageType.Error);
            }
        }

        #endregion

        #region Editor Releted

        public ( string editor_name, string path ) GetTheProjectEditor(string project_path)
        {
            var project_settings = GetInfoSettingsPmFile(project_path);

            if (project_settings != null && project_settings.Editor?.Length > 0)
            {
                var editor_path = Settings.GetValue<string>($"EditorPath: {project_settings.Editor}");

                return (project_settings.Editor, editor_path);
            } 

            return ("Has no Editor","---");
        }
        public void ChangeEditor(string editor)
        {
            if(editor.Length > 0)
            {
                try
                {
                    Settings.setValue(editor);
                    MessagesHandler.Message($"The current editor was change!, for { editor }", MessageType.Information);
                }
                catch (System.Exception)
                {
                    MessagesHandler.Message("Something went wrong!", MessageType.Error);
                }
            }
            else
            {
                MessagesHandler.Message("The editor name must have a name", MessageType.Information);
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
            var settings = Settings.ReadProjectSettings(root);
            
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
                            MessagesHandler.Message("Something went wrong!", MessageType.Error);
                        }
                    }
                }

                return;
            }

            MessagesHandler.Message("Pm didn't find any script with that name", MessageType.Normal);
        }

       #endregion
    }
}