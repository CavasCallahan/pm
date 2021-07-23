using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using pm.Models;
using pm.Helpers;

namespace pm.Helpers
{
    public class SettingsHandler
    {
        public SettingsHandler(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public IConfiguration Configuration { get; }
        public string PathToPmDirectory
        {
            get { return Path.Combine(GetValue<string>("ProjectPath"), ".pm"); }
        }
        
        public string Location { get; set; }

        public void setValue(string currentEditor = null, object editorPath = null)
        {            
            var jsonString = File.ReadAllText("appsettings.json");
            var file = JsonSerializer.Deserialize<AppSettings>(jsonString);

            var settings = new AppSettings{
                ProjectPath = file.ProjectPath,
                CurrentEditor = currentEditor == null ? file.CurrentEditor : currentEditor,
                EditorPath = editorPath == null ? file.EditorPath : new { editorPath, file.EditorPath }
            };

            var options = new JsonSerializerOptions{
                WriteIndented = true
            };

            var data = JsonSerializer.Serialize<AppSettings>(settings, options);
            File.WriteAllText("appsettings.json",data);
        }
        
        public void writeRedirectFile(string folder)
        {
            var root = Path.Join(PathToPmDirectory, "redirect.json");

            var jsonString = File.ReadAllText(root);
            var file = JsonSerializer.Deserialize<RedirectModel>(jsonString);

            file.Redirect.Add(folder);

            var options = new JsonSerializerOptions{
                WriteIndented = true
            };

            var data = JsonSerializer.Serialize<RedirectModel>(file, options);
            File.WriteAllText(root, data);   
        }

         public void DeleteFromRedirectFile(string folder)
        {
            var root = Path.Join(PathToPmDirectory, "redirect.json");

            var jsonString = File.ReadAllText(root);
            var file = JsonSerializer.Deserialize<RedirectModel>(jsonString);

            file.Redirect.Remove(folder);

            var options = new JsonSerializerOptions{
                WriteIndented = true
            };

            var data = JsonSerializer.Serialize<RedirectModel>(file, options);
            File.WriteAllText(root, data);   
        }

        public object ListEditors()
        {
            var jsonString = File.ReadAllText("appsettings.json");
            var file = JsonSerializer.Deserialize<AppSettings>(jsonString);

            System.Console.WriteLine(file.EditorPath);
            return file.EditorPath;
        }

        public T GetValue<T>(string value)
        {
            var response = Configuration.GetValue<T>(value);

            return response;
        }
        public T GetCurrentEditor<T>()
        {
            var currentEditor = GetValue<string>("CurrentEditor");
            var editor = Configuration.GetValue<T>($"EditorPath:{ currentEditor }");
            
            return editor;
        }

        public string GetTemplatePath()
        {
            var baseDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            return $@"{ baseDirectory }\Templates\";
        }

        public string GetExtentionPath()
        {
            var baseDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            return $@"{ baseDirectory }\Extentions\";
        }

        public SettingsModel ReadProjectSettings(string rootPath)
        {
            var jsonString = File.ReadAllText($@"{rootPath}\settings.pm.json");
            var file = JsonSerializer.Deserialize<SettingsModel>(jsonString);

            return file;
        }

        public string GetSettingsPath()
        {
            var path = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\appsettings.json";
            return path;
        }
    }
}