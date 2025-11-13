using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppDevicesMedical.Models
{
    public class Examen
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdExamen { get; set; }

        [Required]
        [StringLength(255)]
        public string Titulo { get; set; }

        [Required]
        public string EnlaceFormulario { get; set; }
    }

}
