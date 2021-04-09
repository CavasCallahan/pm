using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.library.Model
{
    public class SettingsModel
    {
        public string ProjectName { get; set; }

        public string Editor { get; set; }

        public string Description { get; set; }

        public object Scripts { get; set; }
    }
}
