using KuSonchik.Utilities;
using System;
using System.Collections.Generic;
using System.Windows;

namespace KuSonchik.Models;

public partial class Order
{
    [ColumnProperties("ID", 0.1)]
    [VisibilityColumn(Visibility.Visible, Visibility.Collapsed)]
    public int Id { get; set; }

    [ColumnProperties("ID клиента", 0.15)]
    [VisibilityColumn(Visibility.Visible, Visibility.Visible)]
    [IsForeignKey(true, typeof(Client), "Id")]
    public int? IdClient { get; set; }

    [ColumnProperties("ID товара", 0.15)]
    [VisibilityColumn(Visibility.Visible, Visibility.Visible)]
    [IsForeignKey(true, typeof(Product), "Id")]
    public int IdProduct { get; set; }

    [ColumnProperties("Дата", 0.2)]
    [VisibilityColumn(Visibility.Visible, Visibility.Visible)]
    public DateTime? Date { get; set; }

    [ColumnProperties("Статус", 0.2)]
    [VisibilityColumn(Visibility.Visible, Visibility.Visible)]
    public string? Status { get; set; }

    [ColumnProperties("Стоимость", 0.2)]
    [VisibilityColumn(Visibility.Visible, Visibility.Visible)]
    public int Cost { get; set; }

    [IsNavigation(true)]
    [VisibilityColumn(Visibility.Collapsed, Visibility.Collapsed)]
    public virtual ICollection<Delievery> Delieveries { get; set; } = new List<Delievery>();

    [IsNavigation(true)]
    [VisibilityColumn(Visibility.Collapsed, Visibility.Collapsed)]
    public virtual Client? IdClientNavigation { get; set; }

    [IsNavigation(true)]
    [VisibilityColumn(Visibility.Collapsed, Visibility.Collapsed)]
    public virtual Product IdProductNavigation { get; set; } = null!;
}
