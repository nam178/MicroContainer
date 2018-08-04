using Xunit;

namespace MicroContainer.Tests
{
	public class SomeRegistrationsTest
	{
		interface INotRegistered
		{

		}

		interface IRegisterd
		{

		}

		class RegisteredImpl : IRegisterd
		{

		}

		class RegisteredImplOverwrite : IRegisterd
		{

		}

		[Fact]
		public void Resolving_Non_Registered_Interface()
		{
			Assert.Throws<DependencyException>(() => Container.Create().Resolve<INotRegistered>());
		}

		[Fact]
		public void Resolving_Registered_Interface()
		{
			var container = Container.Create();
			container
				.Register<RegisteredImpl>()
				.AsImplementedInterfaces();

			Assert.NotNull(container.Resolve<IRegisterd>());
		}

		[Fact]
		public void Overwrite_Registration()
		{
			var container = Container.Create();
			container.Register<RegisteredImplOverwrite>().AsImplementedInterfaces();

			Assert.True(container.Resolve<IRegisterd>().GetType() == typeof(RegisteredImplOverwrite));
		}
	}
}
