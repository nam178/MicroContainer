using System;
using System.Reflection;

namespace MicroContainer
{
	sealed class RegistrationGeneric<TConcrete> : RegistrationBase, IRegistration<TConcrete>
	{
		readonly IContainer _forContainer;
		readonly IActivator _activator;

		public RegistrationGeneric(IContainer forContainer, IActivator activator, string registrationName)
			: base(new ResolverAlwaysCreateNew(typeof(TConcrete), registrationName, activator), typeof(TConcrete), registrationName)
		{
			_forContainer = forContainer;
			_activator = activator;
		}

		public IRegistration<TConcrete> As(Type interfaceType)
		{
			_forContainer.AddRegistration(interfaceType, this, Name);
			return this;
		}

		public IRegistration<TConcrete> As<TInterface>()
		{
			_forContainer.AddRegistration(typeof(TInterface), this, Name);
			return this;
		}

		public IRegistration<TConcrete> InstancePerContainer()
		{
			Resolver = new ResolverSingletonPerContainer(typeof(TConcrete), Name, _activator);
			return this;
		}

		public IRegistration<TConcrete> Singleton()
		{
			Resolver = new ResolverSingleton(typeof(TConcrete), Name, _activator);
			return this;
		}

		public IRegistration<TConcrete> InstancePerNamedContainer(string containerName)
		{
			Resolver = new ResolverSingletonPerNamedContainer(typeof(TConcrete), Name, _activator, containerName);
			return this;
		}

		public IRegistration<TConcrete> WithParameter(
			Func<ParameterInfo, bool> match, 
			Func<ParameterInfo, IContainer, object> valueProvider)
		{
			_activator.SetParameter(match, valueProvider);
			return this;
		}
	}
}