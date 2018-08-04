using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroContainer
{
	sealed class ContainerImpl : IContainer
	{
		readonly Dictionary<Type, IRegistrationReadOnly> _registrations;
		readonly Dictionary<Type, Dictionary<string, IRegistrationReadOnly>> _namedRegistration;
		readonly ContainerImpl _parent;
		readonly string _name;
		readonly List<IDisposable> _disposables = new List<IDisposable>();
		readonly ContainerInstances _instances;
		readonly object _syncRoot = new object();

		public string Name
		{ get { return _name; } }

		public IContainer Parent
		{ get { return _parent; } }

		public IContainerInstances Instances
		{ get { return _instances; } }

		private ContainerImpl(string name, ContainerImpl parent)
			: this(name)
		{
			if (null == parent)
				throw new ArgumentNullException("parent");
			_parent = parent;
		}

		public ContainerImpl(string name)
		{
			_name = name;
			_registrations = new Dictionary<Type, IRegistrationReadOnly>();
			_namedRegistration = new Dictionary<Type, Dictionary<string, IRegistrationReadOnly>>();
			_instances = new ContainerInstances(this);
		}

		public object Resolve(Type type, string registrationName = null)
		{
			return Resolve(type, null, registrationName);
		}

		public object Resolve(Type type, IResolvingContext context, string registrationName = null)
		{
			ThrowIfDisposed();

			var registration = FindRegistration(type, registrationName);
			context = null == context 
				? new ResolvingContext { KickOffContainer = this } 
				: context;
			context.CurrentContainer = this;
			if (null == registration)
				throw new DependencyException(
					"Unregistered type: " + type.ToString() + "; registrationName=" + registrationName, 
					context.ResolvingStack);
			else
				return registration.Resolver.Resolve(context);
		}

		public IContainer Create(string childContainerName = null)
		{
			IContainer result;
			lock(_syncRoot)
			{
				ThrowIfDisposed();
				result = new ContainerImpl(childContainerName, this);
				_disposables.Add(result);
			}
			return result;
		}

		public void AddDisposable(IDisposable disposable)
		{
			if (null == disposable)
				throw new ArgumentNullException("disposable");
			lock(_syncRoot)
			{
				ThrowIfDisposed();
				if(false == _disposables.Contains(disposable))
					_disposables.Add(disposable);
			}
		}

		public void AddRegistration(
			Type forInterfaceType, 
			IRegistrationReadOnly registration, 
			string registrationName = null)
		{
			lock (_syncRoot)
			{
				ThrowIfDisposed();
				// Name isn't specified?
				if(registrationName == null)
					_registrations[forInterfaceType] = registration;
				// Name specified? Add registration to the named dictionary.
				else
				{
					if(false == _namedRegistration.ContainsKey(forInterfaceType))
						_namedRegistration[forInterfaceType] = new Dictionary<string, IRegistrationReadOnly>();
					_namedRegistration[forInterfaceType][registrationName] = registration;
				}
			}
		}

		IRegistrationReadOnly FindRegistration(Type type, string name)
		{
			var result = (IRegistrationReadOnly)null;
			lock(_syncRoot)
			{
				if (name == null && _registrations.ContainsKey(type))
					result = _registrations[type];
				else if (name != null 
					&& _namedRegistration.ContainsKey(type) 
					&& _namedRegistration[type].ContainsKey(name))
				{
					result = _namedRegistration[type][name];
				}
			}

			if(null == result && _parent != null)
				return _parent.FindRegistration(type, name);

			return result;
		}

		void ThrowIfDisposed()
		{
			if (_isDisposed)
				throw new ObjectDisposedException(GetType().Name);
		}

		bool _isDisposed;
		public void Dispose()
		{
			lock(_syncRoot)
			{
				if (_isDisposed)
					return;
				_isDisposed = true;
			}
			
			// Dispose the disposables
			List<IDisposable> tobeDisposed;
			lock(_syncRoot)
			{
				tobeDisposed = _disposables.ToList();
			}
			for(var i = 0; i < tobeDisposed.Count; i++)
				tobeDisposed[i].Dispose();
		}
	}
}
