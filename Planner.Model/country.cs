namespace Planner.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("U04D7a.country")]
    public partial class Country
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CountryId { get; set; }

        [Column("country")]
        [Required]
        [StringLength(50)]
        public string Country1 { get; set; }

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
