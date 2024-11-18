namespace HolidayTracker.Models;

public class Holiday : INotifyPropertyChanged
{

    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    private bool _edit;

    [Ignore]
    public bool Edit
    {
        get
        {
            return _edit;
        }

        set
        {
            _edit = value;
            OnPropertyChanged(nameof(Edit));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Person { get; set; }
    public string Status { get; set; }
    public int Taken { get; set; }
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
