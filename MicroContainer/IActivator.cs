using System;
using System.Reflection;

namespace MicroContainer
{
	public interface IActivator
	{
		object Activate(
			Type concreteType,
			IContainer container,
			IResolvingContext context
			);

		void SetParameter(
			Func<ParameterInfo, bool> match,
			Func<ParameterInfo, IContainer, object> valueProvider
			);
	}
}
