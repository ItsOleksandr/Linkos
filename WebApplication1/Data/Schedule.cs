namespace WebApplication1.Data;
public class Schedule
{
    public static PowerOff[] PowerOffs => _powerOffs;
    private static PowerOff[] _powerOffs =
    [
        new PowerOff(["Center","Holosievski","Podol"],new TimeOnly(8,30),new TimeOnly(11,30), [0,4]),
        new PowerOff(["Teremki","Poznyaki","Svyatoshin","Obolon","Pochaina"],new TimeOnly(15,30),new TimeOnly(23,00), [3,5,6]),
        new PowerOff(["DVRZ","VDNG","TSYM"],new TimeOnly(12,00),new TimeOnly(15,00), [1,2]),
    ];
    
    /// <exception cref="InvalidOperationException"></exception>
    public static PowerOff GetByArea(string? area)  => _powerOffs.First(x => area != null && x.Areas.Contains(area));
    public static void SetNewSchedule(PowerOff[] powerOffs) => _powerOffs = powerOffs;
}