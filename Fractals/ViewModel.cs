using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using FractalsLibrary;

namespace Fractals
{
    internal class ViewModel : INotifyPropertyChanged
    {
        public ViewModel()
        {
            list.Add(new BlownFractalTree());
            list.Add(new KochCurve());
            list.Add(new SerpinskyCarpet());
            list.Add(new SerpinskyTriangle());
            list.Add(new CantorSet());
            availableFractals = new CollectionView(list);
        }

        public IList<Fractal> list = new List<Fractal>();

        private readonly CollectionView availableFractals;

        public CollectionView AvailableFractals
        {
            get => availableFractals;
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
