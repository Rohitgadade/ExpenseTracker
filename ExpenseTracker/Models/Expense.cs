
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Models
{
    public class Expense
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter expense title")]
        [StringLength(50, MinimumLength = 3,
            ErrorMessage = "Title must be between 3 and 50 characters")]
        public string Title { get; set; } = string.Empty;


        [Required(ErrorMessage = "Please enter amount")]
        [Range(1, 1000000,
            ErrorMessage = "Amount must be between 1 and 1000000")]
        public decimal Amount { get; set; }


        [Required(ErrorMessage = "Please select expense date")]
        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }


        [StringLength(200,
            ErrorMessage = "Description cannot exceed 200 characters")]
        public string? Description { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select category")]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }


    }
}
