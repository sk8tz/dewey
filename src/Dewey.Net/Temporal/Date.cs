using Dewey.Net.Types;
using System;

namespace Dewey.Net.Temporal
{
    public class Date
    {
        private DateTime _dateTime = DateTime.UtcNow.StripTime();

        public int Year
        {
            get
            {
                return _dateTime.Year;
            }
            set
            {
                _dateTime = new DateTime(value, _dateTime.Month, _dateTime.Day);
            }
        }

        public int Month
        {
            get
            {
                return _dateTime.Month;
            }
            set
            {
                _dateTime = new DateTime(_dateTime.Year, value, _dateTime.Day);
            }
        }

        public int Day
        {
            get
            {
                return _dateTime.Day;
            }
            set
            {
                _dateTime = new DateTime(_dateTime.Year, _dateTime.Month, value);
            }
        }

        public Date()
        {
        }

        public Date(int year, int month, int day)
        {
            if (year < 0) {
                throw new ArgumentException("Hour cannot be smaller than 0.");
            }

            if (month < 1) {
                throw new ArgumentException("Minute cannot be smaller than 1.");
            }

            if (day < 0) {
                throw new ArgumentException("Second cannot be smaller than 1.");
            }

            _dateTime = new DateTime(year, month, day);

            _dateTime.StripTime();
        }

        public Date(string date)
        {
            if (date.IsEmpty()) {
                throw new ArgumentNullException("The 'date' parameter cannot be null.");
            }

            date = date.ToLower();

            try {
                _dateTime = DateTime.ParseExact(date, "yyyy/MM/dd", System.Globalization.CultureInfo.CurrentCulture);
            } catch {
                try {
                    _dateTime = DateTime.ParseExact(date, "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentCulture);
                } catch {
                    try {
                        _dateTime = DateTime.ParseExact(date, "yyyy/MM/dd H:mm:ss", System.Globalization.CultureInfo.CurrentCulture);
                    } catch {
                        try {
                            _dateTime = DateTime.ParseExact(date, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);
                        } catch {
                            throw new ArgumentException("Date is not in a valid format.");
                        }
                    }
                }
            }

            _dateTime.StripTime();
        }

        public Date(string date, string format)
        {
            if (date.IsEmpty()) {
                throw new ArgumentNullException("The 'date' parameter cannot be null.");
            }

            date = date.ToLower();

            try {
                _dateTime = DateTime.ParseExact(date, format, System.Globalization.CultureInfo.CurrentCulture);
            } catch {
                throw new ArgumentException("Date is not in a valid format.");
            }
        }

        public Date(DateTime dateTime)
        {
            _dateTime = dateTime.StripTime();
        }

        public Date AddYears(int years)
        {
            _dateTime.AddYears(years);

            return this;
        }

        public Date AddMonths(int months)
        {
            _dateTime.AddMonths(months);

            return this;
        }

        public Date AddDays(int days)
        {
            _dateTime.AddDays(days);

            return this;
        }

        public DateTime ToDateTime()
        {
            return _dateTime;
        }

        public override string ToString()
        {
            return _dateTime.ToString("yyyy/MM/dd");
        }

        public string ToString(string format)
        {
            return _dateTime.ToString(format);
        }

        public static implicit operator Date(string date)
        {
            return new Date(date);
        }

        public static implicit operator Date(DateTime dateTime)
        {
            return new Date(dateTime);
        }
    }
}
