using Microsoft.Extensions.Options;
using Quest.Infrastructure.Helper;
using Quest.WPF.Client.Interfaces;
using Quest.WPF.Configuration;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
namespace Quest.WPF;
public partial class MainWindow : Window, INotifyPropertyChanged
{
    private readonly IOptionsMonitor<SetupOptions>? _setupOptions;
    private readonly IClient? _client;
    private Process _engineProcess = new Process();
    private static Timer? _timer;
    public event PropertyChangedEventHandler? PropertyChanged;
    private ObservableCollection<ResultDto> _items = new ObservableCollection<ResultDto>();
    public ObservableCollection<ResultDto> Items
    {
        get => _items;
        set => SetField(ref _items, value);
    }
    private String _labelConcurrentJobs = string.Empty;
    public String LabelConcurrentJobs
    {
        get { return _labelConcurrentJobs; }
        set
        {
            _labelConcurrentJobs = value;
            OnPropertyChanged(nameof(LabelConcurrentJobs));

        }
    }
    private String _labelContent = string.Empty;
    public String LabelContent
    {
        get { return _labelContent; }
        set
        {
            _labelContent = value;
            OnPropertyChanged(nameof(LabelContent));

        }
    }

    public MainWindow(IOptionsMonitor<SetupOptions> setupOptions, IClient? client)
    {
        InitializeComponent();
        DataContext = this;

        _setupOptions = setupOptions ?? throw new ArgumentNullException(nameof(setupOptions));
        _client = client ?? throw new ArgumentNullException(nameof(client));

        Reports.ItemsSource = Items;
        LabelContent = "Check interval: " + _setupOptions.CurrentValue.CheckInterval + " milliseconds";
        LabelConcurrentJobs = "Concurrent Jobs: " + _setupOptions.CurrentValue.MaximumConcurentProcessingJobs;

        RunEngine(null, null);
        _setupOptions.OnChange(OnConfigurationChange);

    }
    public MainWindow() { }

    private void ShowReports(object sender, RoutedEventArgs e)
    {
        Items.Clear();
        foreach (var item in _client!.ShowReports())
        {
            _items.Add(item);
        }
    }
    private void RunEngine(object? sender, RoutedEventArgs? e)
    {
        string solutionDirectory = Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent!.Parent!.FullName;
        if (string.IsNullOrEmpty(solutionDirectory)) return;

        string consoleAppPath = Path.Combine(solutionDirectory, "Quest.Engine", "bin", "Debug", "net8.0", "Quest.Engine.exe");

        if (!File.Exists(consoleAppPath))
        {
            MessageBox.Show($"Console application not found at {consoleAppPath}");
            return;
        }
        _engineProcess.StartInfo.FileName = consoleAppPath;
        _engineProcess.StartInfo.UseShellExecute = true;

        try
        {
            _engineProcess.Start();
            SetInterval(_setupOptions!.CurrentValue.CheckInterval);

            RunEngineButton.Visibility = Visibility.Collapsed;
            StopEngineButton.Visibility = Visibility.Visible;

        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to start server: {ex.Message}");
        }
    }
    private void StopEngine(object sender, RoutedEventArgs e)
    {
        _engineProcess.Kill();
        RunEngineButton.Visibility = Visibility.Visible;
        StopEngineButton.Visibility = Visibility.Collapsed;
    }
    void SetInterval(int interval)
    {
        _timer = new Timer(TimerCallback, null, 0, interval);
    }
    private void TimerCallback(object? state)
    {
        try
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ShowReports(null!, null!);
            });
        }
        catch (TimeoutException ex)
        {
            _timer!.Change(Timeout.Infinite, Timeout.Infinite);
            MessageBox.Show("Error: " + ex.Message);

        }

    }
    private void OnConfigurationChange(SetupOptions options)
    {
        LabelContent = "Check interval: " + options.CheckInterval + " milliseconds";
        LabelConcurrentJobs = "Concurrent Jobs: " + options.MaximumConcurentProcessingJobs;

    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName!);
        return true;
    }

}