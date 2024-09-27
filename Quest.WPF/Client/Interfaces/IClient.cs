using Quest.Infrastructure.Helper;
using System.Collections.ObjectModel;

namespace Quest.WPF.Client.Interfaces;
public interface IClient
{
    ObservableCollection<ResultDto> ShowReports();
}
