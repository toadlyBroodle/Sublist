using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Template10.Services.FileService;
using Template10.Services.SettingsService;
using Windows.Storage;

namespace Sublist.Classes
{
	public class DataHandler
	{
		const string TAG = "DataHandler: ";
		const string FILE_USER_DATA = "userData.txt";
		const string SHOW_COMPLETED = "showCompleted";

		MainPage mainPage;

		public SettingsHelper settingsHelper;
		public FileHelper fileHelper;
		public MasterList<Entry> userDataList;

		public DataHandler(MainPage mp)
		{
			mainPage = mp;

			settingsHelper = new SettingsHelper();
			fileHelper = new FileHelper();

			LoadUserSettings();

			Task.Run(() =>
			{
				userDataList = LoadUserData();
			});
			Task.WaitAll();
		}

		public void SaveUserSettings()
		{
			settingsHelper.Write<bool>(SHOW_COMPLETED, (bool)mainPage.appBarShowCompl.IsChecked);
			//Debug.WriteLine(TAG + "Settings saved.");
		}

		public void LoadUserSettings()
		{
			mainPage.appBarShowCompl.IsChecked = settingsHelper.Read<bool>(SHOW_COMPLETED, false);
			//Debug.WriteLine(TAG + "Settings loaded.");
		}

		public async void SaveUserData()
		{
			userDataList = MainPage.masterList;

			// save to file, if possible, i.e. it's not currently in use
			try	{ await fileHelper.WriteFileAsync(FILE_USER_DATA, userDataList); }
			catch (Exception ex) { Debug.WriteLine(TAG, ex.Message); }

			//Debug.WriteLine(TAG, "User data saved.");
		}

		public MasterList<Entry> LoadUserData()
		{
			MasterList<Entry> returnList = fileHelper.ReadFileAsync<MasterList<Entry>>(FILE_USER_DATA).Result;

			//Debug.WriteLine(TAG, "User data loaded.");

			return returnList;
		}

	}

}
