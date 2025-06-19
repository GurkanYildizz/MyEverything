namespace MyEverything.ThisMvc.Models;

public class Data_DTO
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Title { get; set; }
    public string? Image { get; set; }
    public string? Explanation { get; set; }

    public DateTime Created { get; set; } = DateTime.Now;
  
}

public static class Datas
{
    
    private static List<Data_DTO> allDatas = new List<Data_DTO>();

    public static void AddDatas(List<Data_DTO> datas)
    {
        allDatas.AddRange(datas);
    }

    public static void AddData(Data_DTO data)
    {
        allDatas.Add(data);
    }

    public static List<Data_DTO> GetAllDatas()
    {
        return allDatas;
    }
    public static void ClearDatas()
    {
        allDatas.Clear();
    }
}