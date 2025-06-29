using OfficeOpenXml;
using WebApplication1.Data;

namespace WebApplication1.Uttillities;
public static class ExcelImport
{
    static ExcelImport()
    {
        ExcelPackage.License.SetNonCommercialPersonal("Sanyok");
    }
    
    /// <exception cref="FormatException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public static void Import(string filePath)
    {
        List<PowerOff> result = new();
        
        using var package = new ExcelPackage(new FileInfo(filePath));
        var worksheet = package.Workbook.Worksheets[0]; // перший лист
        var rowCount = worksheet.Dimension.Rows;
        
        for (int row = 1; row <= rowCount; row++)
        {
            try
            {
                string[] areas = worksheet.Cells[row, 1].Text.Split(',');
                if (areas.Length < 1) throw new FormatException();
                
                TimeOnly start = TimeOnly.Parse(worksheet.Cells[row, 2].Text);
                TimeOnly end = TimeOnly.Parse(worksheet.Cells[row, 3].Text);
                
                var daysOfWeek = worksheet.Cells[row, 4].Text.Split(',').Select(int.Parse).ToArray();
                Console.WriteLine(string.Join(",",daysOfWeek));
                if(daysOfWeek.Any(x=>x < 0 || x > 6)) throw new FormatException("Invalid day of week");
                result.Add(new PowerOff(areas,start,end,daysOfWeek));
                
            }
            catch (FormatException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new InvalidOperationException();
            }
        }

        Schedule.SetNewSchedule(result.ToArray());
    }
}