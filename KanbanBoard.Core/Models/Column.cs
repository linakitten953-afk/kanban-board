using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanBoard.Core.Models;

public class Column
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Order { get; set; }
    public int? WipLimit { get; set; } 
    public int BoardId { get; set; }
    public Board? Board { get; set; }
    public List<Card> Cards { get; set; } = new();
}