namespace Sublist.Providers.Settings
{
    public interface ISettingsProvider
    {
        bool GetShowCompleted();
        void SetShowCompleted(bool value);
    }
}