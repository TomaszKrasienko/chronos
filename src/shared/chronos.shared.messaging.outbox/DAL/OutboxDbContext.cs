using Microsoft.EntityFrameworkCore;

namespace chronos.shared.messaging.outbox.DAL;

public class OutboxDbContext(DbContextOptions<OutboxDbContext> options) : DbContext(options)
{
    public DbSet<OutboxMessage> OutboxMessages { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("outbox");
        
        modelBuilder.Entity<OutboxMessage>(entity =>
        {
            entity.ToTable("outbox_messages");
            entity.HasKey(e => e.MessageId);
            entity.Ignore(e => e.Content);

            entity.Property(e => e.MessageId)
                .HasColumnName("message_id")
                .HasConversion(
                    v => v.ToString(),
                    v => Ulid.Parse(v));

            entity.Property(e => e.JsonContent)
                .HasColumnName("json_content")
                .IsRequired();

            entity.Property(e => e.Type)
                .HasColumnName("type")
                .IsRequired();

            entity.Property(e => e.Exchange)
                .HasColumnName("exchange")
                .IsRequired();

            entity.Property(e => e.CorrelationId)
                .HasColumnName("correlation_id");

            entity.Property(e => e.RoutingKey)
                .HasColumnName("routing_key");

            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            entity.Property(e => e.SentAt)
                .HasColumnName("sent_at");

            entity.Property(e => e.RetryCount)
                .HasColumnName("retry_count")
                .HasDefaultValue(0);

            entity.Property(e => e.ErrorMessage)
                .HasColumnName("error_message");

            entity.HasIndex(e => new { e.SentAt, e.RetryCount })
                .HasDatabaseName("ix_outbox_messages_sent_at_retry_count");
        });
    }
}