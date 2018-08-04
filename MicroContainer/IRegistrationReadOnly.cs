using System;

namespace MicroContainer
{
	public interface IRegistrationReadOnly
	{
		Type ConcreteType
		{ get; }

		IResolver Resolver
		{ get; }
	}
}
