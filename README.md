# Entity Api

This quickly allows you to setup a rest api for all your entities.

Includes:

 - **BaseEntity**: The base entity class which all your entities should extend.
   
    The BaseEntity contains the following fields:
    - *Guid* Id
    - *DateTime* CreatedAt
    - *DateTime* UpdatedAt
     

 - **EntityController**: Extendable controller containing the basic CRUD routes.
 - **EntityPolicy**: By default all CRUD actions are allowed. Extend and override as required.



## Setup

Assuming an entity called `Todo` you can create a basic crud controller with the following code:



### Creating the entity

Recommended to store all entities in a directory called `Entities`

```
using System.ComponentModel.DataAnnotations;
using Bcc.EntityApi;

namespace Bcc.Example.Entities
{
    public class Todo : BaseEntity
    {
        [Required]
        public string Details { get; set; }
        public bool Done { get; set; } = false;
    }
}
```

### Creating the context

Create a context file class that extends the BaseContext:

```
using Bcc.EntityApi;
using Microsoft.EntityFrameworkCore;

namespace Bcc.Example
{
    public class EntityContext : EntityApi.BaseContext
    {
        // Add you DbSet<T> here:
        // eg. public DbSet<User> Users { get; set; }
        public DbSet<Todo> Todos { get; set; }

        public EntityContext(DbContextOptions options) : base(options)
        {
        }
    }
}
```

### Creating the controller

Recommended to store all controllers in a directory called `Controllers`

```
using Bcc.EntityApi;
using Microsoft.AspNetCore.Mvc;

namespace Bcc.Example.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : EntityController<Todo>
    {
        public TodoController(EntityContext context, EntityPolicy<Todo> policy)
         : base(context, policy)
        {
        }
    }
}
```

### Defining the entity's policy (optional)

Recommended to store all policies in a directory called `Policies`

Let's disable being able to view all todo's by overriding the `CanViewAny` policy

```
using System.Threading.Tasks;
using Bcc.EntityApi;

namespace Bcc.Example.Policies
{
    public class TodoPolicy : EntityPolicy<Todo>
    {
        public override Task<bool> CanViewAny(Todo entity = null)
        {
            return Task.FromResult(false);
        }
    }
}
```


## Extra


To make use of the default swagger comments,
add the following line into your `Startup.cs` file under the `ConfigureServices` function

```
services.AddSwaggerGen(c =>
{
    …
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Bcc.EntityApi.xml"));
});
```

