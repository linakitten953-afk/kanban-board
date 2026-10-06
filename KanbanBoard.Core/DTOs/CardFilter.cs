using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanBoard.Core.DTOs;

public class CardFilter
{
    public string? SearchText { get; set; }
    public string? Priority { get; set; }
    public int? AssigneeId { get; set; }
}