namespace Planner.DataAccess
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("U04D7a.customer")]
    public partial class Customer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(45)]
        public string CustomerName { get; set; }

        public int AddressId { get; set; }

        public bool Active { get; set; }

        public DateTime CreateDate { get; set; }

        [Required]
        [StringLength(40)]
        public string CreatedBy { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime LastUpdate { get; set; }

        [Required]
        [StringLength(40)]
        public string LastUpdateBy { get; set; }
    }
}
