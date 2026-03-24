using chronos.contracts.core.Domain.Identifiers;
using chronos.contracts.core.DTOs.Requests;
using chronos.contracts.core.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
builder.Services.AddCore(builder.Configuration);

var app = builder.Build();
app.UseCors("AllowAll");
app.UseChronosExceptionHandling();
app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.MapGet(
    "/api/contracts",
    async (
        IReadContractsService readContractsService,
        CancellationToken cancellationToken) =>
    {
        var contracts = await readContractsService.GetAllAsync(cancellationToken);
        return Results.Ok(contracts);
    })
    .WithName("GetContracts")
    .WithOpenApi();

app.MapGet(
    "/api/contracts/{contractId}",
    async (
        Ulid contractId,
        IReadContractsService readContractsService,
        CancellationToken cancellationToken) =>
    {
        var contract = await readContractsService.GetByIdAsync(
            new ContractId(contractId),
            cancellationToken);

        return contract is null
            ? Results.NotFound()
            : Results.Ok(contract);
    })
    .WithName("GetContractById")
    .WithOpenApi();

app.MapPost(
    "/api/contracts",
    async (
        CreateContractRequestDto request,
        IWriteContractsService contractsService,
        CancellationToken cancellationToken) =>
    {
        var result = await contractsService.CreateContractAsync(
            request.CompanyName,
            request.AssignmentDate,
            cancellationToken);

        return Results.Ok(result.Value.ToString());
    })
    .WithName("CreateContract")
    .WithOpenApi();

app.MapPost(
    "/api/contracts/{contractId}/employees",
    async (
        Ulid contractId,
        AssignEmployeeRequestDto request,
        IWriteContractsService contractsService,
        CancellationToken cancellationToken) =>
    {
        await contractsService.AssignEmployeeAsync(
            new ContractId(contractId),
            request.EmployeeId,
            request.From,
            request.To,
            request.AllocatedHours,
            cancellationToken);

        return Results.NoContent();
    })
    .WithName("AssignEmployee")
    .WithOpenApi();

app.MapDelete(
    "/api/contracts/{contractId}/employees/{contractEmployeeId}",
    async (
        Ulid contractId,
        Ulid contractEmployeeId,
        IWriteContractsService contractsService,
        CancellationToken cancellationToken) =>
    {
        await contractsService.RemoveEmployeeAsync(
            new ContractId(contractId),
            new ContractEmployeeId(contractEmployeeId),
            cancellationToken);

        return Results.NoContent();
    })
    .WithName("RemoveEmployee")
    .WithOpenApi();

app.MapPatch(
    "/api/contracts/{contractId}/employees/{contractEmployeeId}/allocated-hours",
    async (
        Ulid contractId,
        Ulid contractEmployeeId,
        UpdateAllocatedHoursRequestDto request,
        IWriteContractsService contractsService,
        CancellationToken cancellationToken) =>
    {
        await contractsService.UpdateEmployeeAllocatedHoursAsync(
            new ContractId(contractId),
            new ContractEmployeeId(contractEmployeeId),
            request.AllocatedHours,
            cancellationToken);

        return Results.NoContent();
    })
    .WithName("UpdateEmployeeAllocatedHours")
    .WithOpenApi();

app.MapPatch(
    "/api/contracts/{contractId}/close",
    async (
        Ulid contractId,
        CloseContractRequestDto request,
        IWriteContractsService contractsService,
        CancellationToken cancellationToken) =>
    {
        await contractsService.CloseContractAsync(
            new ContractId(contractId),
            request.ClosingDate,
            cancellationToken);

        return Results.NoContent();
    })
    .WithName("CloseContract")
    .WithOpenApi();

app.MapHealthChecks("/health/live");
app.MapHealthChecks("/health/ready");

app.Run();
