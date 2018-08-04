using System;

namespace MicroContainer
{
	abstract class RegistrationBase : IRegistrationReadOnly
	{
		public string Name
		{ get; private set; }

		IResolver _resolver;
		public IResolver Resolver
		{
			get { return _resolver; }
			protected set
			{
				if (null == value)
					throw new ArgumentNullException("value");
				_resolver = value;
			}
		}

		Type _concreteType;
		public Type ConcreteType
		{ get { return _concreteType; } }

		public RegistrationBase(IResolver resolver, Type concreteType, string registrationName)
		{
			if (null == resolver)
				throw new ArgumentNullException("resolver");
			if (concreteType == null)
				throw new ArgumentNullException("concreteType");
			Name = registrationName;
			_resolver = resolver;
			_concreteType = concreteType;
		}
	}
}
