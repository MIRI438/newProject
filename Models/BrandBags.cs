namespace NEWPROJECT.Models;

public class BrandBags
{

    public int Id { get; set; }
    
    public string NameBrand { get; set; }
    
    public bool IsDesigning { get; set; }
    
    public bool IsVegan { get; set; }

    public int UserId { get; set; }
}

