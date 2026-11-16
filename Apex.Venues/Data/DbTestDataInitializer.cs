namespace Apex.Venues.Data
{
    public class DbTestDataInitializer
    {
        private readonly VenuesDbContext _context;

        public DbTestDataInitializer(VenuesDbContext context)
        {
            _context = context;
        }

        public void Initialize()
        {
            // Ensures the database structure is created before we try to seed data 
            // _context.Database.EnsureCreated() is typically called in program.cs 
            // Look for any existing data. 

            if (_context.Venues.Any())
            {
                return; // DB is already seeded 
            }

            var venuesList = new List<Venue>() {
                new Venue { Code = "CRKHL", Name = "Crackling Hall", Description = "Once the residence of Lord and Lady Crackling, this lavish dwelling remains a prime example of 18th century fine living.", Capacity = 150 },
                new Venue { Code = "TNDMR", Name = "Tinder Manor", Description = "Refurbished manor house with fully equipped facilities ready to help you have a good time in business or pleasure.", Capacity = 450 },
                new Venue { Code = "FDLCK", Name = "The Fiddler's Cockatoo", Description = "Rustic pub set in ideallic countryside, the original venue of a notorious local musician and his parrot.", Capacity = 85 }
            };
            _context.AddRange(venuesList);
            _context.SaveChanges();

            var suitabilityList = new List<Suitability>() {
                new Suitability { VenueCode = "CRKHL", EventTypeId = "WED" },
                new Suitability { VenueCode = "CRKHL", EventTypeId = "CON" },
                new Suitability { VenueCode = "CRKHL", EventTypeId = "PTY" },
                new Suitability { VenueCode = "TNDMR", EventTypeId = "WED" },
                new Suitability { VenueCode = "TNDMR", EventTypeId = "CON" },
                new Suitability { VenueCode = "TNDMR", EventTypeId = "MET" },
                new Suitability { VenueCode = "FDLCK", EventTypeId = "WED" },
                new Suitability { VenueCode = "FDLCK", EventTypeId = "PTY" }
            };
            _context.AddRange(suitabilityList);
            _context.SaveChanges();

            var rand = new Random(0);

            var startDate = GetStartDate();
            var dates = new List<Availability>();
            var venues = new[] {
                new { Venue = "FDLCK", Cost = 30.0 },
                new { Venue = "CRKHL", Cost = 50.0 },
                new { Venue = "TNDMR", Cost = 70.0 }
            }.ToList();

            venues.ForEach(v =>
            {
                var more = Enumerable.Range(0, 90)
                    .Select(i => new Availability
                    {
                        VenueCode = v.Venue,
                        Date = startDate.AddDays(i),
                        CostPerHour = Math.Round(v.Cost * (1.0 + rand.NextDouble()), 2)
                    });
                dates.AddRange(more);
            });

            var availabilities = dates.Where(d => rand.NextDouble() < 0.3)
                                        .OrderBy(d => d.Date)
                                        .ToArray();

            _context.AddRange(availabilities);
            _context.SaveChanges();

        }

        protected DateTime GetStartDate(int addMonths = 2)
        {
            const int tagetJanDay = 2;

            DateTime startDate = DateTime.Now.AddMonths(addMonths);

            int targetYear = startDate.Year;

            if (startDate.Month == 12 && startDate.Day >= 22)
            {
                targetYear++;
                startDate = new DateTime(targetYear, 01, tagetJanDay);
            }
            if (startDate.Month == 01 && startDate.Day <= tagetJanDay)
            {
                startDate = new DateTime(targetYear, 01, tagetJanDay);
                if (startDate.DayOfWeek == DayOfWeek.Monday)    // Avoid first Monday in the year - normally a Bank Holiday
                {
                    startDate = startDate.AddDays(1);
                }
            }

            if (startDate.DayOfWeek == DayOfWeek.Saturday)
            {
                startDate = startDate.AddDays(2);
            }
            if (startDate.DayOfWeek == DayOfWeek.Sunday)
            {
                startDate = startDate.AddDays(1);
            }

            return startDate;
        }
    }
}
