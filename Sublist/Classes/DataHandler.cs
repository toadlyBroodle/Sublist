using System;
using System.Collections.Generic;
using System.Diagnostics;
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

		public FileHelper fileHelper;
		public MasterList<Entry> userDataList;

		public DataHandler(MainPage mp)
		{
			mainPage = mp;

			fileHelper = new FileHelper();

			userDataList = LoadUserData();
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
