# Conventions
- In every subdirectory in Core project split configuration
    - add Configuration directory
    - inside add '{featurename}ConfigurationExtensions' eg. CommunicationConfigurationExtensions
    - public method should have name 'Add{featurename}' eg. AddRabbitMq(this IServiceCollection services)

- Asynchronous method should have suffix Async
- In Eventy Async method add CancellationToken cancellationToken = default as the last argument 