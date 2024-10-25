using System;
using System.Collections.Generic;

namespace GeneratePhotoService.Models.ZelbikeChrono;

public class RaceOrganization
{
    public Guid OrganizationGuid { get; set; }

    public Guid RaceGuid { get; set; }

    public int RoleId { get; set; }

    public virtual Organization Organization { get; set; }

    public virtual Race Race { get; set; }
}
