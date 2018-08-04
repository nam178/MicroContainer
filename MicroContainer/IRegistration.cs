using System;
using System.Reflection;

namespace MicroContainer
{
	public interface IRegistration<TConcrete> : IRegistrationReadOnly
	{
		IRegistration<TConcrete> As(Type interfaceType);

		IRegistration<TConcrete> As<TInterface>();

		IRegistration<TConcrete> Singleton();

		IRegistration<TConcrete> InstancePerContainer();

		IRegistration<TConcrete> InstancePerNamedContainer(string scopeName);

		IRegistration<TConcrete> WithParameter(
			Func<ParameterInfo, bool> match, 
			Func<ParameterInfo, IContainer, object> valueProvider
			);
	}

	public interface IRegistration : IRegistrationReadOnly
	{
		IRegistration As(Type interfaceType);

		IRegistration As<TInterface>();

		IRegistration Singleton();

		IRegistration InstancePerContainer();

		IRegistration InstancePerNamedContainer(string scopeName);

		IRegistration WithParameter(
			Func<ParameterInfo, bool> match,
			Func<ParameterInfo, IContainer, object> valueProvider
			);
	}
}
