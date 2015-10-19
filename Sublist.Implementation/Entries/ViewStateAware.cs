using Sublist.Common.Useful;

namespace Sublist.Implementation.Entries
{
    public class ViewStateAware : PropertyChangedBase
    {
        private bool _isVisible;

        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;
                OnPropertyChanged();
            }
        }
    }
}