namespace HolidayTracker.Models
{
    public class GiftDay : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime Day {  get; set; }
        public string? Name { get; set; }
        public string? Person { get; set; }

        private bool _edit;

        public event PropertyChangedEventHandler? PropertyChanged;

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

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
