using System;

namespace MicroContainer
{
	sealed class ResolverSingletonPerNamedContainer : ResolverSingleton
	{
		readonly string _containerName;

		public ResolverSingletonPerNamedContainer(Type forConcreteType, string forRegistrationName, IActivator activator, string containerName)
			: base(forConcreteType, forRegistrationName, activator)
		{
			if (containerName == null)
				throw new ArgumentNullException("containerName");
			_containerName = containerName;
		}

		protected override IContainer GetOwnerContainer(
			IResolvingContext context, 
			Type concreteType)
		{
			var container = context.CurrentContainer;
			while(container.Parent != null)
			{
				if (container.Name == _containerName)
					return container;
				else
					container = (IContainer)container.Parent;
			}

			throw new DependencyException("Failed to find container named: " + _containerName);
		}
	}
}
