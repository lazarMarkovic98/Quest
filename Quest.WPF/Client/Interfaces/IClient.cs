using Quest.Infrastructure.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Quest.WPF.Client.Interfaces;
public interface IClient
{
    ObservableCollection<ResultDto> ShowReports();
}
