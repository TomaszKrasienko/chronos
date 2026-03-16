using System.Net.Http.Json;
using chronos.ui.Models;

namespace chronos.ui.Services;

public sealed class NotificationService(
    ILogger<NotificationService> logger,
    IHttpClientFactory httpClientFactory) : INotificationService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("NotificationsAPI");

    public async Task<IReadOnlyCollection<NotificationMessage>> GetUnreadAsync(string employeeId)
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("X-Employee-Id", employeeId);

            var notifications = await _httpClient.GetFromJsonAsync<List<NotificationMessage>>("notification-messages/unread");
            return notifications ?? new List<NotificationMessage>();
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Error loading unread notifications for employee {EmployeeId}", employeeId);
            return [];
        }
    }

    public async Task MarkAsReadAsync(string notificationId, string employeeId)
    {
        using var requestMessage = new HttpRequestMessage(
            HttpMethod.Put,
            $"notification-messages/{notificationId}/read-status");
        requestMessage.Headers.Add("X-Employee-Id", employeeId);

        var response = await _httpClient.SendAsync(requestMessage);
        response.EnsureSuccessStatusCode();
    }
}
