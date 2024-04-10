using EmpManager.Models;

namespace EmpManager.ViewModels
{
    public class SearchResultVM
    {
        public DateTime SearchDate { get; set; }
        public List<Employee> Employees { get; set; } = new List<Employee>();
    }
}
