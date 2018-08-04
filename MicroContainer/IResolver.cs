using System;

namespace MicroContainer
{
	public interface IResolver
	{
		object Resolve(IResolvingContext context);
	}
}