
using app.library.Model;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace app.library
{
    public class ProjectManager
    {

        private List<ProjectModel> ProjectList { get; set; } = new List<ProjectModel>();
        public List<ProjectModel> GetProjects<T>()
        {
            var projectPaths = Directory.GetDirectories(@"C:\Users\diogo\Documents\Projects\");

            foreach (var project in projectPaths)
            {
                var projectName = Path.GetFileName(project);
                 
                var settingsFile = $@"{ project }\settings.pm.json";

                if (File.Exists(settingsFile))
                {
                    var jsonString = File.ReadAllText(settingsFile);
                    var file = JsonSerializer.Deserialize<SettingsModel>(jsonString);

                    var data = new ProjectModel
                    {
                        Name = file.ProjectName,
                        Description = file.Description
                    };

                    ProjectList.Add(data);
                }
            }

            return ProjectList;
        }

    }
}
