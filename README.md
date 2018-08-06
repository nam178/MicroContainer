# MicroContainer - IoC

Dependency injection container, written for .NET 3.5 (Unity3d Projects).

# Features
* Mutable container, can override registrations at runtime.
* Supports named registration.
* Supports child container (Unit Of Work).
* Supports global singleton, per unit-of-work singleton and per named unit-of-work singleton.
* Syntax inspired by Autofac.

# Quick Start

First of all, create the root container. Make sure you include the MicroContainer namespace to enable its extension methods.

```csharp
using MicroContainer;
...
var root = Container.Create();
```

Then, register, or bind a concrete type to a service interface:

```csharp
root.Register<MachineGun>().As<IWeapon>();
```

From now on, components those depend on IWeapon, are injected with MachineGun, for example, this prints out "MachineGun"

```csharp
class PlayerController
{
    public PlayerController(IWeapon weapon) {
        Console.WriteLine(weapon.GetType().FullName);
    }
}

root.Register<PlayerController>().AsSelf();
root.Resolve<PlayerController>();
```

