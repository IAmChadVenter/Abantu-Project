using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Abantu_System.Models
{
    public class Budget
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Budget_ID { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Currency)]
        public double Amount { get; set; } //Starting amount for a budget

        //public int Event_ID { get; set; }
        //public virtual Event Event{ get; set; }

        public int Programme_ID { get; set; }
        public virtual Programme Programme { get; set; }

        ////Rather use a list of expenses like above?
        //public int BudgetExpense_ID{ get; set; }
        public virtual ICollection<BudgetExpenses> Expenses { get; set; }
    }
}