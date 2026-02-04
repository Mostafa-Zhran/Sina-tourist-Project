using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sina_BLL.DTO.AuthintcationDto
{
    public class AuthResponseDto
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }

        public string? Token { get; set; }

        public string? Id { get; set; }
        public string? Email { get; set; }
        public List<string>? Roles { get; set; }
    }
}
