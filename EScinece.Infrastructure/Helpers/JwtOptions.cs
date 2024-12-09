using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EScinece.Infrastructure.Helpers;
internal class JwtOptions
{
    public string SecretKey { get; set; } = string.Empty;
    public int ExpitesHours { get; set; } = 0;
}