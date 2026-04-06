using System;

namespace API.DTOs.Common;

public class BaseDto
{
    public DateTime DateCreated { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? DateModified { get; set; }
    public int? ModifiedBy { get; set; }
    public bool IsActive { get; set; }
}
