using System;
using System.Reflection;

namespace MicroContainer
{
	/// <summary>
	/// Activator that uses a custom lambda method to activate the instance
	/// </summary>
	class ActivatorLambda : IActivator
	{
		readonly Func<IContainer, object> _lambda;

		public ActivatorLambda(Func<IContainer, object> lambda)
		{
			if (null == lambda)
				throw new ArgumentNullException("lambda");
			_lambda = lambda;
		}

		public object Activate(Type concreteType, IContainer container, IResolvingContext context)
		{
			var t = _lambda(container);
			if (t is IDisposable)
				container.AddDisposable((IDisposable)t);
			return t;
		}

		public void SetParameter(
			Func<ParameterInfo, bool> match, 
			Func<ParameterInfo, IContainer, object> valueProvider)
		{
			throw new NotSupportedException("Lambda activator does not support SetParameter()");
		}
	}
}
