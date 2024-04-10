using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmpManager.Models
{
    public class Leave
    {
        [Key]
        public int Id { get; set; }
        public LeaveType Type { get; set; }
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        public bool IsApproved { get; set; } = false;
        private bool _handled { get; set; } = false;
        public bool Handled
        {
            get { return _handled; }
            set
            {
                if (value && !_handled)
                {
                    _handled = true;
                    HandledDate = DateTime.Now;
                }
                else if (!value && _handled)
                {
                    _handled = false;
                    HandledDate = default;
                }
            }
        }
        [Display(Name = "Application Handled")]
        public DateTime HandledDate { get; private set; }
        [Display(Name = "Application Created")]
        public DateTime Created { get; set; } = DateTime.Now;

        [ForeignKey("Employee")]
        public int FkEmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }
}
