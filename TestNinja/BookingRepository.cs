using System.Linq;

namespace TestNinja
{
    public interface IBookingRepository
    {
        IQueryable<Booking> GetActiveBookings(int? excludedBookingId = null);
    }
    
    public class BookingRepository : IBookingRepository
    {
        public IQueryable<Booking> GetActiveBookings(int? excludedBookingId = null)
        {
            var uniOfWork = new UnitOfWork();
            var bookings =
                uniOfWork.Query<Booking>()
                    .Where(b =>
                        b.Status != "Cancelled"
                    );

            if (excludedBookingId.HasValue)
            {
                bookings = bookings
                    .Where(b => 
                        b.Id != excludedBookingId.Value
                    );
            }

            return bookings;
        }
    }
}