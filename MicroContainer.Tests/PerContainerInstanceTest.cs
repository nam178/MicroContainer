using Xunit;

namespace MicroContainer.Tests
{
	public class PerContainerInstanceTest
	{
		[Fact]
		public void Different_Containers_Resolve_Different_Instances()
		{
			var root = Container.Create();
			root.Register<ExampleConcrete>().AsImplementedInterfaces().InstancePerContainer();

			var x = root.Resolve<IExampleInterface>();
			var x2 = root.Resolve<IExampleInterface>();

			var child = root.Create();
			var y = child.Resolve<IExampleInterface>();
			var y2 = child.Resolve<IExampleInterface>();

			var grandChild = child.Create();
			var z = grandChild.Resolve<IExampleInterface>();
			var z2 = grandChild.Resolve<IExampleInterface>();

			Assert.NotSame(y, z);
			Assert.NotSame(z, x);
			Assert.NotSame(x, y);
			Assert.Same(x, x2);
			Assert.Same(y, y2);
			Assert.Same(z, z2);
		}

		[Fact]
		public void Overwrite_Singleton_In_Root_Container()
		{
			var root = Container.Create();
			root.Register<ExampleConcrete>().AsImplementedInterfaces().Singleton();

			var child = root.Create();
			child.Register<ExampleConcrete>().AsImplementedInterfaces().InstancePerContainer();

			var rootInstance = root.Resolve<IExampleInterface>();
			var childInstance = child.Resolve<IExampleInterface>();

			Assert.NotSame(rootInstance, childInstance);
		}

		[Fact]
		public void Finding_Correct_Dependencys_Container()
		{
			var root = Container.Create();

			root.Register<MyConcreteWithDependency>().AsSelf().Singleton();
			root.Register<MyDependency>().AsSelf().InstancePerContainer();

			var child = root.Create("child");

			var singletonInstance = child.Resolve<MyConcreteWithDependency>();
			Assert.NotSame(singletonInstance.Dependency, child.Resolve<MyDependency>());
			Assert.Same(singletonInstance.Dependency, root.Resolve<MyDependency>());
		}
	}
}
