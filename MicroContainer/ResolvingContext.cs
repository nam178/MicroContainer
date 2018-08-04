using System;
using System.Collections.Generic;

namespace MicroContainer
{
	sealed class ResolvingContext : IResolvingContext
	{
		public IContainer KickOffContainer
		{ get; set; }

		public IContainer CurrentContainer
		{ get; set; }

		readonly List<Type> _resolvingStack = new List<Type>();
		public List<Type> ResolvingStack
		{ get { return _resolvingStack; } }
	}
}
