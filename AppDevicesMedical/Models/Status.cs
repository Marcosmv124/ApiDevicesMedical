using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppDevicesMedical.Models
{
    public class Status
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // Sin auto-incremento
        public int Id_status { get; set; }
        [Required]
        [StringLength(50)]
        public string Nombre_status { get; set; } = string.Empty;
      
    }
}
