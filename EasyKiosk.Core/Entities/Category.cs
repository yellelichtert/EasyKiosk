using System.ComponentModel.DataAnnotations;

namespace EasyKiosk.Core.Entities;

public class Category : Entity
{
    public string Name;
    
    public List<Product> Products;
}