using app.library;
using app.library.Model;
using app.Store;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace app.ViewModel
{
    public class HomeViewModel : ObservableObject
    {
        public DoubleAnimation SlideAnimation { get; set; }

        public UserControl WindowHome { get; set; }
        public DescriptionViewModel DescriptionVM { get; set; }

        public RelayCommand ItemSelectCommand { get; set; }

        public RelayCommand CreateCommand { get; set; }

        private List<ProjectModel> listProjects = new List<ProjectModel>();
        private ProjectManager pm { get; set; } = new ProjectManager();

        private Visibility menu = Visibility.Hidden;

        public Visibility Menu
        {
            get { return menu; }
            set 
            { 
                menu = value;
                OnPropertyChanged();
            }
        }


        private int windowWidth = 800;

        public int WindowWidth
        {
            get { return windowWidth; }
            set 
            { 
                windowWidth = value;
                OnPropertyChanged();
            }
        }

            
        private ProjectModel selectedItem;

        public ProjectModel SelectedItem
        {
            get { return selectedItem; }
            set { selectedItem = value; }
        }


        public List<ProjectModel> ListProjects
        {
            get { return listProjects; }
            set
            {
                listProjects = value;
                OnPropertyChanged();
            }
        }

        public NavigationStore Store { get; }

        public HomeViewModel(NavigationStore store, UserControl windowHome)
        {
            listProjects = pm.GetProjects<ProjectModel>();

            ItemSelectCommand = new RelayCommand(o =>
            {
                store.CurrentView = new DescriptionViewModel(selectedItem);
            });

            CreateCommand = new RelayCommand(o => {
                menu = Visibility.Visible;
            });

            Store = store;
            WindowHome = windowHome;
        }
    }
}
