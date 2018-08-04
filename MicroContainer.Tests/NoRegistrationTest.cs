using Xunit;

namespace MicroContainer.Tests
{
	public class SimpleRegistartionTest
	{
		[Fact]
		public void Resolving_Concrete_Without_Dependency()
		{
			var container = Container.Create();
			container.RegisterAssembly<SimpleRegistartionTest>().AsSelf();

			Assert.NotNull(container.Resolve<MyDependency>());
		}

		[Fact]
		public void Resolving_Concrete_With_Dependency()
		{
			var container = Container.Create();
			container.RegisterAssembly<SimpleRegistartionTest>().AsSelf();

			Assert.NotNull(container.Resolve<MyConcreteWithDependency>());
		}

		[Fact]
		public void Resolving_Concrete_With_Circular_Dependency()
		{
			var container = Container.Create();
			container.RegisterAssembly<SimpleRegistartionTest>().AsSelf();

			Assert.Throws<CircularDependencyException>(() => container.Resolve<MyConcreteWithCircularDependency>());
		}
	}

	class MyDependency
	{

	}

	class MyConcreteWithDependency
	{
		public MyDependency Dependency
		{ get; set; }

		public MyConcreteWithDependency(MyDependency dependency)
		{
			if (dependency == null)
				throw new System.ArgumentNullException("dependency");
			Dependency = dependency;
		}
	}

	class MyConcreteWithCircularDependency
	{
		public MyConcreteWithCircularDependency(MyConcreteWithCircularDependency2 dep)
		{
			if (dep == null)
			{
				throw new System.ArgumentNullException("dep");
			}
		}
	}

	class MyConcreteWithCircularDependency2
	{
		public MyConcreteWithCircularDependency2(MyConcreteWithCircularDependency dep)
		{
			if (dep == null)
				throw new System.ArgumentNullException("dep");
		}
	}

	class SomeOtherClass
	{

	}

	class MyConcreteThisIsNotCircularDependency
	{
		public MyConcreteThisIsNotCircularDependency(SomeOtherClass dep, SomeOtherClass dep2)
		{

		}
	}

}
