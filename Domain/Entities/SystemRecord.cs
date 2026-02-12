
namespace Domain.Entities;

public class SystemRecord
{
    public int SystemRecordId { get; set; }

    public string Description { get; set; } = null!;

    public byte Level { get; set; } = 1;

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public bool Finished { get; set; } = false;
}
