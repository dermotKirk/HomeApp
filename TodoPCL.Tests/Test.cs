using System;
using NUnit.Framework;
using Todo;

namespace TodoPCL.Tests
{
	[TestFixture ()]
	public class ToDoItemTest
	{
		[Test ()]
		public void TodoItemDefaultsToNone ()
		{
			var todo = new TodoItem ();
			Assert.IsFalse (todo.Done);
		}
	}
}

