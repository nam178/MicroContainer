using System;

namespace MicroContainer
{
	public interface IContainerInstances
	{
		object GetOrCreateInstance(
			Type concreteType,
			IActivator activator,
			IResolvingContext context,
			string registrationName
			);
	}
}