using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MicroContainer
{
	sealed class AssemblyRegistration : IAssemblyRegistration
	{
		readonly IContainer _container;
		readonly Assembly _assembly;

		public AssemblyRegistration(IContainer container, Assembly assembly)
		{
			_container = container;
			_assembly = assembly;
		}

		public IAssemblyRegistration AsImplementedInterfaces()
		{
			foreach (var type in GetRegisterableTypes(_assembly))
				_container.Register(type).AsImplementedInterfaces();
			return this;
		}

		public IAssemblyRegistration AsSelf()
		{
			foreach (var type in GetRegisterableTypes(_assembly))
				_container.Register(type).AsSelf();
			return this;
		}

		static IEnumerable<System.Type> GetRegisterableTypes(Assembly assembly)
		{
			return assembly.GetTypes().Where(t => false == t.IsInterface && false == t.IsAbstract);
		}
	}
}