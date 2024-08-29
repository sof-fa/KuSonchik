using KuSonchik.Utilities;
using System;
using System.Collections.Generic;
using System.Windows;

namespace KuSonchik.Models;

public partial class Product
{
    [ColumnProperties("ID", 0.05)]
    [VisibilityColumn(Visibility.Visible, Visibility.Collapsed)]
    public int Id { get; set; }

    [ColumnProperties("ID поставщика", 0.25)]
    [VisibilityColumn(Visibility.Visible, Visibility.Visible)]
    [IsForeignKey(true, typeof(Supplier), "Id")]
    public int? IdSupplier { get; set; }

    [ColumnProperties("Наименование", 0.28)]
    [VisibilityColumn(Visibility.Visible, Visibility.Visible)]
    public string Name { get; set; } = null!;

    [ColumnProperties("Категория", 0.20)]
    [VisibilityColumn(Visibility.Visible, Visibility.Visible)]
    public string? Category { get; set; }

    [ColumnProperties("Стоимость", 0.19)]
    [VisibilityColumn(Visibility.Visible, Visibility.Visible)]
    public int Price { get; set; }

    [ColumnProperties("Размер", 0.14)]
    [VisibilityColumn(Visibility.Visible, Visibility.Visible)]
    public string? Size { get; set; }

    [ColumnProperties("Цвет", 0.09)]
    [VisibilityColumn(Visibility.Visible, Visibility.Visible)]
    public string? Color { get; set; }

    [IsNavigation(true)]
    [VisibilityColumn(Visibility.Collapsed, Visibility.Collapsed)]
    public virtual Supplier? IdSupplierNavigation { get; set; }

    [IsNavigation(true)]
    [VisibilityColumn(Visibility.Collapsed, Visibility.Collapsed)]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
