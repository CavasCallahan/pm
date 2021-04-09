using System.Collections.Generic;
using app.library;
using app.library.Model;

namespace app.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        private List<ProjectModel> listProjects = new List<ProjectModel>();

        public RelayCommand ItemSelectCommand { get; set; }

        private ProjectManager pm { get; set; } = new ProjectManager();

        public List<ProjectModel> ListProjects
        {
            get { return listProjects; }
            set 
            { 
                listProjects = value;
                OnPropertyChanged();
            }
        }

        private void InitializeProject()
        {
            
        }

        public MainViewModel()
        {
            listProjects = pm.GetProjects<ProjectModel>();

            ItemSelectCommand = new RelayCommand(o => {
                System.Console.WriteLine("Hello World!");
            } );
        }
    }
}
