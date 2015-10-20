using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sublist.Contracts.App;
using Sublist.Data;

namespace Sublist.Providers.Settings
{
    public class SettingsProvider : ISettingsProvider
    {
        private readonly IDataProvider _dataProvider;

        private IAppData _currentAppData;

        public SettingsProvider(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
            _currentAppData = _dataProvider.GetAppData();
        }

        public bool GetShowCompleted()
        {
            _currentAppData = _dataProvider.GetAppData();
            return _currentAppData.ShowCompleted;
        }

        public void SetShowCompleted(bool value)
        {
            _currentAppData.ShowCompleted = value;
            _dataProvider.UpdateAppData(_currentAppData);
        }
    }
}
