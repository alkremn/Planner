namespace Planner.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("U04D7a.address")]
    public partial class Address
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AddressId { get; set; }

        [Column("address")]
        [Required]
        [StringLength(50)]
        public string Address1 { get; set; }

        [Required]
        [StringLength(50)]
        public string Address2 { get; set; }

        public int CityId { get; set; }

        [Required]
        [StringLength(10)]
        public string PostalCode { get; set; }

        [Required]
        [StringLength(20)]
        public string Phone { get; set; }

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
