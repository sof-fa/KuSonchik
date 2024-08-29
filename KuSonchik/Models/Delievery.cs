using KuSonchik.Utilities;
using System;
using System.Collections.Generic;
using System.Windows;

namespace KuSonchik.Models;

public partial class Delievery
{
    [ColumnProperties("ID", 0.1)]
    [VisibilityColumn(Visibility.Visible, Visibility.Collapsed)]
    public int Id { get; set; }

    [ColumnProperties("ID заказа", 0.1)]
    [VisibilityColumn(Visibility.Visible, Visibility.Visible)]
    [IsForeignKey(true, typeof(Client), "Id")]
    public int IdOrder { get; set; }

    [ColumnProperties("Тип", 0.2)]
    [VisibilityColumn(Visibility.Visible, Visibility.Visible)]
    public string Type { get; set; } = null!;

    [ColumnProperties("Стоимость", 0.2)]
    [VisibilityColumn(Visibility.Visible, Visibility.Visible)]
    public int Price { get; set; }

    [IsNavigation(true)]
    [VisibilityColumn(Visibility.Collapsed, Visibility.Collapsed)]
    public virtual Order IdOrderNavigation { get; set; } = null!;
}
