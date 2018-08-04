using System;
using System.Reflection;

namespace MicroContainer
{
	public static class IContainerExtensions
	{
		static IActivator GetDefaultActivator()
		{
			return new ActivatorReflection();
		}

		public static T Resolve<T>(this IContainer container, string registrationName = null)
		{
			return (T)container.Resolve(typeof(T), registrationName);
		}

		public static IAssemblyRegistration RegisterAssembly(
			this IContainer container, 
			Type exampleTypeInAssembly)
		{
			return container.RegisterAssembly(Assembly.GetAssembly(exampleTypeInAssembly));
		}

		public static IAssemblyRegistration RegisterAssembly<TExample>(
			this IContainer container)
		{
			return container.RegisterAssembly(typeof(TExample));
		}

		public static IAssemblyRegistration RegisterAssembly(
			this IContainer container, 
			Assembly assembly)
		{
			return new AssemblyRegistration(container, assembly);
		}

		public static IRegistration<TConcrete> Register<TConcrete>(
			this IContainer container, 
			string registrationName = null)
		{
			return new RegistrationGeneric<TConcrete>(
				container,
				GetDefaultActivator(),
				registrationName);
		}

		public static IRegistration<TConcrete> Register<TConcrete>(
			this IContainer container, 
			Func<IContainer, object> activator, 
			string registrationName = null)
		{
			var activatorLambda = new ActivatorLambda(activator);
			return new RegistrationGeneric<TConcrete>(
				container,
				activatorLambda,
				registrationName
				);
		}

		public static IRegistration Register(
			this IContainer container, 
			Type type, 
			string registrationName = null)
		{
			return new RegistrationNonGeneric(
				container,
				GetDefaultActivator(),
				type,
				registrationName);
		}

		public static IRegistration<T> RegisterInstance<T>(
			this IContainer container, 
			T instance,
			string registrationName = null)
		{
			return container
				.Register<T>(x => instance, registrationName)
				.AsSelf()
				.Singleton();
		}
	}
}
