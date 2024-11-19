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

    private double _numberOfDaysTaken;

    public double NumberOfDaysTaken
    {
        get
        {
            return _numberOfDaysTaken;
        }

        set
        {
            _numberOfDaysTaken = value;
            OnPropertyChanged(nameof(NumberOfDaysTaken));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public string? Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Person { get; set; }
    public string? Status { get; set; }
    public bool HalfDay { get; set; }
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
