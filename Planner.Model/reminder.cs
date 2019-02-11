namespace Planner.DataAccess
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("U04D7a.reminder")]
    public partial class Reminder
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ReminderId { get; set; }

        public DateTime ReminderDate { get; set; }

        public int SnoozeIncrement { get; set; }

        public int SnoozeIncrementTypeId { get; set; }

        public int AppointmentId { get; set; }

        [Required]
        [StringLength(45)]
        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        [Required]
        [StringLength(45)]
        public string Remindercol { get; set; }
    }
}
