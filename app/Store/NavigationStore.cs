using app.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace app.Store
{
    public class NavigationStore
    {
        public event Action CurrentViewModelChange;

        public DoubleAnimation PageTransition { get; set; } = new DoubleAnimation();

        private object currentView;

        public object CurrentView
        {
            get => currentView;
            set
            {
                currentView = value;
                OnCurrentViewModelChange();
            }
        }

        private void OnCurrentViewModelChange()
        {
            CurrentViewModelChange?.Invoke();
        }

    }
}
