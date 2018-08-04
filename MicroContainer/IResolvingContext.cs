using System;
using System.Collections.Generic;

namespace MicroContainer
{
	public interface IResolvingContext
	{
		/// <summary>
		/// List of concrete types being resolved
		/// </summary>
		List<Type> ResolvingStack
		{ get; }

		/// <summary>
		/// The container that triggered the resolving operation
		/// </summary>
		IContainer KickOffContainer
		{ get; }

		/// <summary>
		/// The container that is currently asking to resolve
		/// </summary>
		IContainer CurrentContainer
		{ get; set; }
	}
}