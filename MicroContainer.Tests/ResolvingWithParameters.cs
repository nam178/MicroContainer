using System;
using Xunit;

namespace MicroContainer.Tests
{
	public class ResolvingWithParameters
	{
		class MyDependency : IDisposable
		{
			public int DisposedCount
			{ get; set; }

			public string Name
			{ get; set; }

			public void Dispose()
			{
				DisposedCount++;
			}
		}

		class MyImpl
		{
			readonly MyDependency dependency;

			public MyDependency Dependency
			{ get { return dependency; } }

			public MyImpl(MyDependency dependency)
			{
				if (dependency == null)
					throw new ArgumentNullException("dependency");
				this.dependency = dependency;
			}
		}

		[Fact]
		public void Custom_Parameter_Value_Provider_Will_Be_Used()
		{
			var container = Container.Create();

			container
				.Register<MyImpl>()
				.AsSelf()
				.WithParameter(p => p.Name == "dependency", (p, c) => new MyDependency { Name = "Custom" });

			Assert.True(container.Resolve<MyImpl>().Dependency.Name == "Custom");
		}

		[Fact]
		public void Custom_Provided_Parameters_Are_Disposed_With_Container()
		{
			var container = Container.Create();
			container
				.Register<MyImpl>()
				.AsSelf()
				.WithParameter(p => p.Name == "dependency", (p, c) => new MyDependency { Name = "Custom" });

			var t = container.Resolve<MyImpl>();
			container.Dispose();
			Assert.Equal(1, t.Dependency.DisposedCount);
		}
	}
}
