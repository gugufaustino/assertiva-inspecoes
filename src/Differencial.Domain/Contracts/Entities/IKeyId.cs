using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Differencial.Domain.Contracts.Entities
{
    public interface IKeyId
    {
        [Key]
        int Id { get; set; }
    }
}
