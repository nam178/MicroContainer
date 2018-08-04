using System;
using System.Reflection;

namespace MicroContainer
{
	sealed class RegistrationNonGeneric : RegistrationBase, IRegistration
	{
		readonly IActivator _activator;
		readonly IContainer _forContainer;

		public RegistrationNonGeneric(IContainer forContainer, IActivator activator, Type concreteType, string registrationName) 
			: base(new ResolverAlwaysCreateNew(concreteType, registrationName, activator), concreteType, registrationName)
		{
			_forContainer = forContainer;
			_activator = activator;
		}

		public IRegistration As(Type interfaceType)
		{
			_forContainer.AddRegistration(interfaceType, this, Name);
			return this;
		}

		public IRegistration As<TInterface>()
		{
			_forContainer.AddRegistration(typeof(TInterface), this, Name);
			return this;
		}

		public IRegistration InstancePerContainer()
		{
			Resolver = new ResolverSingletonPerContainer(ConcreteType, Name, _activator);
			return this;
		}

		public IRegistration InstancePerNamedContainer(string scopeName)
		{
			Resolver = new ResolverSingletonPerNamedContainer(ConcreteType, Name, _activator, scopeName);
			return this;
		}

		public IRegistration Singleton()
		{
			Resolver = new ResolverSingleton(ConcreteType, Name, _activator);
			return this;
		}

		public IRegistration WithParameter(
			Func<ParameterInfo, bool> match, 
			Func<ParameterInfo, IContainer, object> valueProvider)
		{
			_activator.SetParameter(match, valueProvider);
			return this;
		}
	}
}
