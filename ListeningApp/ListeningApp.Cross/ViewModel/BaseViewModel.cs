using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ListeningApp.ViewModel
{
    public class BaseViewModel : ObservableObject, INotifyPropertyChanged
    {
        string title = string.Empty;

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        string subTitle = string.Empty;

        /// <summary>
        /// Gets or sets the subtitle.
        /// </summary>
        /// <value>The subtitle.</value>
        public string Subtitle
        {
            get { return subTitle; }
            set { SetProperty(ref subTitle, value); }
        }

        string icon = string.Empty;

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        /// <value>The icon.</value>
        public string Icon
        {
            get { return icon; }
            set { SetProperty(ref icon, value); }
        }

        bool isBusy;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is busy.
        /// </summary>
        /// <value><c>true</c> if this instance is busy; otherwise, <c>false</c>.</value>
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                if (SetProperty(ref isBusy, value))
                    IsNotBusy = !isBusy;
            }
        }

        bool isNotBusy = true;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is not busy.
        /// </summary>
        /// <value><c>true</c> if this instance is not busy; otherwise, <c>false</c>.</value>
        public bool IsNotBusy
        {
            get { return isNotBusy; }
            private set { SetProperty(ref isNotBusy, value); }
        }

        bool canLoadMore = true;

        /// <summary>
        /// Gets or sets a value indicating whether this instance can load more.
        /// </summary>
        /// <value><c>true</c> if this instance can load more; otherwise, <c>false</c>.</value>
        public bool CanLoadMore
        {
            get { return canLoadMore; }
            set { SetProperty(ref canLoadMore, value); }
        }

        #region [ INotifyPropertyChanged ]

        protected bool SetProperty<T>(
            ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion [ INotifyPropertyChanged ]

    }

    public class ObservableObject : INotifyPropertyChanged
    {
        /// <summary>
        /// Sets the property.
        /// </summary>
        /// <returns><c>true</c>, if property was set, <c>false</c> otherwise.</returns>
        /// <param name="backingStore">Backing store.</param>
        /// <param name="value">Value.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="onChanged">On changed.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        protected bool SetProperty<T>(
            ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Occurs when property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Raises the property changed event.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        protected void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
