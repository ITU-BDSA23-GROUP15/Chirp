https://code-maze.com/onion-architecture-in-aspnetcore/

# Domain (Core/Chirp.Core) 

- Entities
- Repository interfaces

- Exceptions
- Domain services

## Examples:

- Classes with get/set. Etc.: Author, Cheep

- **Repository classes**
	- IAuthorRepository, ICheepRepository
	- Corresponding methods/tasks
- Exceptions would be defined here too, but handled higher up in the layers.

#Service/Application-Layer

- Definition for the service interface
- Data transfer objects (DTO's)

Note: It seems to look like we have skipped this step in our application, but as i can tell they often are displayed on the same layer as well. A small explanation below for why it can be relevant.
It looks to work like sort of like a singleton instance, meaning it is follows a lazy implementation.
This means the instance will only be created when we access them for the first time and not before.
The reason for having a service layer, is to isolate it and all services will not be available for any classes outside the service layer.


# Infrastructure layer
The Infrastructure layer should be concerned with encapsulating anything related to external systems or services that our application is interacting with. These external services can be:

- Database
- Identity provider
- Messaging queue
- Email service

# Examples to our project:

- DbContext (chirpContext)
	- builder
	- onModelCreating (constraints etc)
	- Database ensureCreated() //sort of -> which leads to next step.
- Repositories (Cheep,Author-repository)
	- Implements interface created in domain (Core)

# Presentation layer (Chirp.Razor/Web)

- Using restfulAPI (ASP.NET Core .Net Web package)
- Views (Pages, Models, Controllers)

