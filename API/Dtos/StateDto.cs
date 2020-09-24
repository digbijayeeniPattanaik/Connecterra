using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class StateDto
    {
        [Required(ErrorMessage = "State is mandatory")]
        public string State { get; set; }
    }
}
