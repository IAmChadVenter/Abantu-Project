using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Abantu_System.Models
{
    public class BudgetExpenses
    {
        [Key]
        [Required(ErrorMessage = "*")]
        [StringLength(50)]
        public string ExpenseName { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Currency)]
        public double Amount { get; set; } //Starting amount for a budget

        public int Budget_ID { get; set; }
        public virtual Budget Budget { get; set; }

    }
}