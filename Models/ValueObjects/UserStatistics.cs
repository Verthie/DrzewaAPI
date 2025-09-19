using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrzewaAPI.Models.ValueObjects;

public class UserStatistics
{
    public int SubmissionCount { get; set; } = 0;
    public int ApplicationCount { get; set; } = 0;
}