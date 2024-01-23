### Installation

Package can be installed using the Nuget package manager or the dotnet CLI.

```sh
dotnet add package Geneirodan.Generics.Repository
```

### Usage

Create your database entity class:
```csharp
using Geneirodan.Generics.Repository;

public class User : Entity<string>{
    public string Name { get; set;} = null!;    
}
```

Create EF Core DbContext containing set of your entities:
```csharp
public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
}
```

Create your entity repository:
```csharp
public class UserRepository(ApplicationContext context) 
    : Repository<User, string, ApplicationContext>(context);
```

Example usage:
```csharp
using var context = new ApplicationContext();

var repository = new UserRepository(context);

// Add user
User created = repository.Add(new User { Name = "John" });

// Get user
User user = await repository.await repository.GetAsync(created.Id);

// Remove user
repository.Remove(user);

// Save changes
await repository.ConfirmAsync();
```