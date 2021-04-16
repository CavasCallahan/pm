using System.Collections.Generic;


namespace app.library.Model
{
    public class ProjectModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public List<CommandModel> Commands { get; set; }
    }
}
