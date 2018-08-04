using System;

namespace MicroContainer
{
	public class CircularDependencyException : DependencyException
	{
		public CircularDependencyException(Type concreteType)
			: base("Circular dependency: " + concreteType.ToString())
		{
		}
	}
}