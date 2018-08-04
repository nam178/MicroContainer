using System;

namespace MicroContainer
{
	abstract class ResolverBase : IResolver
	{
		readonly protected Type _concreteType;
		readonly protected string _registrationName;

		public ResolverBase(
			Type forConcreteType, 
			string forRegistrationName)
		{
			if (forConcreteType == null)
				throw new ArgumentNullException("forConcreteType");

			_concreteType = forConcreteType;
			_registrationName = forRegistrationName;
		}

		object IResolver.Resolve(IResolvingContext context)
		{
			// Validate
			if (context.ResolvingStack.Contains(_concreteType))
				throw new CircularDependencyException(_concreteType);

			// To avoid having circular dependency,
			// we keep track the stack
			context.ResolvingStack.Add(_concreteType);
			try
			{
				return ResolveImpl(context);
			}
			finally { context.ResolvingStack.Remove(_concreteType); }
		}

		object ResolveImpl(IResolvingContext context)
		{
			// Grab the container
			var container = GetOwnerContainer(context, _concreteType);
			var instance = GetInstance(context, _concreteType, container);

			// return it
			return instance;
		}

		protected abstract IContainer GetOwnerContainer(
			IResolvingContext context, 
			Type concreteType
			);

		protected abstract object GetInstance(
			IResolvingContext context,
			Type concreteType,
			IContainer fromContainer
			);
	}
}