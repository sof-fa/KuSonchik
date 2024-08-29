using KuSonchik.Utilities;
using System;
using System.Collections.Generic;
using System.Windows;

namespace KuSonchik.Models;

public partial class Client
{
    [ColumnProperties("ID", 0.05)]
    [VisibilityColumn(Visibility.Visible, Visibility.Collapsed)]
    public int Id { get; set; }

    [ColumnProperties("Фамилия", 0.19)]
    [VisibilityColumn(Visibility.Visible, Visibility.Visible)]
    public string Surname { get; set; } = null!;

    [ColumnProperties("Имя", 0.19)]
    [VisibilityColumn(Visibility.Visible, Visibility.Visible)]
    public string Firstname { get; set; } = null!;

    [ColumnProperties("Отчество", 0.19)]
    [VisibilityColumn(Visibility.Visible, Visibility.Visible)]
    public string? Patronymic { get; set; }

    [ColumnProperties("Номер телефона", 0.28)]
    [VisibilityColumn(Visibility.Visible, Visibility.Visible)]
    public decimal NumberPhone { get; set; }


    [ColumnProperties("Статус", 0.2)]
    [VisibilityColumn(Visibility.Visible, Visibility.Visible)]
    public string? Status { get; set; }


    [IsNavigation(true)]
    [VisibilityColumn(Visibility.Collapsed, Visibility.Collapsed)]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
