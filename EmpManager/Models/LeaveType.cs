using System.ComponentModel.DataAnnotations;

namespace EmpManager.Models
{
    public enum LeaveType
    {
        Vacation = 1,
        Sick,
        [Display(Name = "Care For Sick Child")]
        CareForChildren,
        Leave

    }
}
