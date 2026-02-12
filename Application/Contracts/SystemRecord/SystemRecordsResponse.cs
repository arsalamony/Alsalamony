

namespace Application.Contracts.SystemRecord;

public class SystemRecordsResponse
{
    public int SystemRecordId { get; set; }

    public string Description { get; set; } = null!;

    public byte Level { get; set; }

    public bool Finished { get; set; }

    public DateTime CreatedDate { get; set; }
}
