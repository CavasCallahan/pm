using System;
using System.Collections.Generic;
using app.library;
using app.library.Model;
using app.Store;

namespace app.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        private object currentView;

        public object CurrentView
        {
            get { return Store.CurrentView; }
            set 
            { 
                currentView = value;
                OnPropertyChanged();
            }
        }

        public NavigationStore Store { get; }

        public MainViewModel(NavigationStore store)
        {
            Store = store;

            Store.CurrentViewModelChange += OnCurrentViewModelChange;
        }

        private void OnCurrentViewModelChange()
        {
            OnPropertyChanged(nameof(CurrentView));
        }
    }
}
