namespace Planner.DataAccess
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("U04D7a.incrementtypes")]
    public partial class Incrementtype
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IncrementTypeId { get; set; }

        [Required]
        [StringLength(45)]
        public string IncrementTypeDescription { get; set; }
    }
}
