namespace Planner.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("U04D7a.city")]
    public partial class City
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CityId { get; set; }

        [Column("city")]
        [Required]
        [StringLength(50)]
        public string City1 { get; set; }

        public int CountryId { get; set; }

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
