using Xunit;

namespace MicroContainer.Tests
{
	public class SingletonTest
	{
		[Fact]
		public void Non_Singleton_Creates_New_Instance_Everytime()
		{
			var container = Container.Create();
			container.Register<ExampleConcrete>().AsSelf();

			Assert.False(container.Resolve<ExampleConcrete>() == container.Resolve<ExampleConcrete>());
		}

		[Fact]
		public void Singleton_The_Same_In_Parent_And_Child_Container()
		{
			var container = Container.Create();
			container.Register<ExampleConcrete>().AsImplementedInterfaces().Singleton();

			Assert.True(container.Resolve<IExampleInterface>() == container.Resolve<IExampleInterface>(), "not a singleton");
			Assert.True(container.Resolve<IExampleInterface>()
				== container.Create("test").Resolve<IExampleInterface>(), "singleton in parent is different to the one in child");
			Assert.True(container.Resolve<IExampleInterface>()
				== container.Create().Resolve<IExampleInterface>(), "singleton in parent is different to the one in child");
		}


		[Fact]
		public void Non_Registered_Concrete_MustBe_Different_To_Registered_Interface()
		{
			var container = Container.Create();

			container.Register<ExampleConcrete>().AsSelf();

			container.Register<ExampleConcrete>().AsImplementedInterfaces().Singleton();

			var a = container.Resolve<ExampleConcrete>();
			var b = container.Resolve<IExampleInterface>();

			Assert.False(a == b, "Concrete was not registered as singleton, so must be different to the singleton");
		}
	}
}
