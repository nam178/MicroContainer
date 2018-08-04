using System;

namespace MicroContainer
{
	public interface IContainer : IDisposable
	{
		IContainer Parent
		{ get; }

		IContainerInstances Instances
		{ get; }

		string Name
		{ get; }

		void AddRegistration(
			Type forInterfaceType, 
			IRegistrationReadOnly registration, 
			string registrationName = null
			);

		object Resolve(
			Type type, 
			string registrationName = null
			);

		object Resolve(
			Type type,
			IResolvingContext context,
			string registrationName = null
			);

		IContainer Create(string childContainerName = null);

		void AddDisposable(IDisposable disposable);
	}
}