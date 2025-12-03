public class GetByNameRequestDto
{
    public string Name { get; set; }
    public long StartTime { get; set; }  // Epoch milliseconds
    public long EndTime { get; set; }    // Epoch milliseconds
}
