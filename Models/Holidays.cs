namespace HolidayTracker.Models;

public class Holidays : INotifyPropertyChanged
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

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
