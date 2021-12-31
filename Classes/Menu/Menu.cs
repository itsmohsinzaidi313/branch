using System.Collections.Generic;
using Branch.Classes;
using Branch.Classes.Discounts;
using Branch.Classes.Menu;

public class Menu
{
    public List<Category> Categories { get; set; } = new List<Category>();
    public List<MenuItem> Items { get; set; } = new List<MenuItem>();
}