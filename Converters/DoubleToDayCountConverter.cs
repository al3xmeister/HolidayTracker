
namespace HolidayTracker.Converters
{
    public class DoubleToDayCountConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is double d)
            {
                //return no decimals if castable to an int
                if (d % 1 == 0)
                {
                    return $"{parameter} {d:F0}";
                }
                //return 1 decimal if not castable to an int
                else
                {
                    return $"{parameter} {d:F1}";
                }
            }

            return value;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
