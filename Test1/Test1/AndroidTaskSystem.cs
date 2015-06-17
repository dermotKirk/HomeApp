using Xamarin.UITest;
using System.Linq;
using System;
using Xamarin.UITest.Queries;

namespace Test1
{
	public class AndroidTaskSystem : ITaskSystem
	{
		IApp app;

		public AndroidTaskSystem(IApp app)
		{
			this.app = app;
		}

		public ITaskSystem Add ()
		{
			app.Tap (c => c.Marked ("Add Task"));
			return this;
		}

        public ITaskSystem Delete(string name)
        {
           	app.Tap(c => c.Marked(name));
            app.WaitForElement(c => c.Marked("Delete"), "Time out", TimeSpan.FromSeconds(3));
           	app.Tap(c => c.Marked("Delete"));
           	app.WaitForNoElement(c => c.Marked(name), "Timed out", TimeSpan.FromSeconds(3));

            return this;
        }

		public ITaskSystem SetName (string name)
		{
			app.EnterText (c => c.Id ("txtName"), name);
			return this;
		}

		public ITaskSystem SetNotes (string notes)
		{
			app.EnterText (c => c.Id("txtNotes"), notes);
			return this;
		}

		public ITaskSystem Save ()
		{
			app.Tap (c => c.Marked ("Save"));
			return this;
		}

		public ITaskSystem Cancel ()
		{
			app.Back ();
			return this;
		}

		public bool HasItem(string itemName)
		{
            return app.Query (c => c.Marked (itemName)).Any();
		}
    }

    public static class UITestHelpers
    {
        public static void WaitThenEnterText(this IApp app, Func<AppQuery, AppQuery> lambda, string text)
        {
            app.WaitForElement(lambda);
            app.EnterText(lambda, text);
        }
    }
}
