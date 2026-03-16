using chronos.jobs.synchronizer;
using chronos.jobs.synchronizer.Jobs;
using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<HangfireOptions>(builder.Configuration.GetSection(nameof(HangfireOptions)));

builder.Services.AddHangfire((serviceProvider, configuration) =>
{
    var hangfireOptions = serviceProvider.GetRequiredService<IOptions<HangfireOptions>>().Value;

    var mongoUrlBuilder = new MongoUrlBuilder(hangfireOptions.ConnectionString)
    {
        DatabaseName = hangfireOptions.DatabaseName
    };

    var mongoClient = new MongoClient(mongoUrlBuilder.ToMongoUrl());

    configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseMongoStorage(mongoClient, hangfireOptions.DatabaseName, new MongoStorageOptions
        {
            MigrationOptions = new MongoMigrationOptions
            {
                MigrationStrategy = new MigrateMongoMigrationStrategy(),
                BackupStrategy = new CollectionMongoBackupStrategy()
            },
            CheckQueuedJobsStrategy = CheckQueuedJobsStrategy.TailNotificationsCollection,
            Prefix = "hangfire"
        });
});

builder.Services.AddHangfireServer();
builder.Services.AddScoped<FailureFirstJob>();

var app = builder.Build();

app.UseHangfireDashboard();

RecurringJob.AddOrUpdate<FailureFirstJob>(
    "failure-first-job",
    job => job.ExecuteAsync(CancellationToken.None),
    Cron.Minutely);

app.Run();
