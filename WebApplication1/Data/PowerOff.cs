namespace WebApplication1.Data;
public class PowerOff
{
    public IEnumerable<string> Areas { get; init; }
    public TimeOnly StartTime { get; init; }
    public TimeOnly EndTime { get; init; }
    public IEnumerable<int> RegularityDaysOfWeek { get; init; }
     
    public bool UnscheduledOutage = false; 
    
    public PowerOff(IEnumerable<string> areas, TimeOnly startTime, TimeOnly endTime, IEnumerable<int> regularityDaysOfWeek)
    {
        Areas = areas;
        StartTime = startTime;
        EndTime = endTime;
        RegularityDaysOfWeek = regularityDaysOfWeek;
    }

    public bool IsElectricityOff()
    {
        var now = DateTime.Now;
        return UnscheduledOutage || RegularityDaysOfWeek.Contains((int)now.DayOfWeek) && StartTime < TimeOnly.FromDateTime(now) && EndTime > TimeOnly.FromDateTime(now);
    } 
}