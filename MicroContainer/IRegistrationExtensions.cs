using System;
using System.Reflection;

namespace MicroContainer
{
	public static class IRegistrationExtensions
	{
		public static IRegistration<T> AsImplementedInterfaces<T>(this IRegistration<T> registration)
		{
			foreach(var @interface in typeof(T).GetInterfaces())
				registration.As(@interface);
			return registration;
		}

		public static IRegistration AsImplementedInterfaces(this IRegistration registration)
		{
			foreach (var @interface in registration.ConcreteType.GetInterfaces())
				registration.As(@interface);
			return registration;
		}

		public static IRegistration<T> AsSelf<T>(this IRegistration<T> registration)
		{
			registration.As(typeof(T));
			return registration;
		}

		public static IRegistration AsSelf(this IRegistration registration)
		{
			registration.As(registration.ConcreteType);
			return registration;
		}

		public static IRegistration<T> WithParameter<T>(
			this IRegistration<T> registration, 
			string parameterName, 
			string registrationName)
		{
			if (parameterName == null)
				throw new ArgumentNullException("parameterName");
			if (registrationName == null)
				throw new ArgumentNullException("registrationName");

			registration.WithParameter(
				p => p.Name == parameterName,
				(p, c) => c.Resolve(p.ParameterType, registrationName)
				);
			return registration;
		}

		public static IRegistration<T> WithParameter<T>(
			this IRegistration<T> registration,
			Type parameterType,
			string registrationName)
		{
			if (parameterType == null)
				throw new ArgumentNullException("parameterType");
			if (registrationName == null)
				throw new ArgumentNullException("registrationName");

			registration.WithParameter(
				p => p.ParameterType == parameterType,
				(p, c) => c.Resolve(p.ParameterType, registrationName)
				);
			return registration;
		}

		public static IRegistration<T> WithParameter<T>(
			this IRegistration<T> registration,
			string parameterName,
			Func<ParameterInfo, IContainer, object> valueProvider)
		{
			if (parameterName == null)
				throw new ArgumentNullException("parameterName");
			if (valueProvider == null)
				throw new ArgumentNullException("valueProvider");

			registration.WithParameter(
				p => p.Name == parameterName,
				valueProvider
				);
			return registration;
		}

		public static IRegistration<T> WithParameter<T>(
			this IRegistration<T> registration,
			Type parameterType,
			Func<ParameterInfo, IContainer, object> valueProvider)
		{
			if (parameterType == null)
				throw new ArgumentNullException("parameterType");
			if (valueProvider == null)
				throw new ArgumentNullException("valueProvider");

			registration.WithParameter(
				p => p.ParameterType == parameterType,
				valueProvider
				);
			return registration;
		}
	}
}
