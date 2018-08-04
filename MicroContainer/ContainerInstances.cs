using System;
using System.Collections.Generic;
using System.Threading;

namespace MicroContainer
{
	/// <summary>
	/// Contains instances that managed by a single container
	/// </summary>
	sealed class ContainerInstances : IContainerInstances
	{
		readonly Dictionary<Type, object> _instances;
		readonly Dictionary<Type, Dictionary<string, object>> _namedInstances;
		readonly IContainer _parent;
		readonly object _syncRoot = new object();

		public ContainerInstances(IContainer parent)
		{
			if (parent == null)
				throw new ArgumentNullException("parent");
			_parent = parent;
			_instances = new Dictionary<Type, object>();
			_namedInstances = new Dictionary<Type, Dictionary<string, object>>();
		}

		public object GetOrCreateInstance(
			Type concreteType, 
			IActivator activator, 
			IResolvingContext context,
			string registrationName)
		{
			if(!Monitor.TryEnter(_syncRoot, TimeSpan.FromSeconds(5)))
				throw new InvalidOperationException("Potentially deadlock");
			try
			{
				if(HasInstance(concreteType, registrationName))
					return GetInstance(concreteType, registrationName);
				else
				{
					var instance = activator.Activate(concreteType, _parent, context); ;
					PutInstance(concreteType, instance, registrationName);
					return instance;
				}
			}
			finally
			{
				Monitor.Exit(_syncRoot);
			}
	}

		bool HasInstance(Type type, string registrationName = null)
		{
			if (registrationName == null)
				return _instances.ContainsKey(type);
			else
			{
				return _namedInstances.ContainsKey(type)
					&& _namedInstances[type].ContainsKey(registrationName);
			}
		}

		object GetInstance(Type type, string registrationName = null)
		{
			if (registrationName == null)
				return _instances[type];
			else
				return _namedInstances[type][registrationName];
		}

		void PutInstance(Type type, object instance, string registrationName = null)
		{
			if(registrationName == null)
				_instances[type] = instance;
			else
			{
				if (false == _namedInstances.ContainsKey(type))
					_namedInstances[type] = new Dictionary<string, object>();
				_namedInstances[type][registrationName] = instance;
			}
		}
	}
}
