using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace ChessGame
{
    public class TestNotifyPropertyChanged : INotifyPropertyChanged
    {
        private string text;

        public string Text
        {
            set
            {
                text = value;
                OnPropertyChanged("Text");
            }
            get
            {
                return text;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var propertyChanged = this.PropertyChanged;
            if(propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
