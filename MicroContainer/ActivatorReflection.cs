using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MicroContainer
{
	/// <summary>
	/// An activator that uses reflection to construct the instance,
	/// also, uses the current container to resolve its dependencies.
	/// </summary>
	class ActivatorReflection : IActivator
	{
		readonly List<ParameterResolver> _parameterResolvers = new List<ParameterResolver>();

		class ParameterResolver
		{
			public Func<ParameterInfo, bool> Match
			{ get; set; }

			public Func<ParameterInfo, IContainer, object> ValueProvider
			{ get; set; }
		}

		public object Activate(
			Type concreteType,
			IContainer container,
			IResolvingContext context)
		{
			// Freshly construct the instance
			var constructor = concreteType.GetConstructors().First();
			var resolvedParameters = constructor
				.GetParameters()
				.Select(parameterInfo => ProvideParameterValue(container, context, parameterInfo))
				.ToArray();
			var inst = constructor.Invoke(resolvedParameters);
			if(inst is IDisposable)
				container.AddDisposable((IDisposable)inst);
			return inst;
		}

		public void SetParameter(
			Func<ParameterInfo, bool> match, 
			Func<ParameterInfo, IContainer, object> valueProvider)
		{
			_parameterResolvers.Add(new ParameterResolver
			{
				Match = match,
				ValueProvider = valueProvider
			});
		}

		object ProvideParameterValue(
			IContainer container,
			IResolvingContext context,
			ParameterInfo parameterInfo)
		{
			// Search to find the first value provider that match this parameter
			for (var i = 0; i < _parameterResolvers.Count; i++)
			{
				if (_parameterResolvers[i].Match(parameterInfo))
				{
					var inst = _parameterResolvers[i].ValueProvider(parameterInfo, container);
					if(inst is IDisposable)
						container.AddDisposable((IDisposable)container);
					return inst;
				}
			}

			// Use the container to resolve the parameter by default
			return container.Resolve(parameterInfo.ParameterType, context);
		}
	}
}
