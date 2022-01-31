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
    /// <summary>
    /// Класс для привязки данных о доступных фракталах.
    /// </summary>
    internal class ViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Конструктор, задающий доступные фракталы.
        /// </summary>
        public ViewModel()
        {
            list.Add(new BlownFractalTree());
            list.Add(new KochCurve());
            list.Add(new SerpinskyCarpet());
            list.Add(new SerpinskyTriangle());
            list.Add(new CantorSet());
            availableFractals = new CollectionView(list);
        }

        /// <summary>
        /// Список доступных фракталов.
        /// </summary>
        public IList<Fractal> list = new List<Fractal>();

        /// <summary>
        /// Коллекция доступных фракталов.
        /// </summary>
        private readonly CollectionView availableFractals;

        /// <summary>
        /// Свойство для доступа к коллекции доступных фракталов.
        /// </summary>
        public CollectionView AvailableFractals
        {
            get => availableFractals;
        }

        /// <summary>
        /// Событие для реагирования на изменения свойства.
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// События изменения свойства.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
