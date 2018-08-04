namespace MicroContainer
{
	public static class Container
	{
		public static IContainer Create()
		{
			return new ContainerImpl(null);
		}
	}
}
