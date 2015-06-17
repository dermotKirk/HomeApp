using Xamarin.UITest;

namespace Test1
{
	public interface IMyHomeAppSystem
	{
		IMyHomeAppSystem Add();
        IMyHomeAppSystem Delete(string name);

		IMyHomeAppSystem SetName(string name);
		IMyHomeAppSystem SetNotes (string notes);
		IMyHomeAppSystem Save();
		IMyHomeAppSystem Cancel();

		bool HasItem(string itemName);
	}
}

