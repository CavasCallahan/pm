using System.Collections.Generic;
using System.Text.Json;

namespace pm.Models
{
    public class SettingsModel
    {
        public string ProjectName { get; set; }
        public string Editor { get; set; }
        public string Description { get; set; }
        public List<Script> Scripts { get; set; }

        public override string ToString() => JsonSerializer.Serialize<SettingsModel>(this);

    }
}