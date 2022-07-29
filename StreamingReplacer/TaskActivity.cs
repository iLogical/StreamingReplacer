using Newtonsoft.Json;

namespace StreamingReplacer;

public sealed class TaskActivity
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("task_id")] public int? TaskId { get; set; }
    [JsonProperty("type")] public int Type { get; set; }
    [JsonProperty("occurred_at")] public DateTime OccurredAt { get; set; }
    [JsonProperty("message")] public string Message { get; set; }
    [JsonProperty("value_a")] public string ValueA { get; set; }
    [JsonProperty("value_b")] public string ValueB { get; set; }
    [JsonProperty("pinned")] public bool? Pinned { get; set; }
    [JsonProperty("archived")] public bool? Archived { get; set; }

    public TaskActivity()
    {
        Message = string.Empty;
        ValueA = string.Empty;
        ValueB = string.Empty;
    }
}