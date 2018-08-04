
using Xunit;

namespace MicroContainer.Tests
{
	public class NamedRegistrationTest
	{
		class Implementation1 : IExampleInterface
		{

		}

		class Implementation2 : IExampleInterface
		{

		}

		[Fact]
		public void Resolve_Named_To_Correct_Named_Registration()
		{
			var container = Container.Create();
			container.Register<Implementation1>("firstImplementation").As<IExampleInterface>();
			container.Register<Implementation2>("secondImplementation").As<IExampleInterface>();

			Assert.True(container.Resolve<IExampleInterface>("firstImplementation").GetType() == typeof(Implementation1));
			Assert.True(container.Resolve<IExampleInterface>("secondImplementation").GetType() == typeof(Implementation2));
		}

		[Fact]
		public void Resolving_Non_Registered_Unnamed_Type_Still_Throws_Exception()
		{
			var container = Container.Create();
			container.Register<Implementation1>("firstImplementation").As<IExampleInterface>();

			Assert.Throws<DependencyException>(() => container.Resolve<Implementation1>());
		}

		[Fact]
		public void Named_Registration_Vs_Unnamed_Can_Coexist()
		{
			var container = Container.Create();
			container.Register<Implementation1>("firstImplementation").As<IExampleInterface>();
			container.Register<Implementation2>().As<IExampleInterface>();

			Assert.NotNull(container.Resolve<IExampleInterface>());
			Assert.NotNull(container.Resolve<IExampleInterface>("firstImplementation"));
			Assert.NotSame(container.Resolve<IExampleInterface>(), container.Resolve<IExampleInterface>("firstImplementation"));
		}

		[Fact]
		public void Named_Singleton_Test()
		{
			var container = Container.Create();
			container.Register<Implementation1>("firstImp").AsImplementedInterfaces().Singleton();
			container.Register<Implementation2>("secondImp").AsImplementedInterfaces().Singleton();
			container.Register<Implementation1>().AsImplementedInterfaces().Singleton();

			Assert.Same(
				container.Resolve<IExampleInterface>(),
				container.Resolve<IExampleInterface>());

			Assert.Same(
				container.Resolve<IExampleInterface>("firstImp"),
				container.Resolve<IExampleInterface>("firstImp"));

			Assert.Same(
				container.Resolve<IExampleInterface>("secondImp"),
				container.Resolve<IExampleInterface>("secondImp"));

			Assert.NotSame(
				container.Resolve<IExampleInterface>("firstImp"),
				container.Resolve<IExampleInterface>("secondImp"));

			Assert.NotSame(
				container.Resolve<IExampleInterface>("firstImp"),
				container.Resolve<IExampleInterface>());
		}

		[Fact]
		public void Named_Singleton_Per_Container_Test()
		{
			var container = Container.Create();

			container.Register<Implementation1>("firstImp").AsImplementedInterfaces().InstancePerContainer();

			var childContainer = container.Create("child");

			Assert.NotSame(
				container.Resolve<IExampleInterface>("firstImp"),
				childContainer.Resolve<IExampleInterface>("firstImp"));

			Assert.Same(
				childContainer.Resolve<IExampleInterface>("firstImp"),
				childContainer.Resolve<IExampleInterface>("firstImp")
				);

			Assert.Same(
				childContainer.Resolve<IExampleInterface>("firstImp"),
				childContainer.Resolve<IExampleInterface>("firstImp")
				);
		}

		[Fact]
		public void Named_Registration_Per_Named_Container_Test()
		{
			var container = Container.Create();

			container
				.Register<Implementation1>("firstImp")
				.AsImplementedInterfaces()
				.InstancePerNamedContainer("sessionContainer");

			container
				.Register<Implementation2>("secondImp")
				.AsImplementedInterfaces()
				.InstancePerNamedContainer("sessionContainer");

			Assert.Throws<DependencyException>(() => container.Resolve<IExampleInterface>("firstImp"));

			var sessionContainer = container.Create("sessionContainer");
			Assert.Same(
				sessionContainer.Resolve<IExampleInterface>("firstImp"),
				sessionContainer.Create().Resolve<IExampleInterface>("firstImp")
				);

			Assert.NotSame(
				sessionContainer.Resolve<IExampleInterface>("firstImp"),
				sessionContainer.Resolve<IExampleInterface>("secondImp")
				);

			Assert.Equal(
				typeof(Implementation1),
				sessionContainer.Resolve<IExampleInterface>("firstImp").GetType()
				);
		}
	}
}
