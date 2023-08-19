using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace XichLip.WebApi.Models.Base
{
    public class BaseRequestGetModel
    {
        [Required(ErrorMessage = "required_field")]
        
        [Range(1, int.MaxValue, ErrorMessage = "ErrorParamId")]
        public int Id { get; set; }
      
    }
}
