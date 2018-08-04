using System;

namespace MicroContainer
{
	sealed class ResolverSingletonPerContainer : ResolverSingleton
	{
		public ResolverSingletonPerContainer(Type forConcreteType, string forRegistrationName, IActivator activator) 
			: base(forConcreteType, forRegistrationName, activator)
		{
		}

		protected override IContainer GetOwnerContainer(
			IResolvingContext context, 
			Type concreteType
			)
		{
			return context.CurrentContainer;
		}
	}
}
