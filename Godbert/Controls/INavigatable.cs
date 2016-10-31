using SaintCoinach.Ex;
using SaintCoinach.Ex.Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Godbert.Controls
{
    public interface INavigatable
    {
        IRow OnNavigate(object sender, RoutedEventArgs e);
    }
}
