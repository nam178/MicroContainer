using System;
using System.Collections.Generic;
using System.Text;

namespace MicroContainer
{
	public class DependencyException : Exception
	{
		public DependencyException(string message, IEnumerable<Type> resolvingStack)
			: this(message + "\nResolving stack:\n" + StackToString(resolvingStack))
		{

		}

		static string StackToString(IEnumerable<Type> resolvingStack)
		{
			var t = new StringBuilder();
			
			foreach(var type in resolvingStack)
			{
				t.Append("> " + type.FullName + "\n");
			}

			return t.ToString();
		}

		public DependencyException(string message)
			: base(message)
		{
		}
	}
}