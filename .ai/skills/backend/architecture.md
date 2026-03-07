# Architecture
- every new microservice or project is in src/{name}
- if it is microservice with business logic create in src/{name} to projects
    - chronos.{name}.api
    - chronos.{name}.core
- if it is microservice witout business logic create in src/{name} project with name 'chronos.{name}'
- Core Projects:
    - Domain - Models with business logic - only OOP architecture
    - Events - Integration Events - Event Driven Architecture
    - DAL - Configuration for MongoDB with Entity Framework (search details below in heading 'Data Access Layer')
    - Communication - Details in '.ai/communication.md'
    - Configutation - Extensions method with IServiceCollection argument, optional with IConfiguration

## Data Access Layer
- for microservice you build one DbContext '{microservice_name}DbContext' eg. EmployeesDbContext
- inside DbContext in OnModelCreating - Configuration for models