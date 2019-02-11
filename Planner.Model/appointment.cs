namespace Planner.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("U04D7a.appointment")]
    public partial class Appointment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AppointmentId { get; set; }

        public int CustomerId { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Column(TypeName = "text")]
        [Required]
        [StringLength(65535)]
        public string Description { get; set; }

        [Column(TypeName = "text")]
        [Required]
        [StringLength(65535)]
        public string Location { get; set; }

        [Column(TypeName = "text")]
        [Required]
        [StringLength(65535)]
        public string Contact { get; set; }

        [Required]
        [StringLength(255)]
        public string Url { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

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
