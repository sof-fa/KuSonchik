using KuSonchik.Utilities;
using System;
using System.Collections.Generic;
using System.Windows;

namespace KuSonchik.Models;

public partial class Supplier
{
    [ColumnProperties("ID", 0.1)]
    [VisibilityColumn(Visibility.Visible, Visibility.Collapsed)]
    public int Id { get; set; }

    [ColumnProperties("Название", 0.2)]
    [VisibilityColumn(Visibility.Visible, Visibility.Visible)]
    public string Name { get; set; } = null!;

    [ColumnProperties("Номер телефона", 0.2)]
    [VisibilityColumn(Visibility.Visible, Visibility.Visible)]
    public decimal NumberPhone { get; set; }

    [ColumnProperties("ИНН", 0.2)]
    [VisibilityColumn(Visibility.Visible, Visibility.Visible)]
    public decimal Inn { get; set; }

    [IsNavigation(true)]
    [VisibilityColumn(Visibility.Collapsed, Visibility.Collapsed)]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
