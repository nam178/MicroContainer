using System;

namespace MicroContainer
{
	class ResolverSingleton : ResolverBase
	{
		readonly IActivator _activator;

		public ResolverSingleton(Type forConcreteType, string forRegistrationName, IActivator activator)
			: base(forConcreteType, forRegistrationName)
		{
			if (null == activator)
				throw new ArgumentNullException("activator");
			_activator = activator;
		}

		protected override IContainer GetOwnerContainer(
			IResolvingContext context, 
			Type concreteType
			)
		{
			// Notes
			// Singleton are stored in the root container
			var container = context.KickOffContainer;
			while (container.Parent != null)
				container = (IContainer)container.Parent;
			return container;
		}

		protected override object GetInstance(
			IResolvingContext context, 
			Type concreteType, 
			IContainer container)
		{
			return container.Instances.GetOrCreateInstance(
				concreteType,  
				_activator, 
				context, 
				_registrationName);
		}
	}
}
