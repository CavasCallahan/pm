using app.library.Model;
using System;
using System.Collections.Generic;

namespace app.ViewModel
{
    public class DescriptionViewModel : ObservableObject
    {
        public ProjectModel Project { get; }

        private string name;

        public string Name
        {
            get { return name; }
            set 
            { 
                name = value;
                OnPropertyChanged();
            }
        }

        private string description;

        public string Description
        {
            get { return description; }
            set 
            { 
                description = value;
                OnPropertyChanged();
            }
        }



        public DescriptionViewModel(ProjectModel project)
        {
            Project = project;

            name = Project.Name;
            description = Project.Description;
        }



    }
}
