using System;

namespace MicroContainer
{
	/// <summary>
	/// A resolver that always create new instance for every resolve request
	/// </summary>
	sealed class ResolverAlwaysCreateNew : ResolverBase, IResolver
	{
		readonly IActivator _activator;

		public ResolverAlwaysCreateNew(Type forConcreteType, string forRegistrationName, IActivator activator)
			: base(forConcreteType, forRegistrationName)
		{
			if (null == activator)
				throw new ArgumentNullException("activator");
			_activator = activator;
		}

		protected override IContainer GetOwnerContainer(
			IResolvingContext context, 
			Type concreteType)
		{
			return context.KickOffContainer;
		}

		protected override object GetInstance(
			IResolvingContext context, 
			Type concreteType, 
			IContainer fromContainer)
		{
			return _activator.Activate(concreteType, fromContainer, context);
		}
	}
}
