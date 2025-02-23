using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EScinece.Infrastructure.Helpers;
public class JwtOptions
{
    public string SecretKey { get; set; } = string.Empty;
    public int ExpiresHours { get; set; } = 0;
    public int ExpiresDays { get; set; } = 0;
}