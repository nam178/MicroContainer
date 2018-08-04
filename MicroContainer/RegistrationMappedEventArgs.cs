using System;

namespace MicroContainer
{
	sealed class RegistrationMappedEventArgs : EventArgs
	{
		readonly Type _interfaceType;
		public Type InterfaceType
		{ get { return _interfaceType; } }

		readonly Type _concreteType;
		public Type ConcreteType
		{ get { return _concreteType; } }

		public RegistrationMappedEventArgs(
			Type interfaceType,
			Type concreteType
			)
		{
			if (interfaceType == null)
				throw new ArgumentNullException("interfaceType");
			if (concreteType == null)
				throw new ArgumentNullException("concreteType");
			_interfaceType = interfaceType;
			_concreteType = concreteType;
		}
	}
}