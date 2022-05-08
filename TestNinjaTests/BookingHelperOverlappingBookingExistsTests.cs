using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using TestNinja;

namespace TestNinjaTests
{
    public class BookingHelperOverlappingBookingExistsTests
    {
        private Booking _existingBooking;
        private Mock<IBookingRepository> _repository;

        [SetUp]
        public void SetUp()
        {
            //Arrange
            _existingBooking = new Booking()
            {
                Id = 2,
                ArrivalDate = new DateTime(2022, 1, 15, 14, 0, 0),
                DepartureDate = new DateTime(2022, 1, 20, 14, 0, 0),
                Reference = "a"
            };

            var existingBookings = new List<Booking> {_existingBooking}.AsQueryable();

            _repository = new Mock<IBookingRepository>();
            _repository.Setup(repo => repo.GetActiveBookings(1))
                .Returns(existingBookings);
        }

        [Test]
        public void BookingStartAndFinishedBeforeExistingBooking_ReturnEmptyString()
        {
            //Act
            var result = BookingHelper.OverlappingBookingsExist(new Booking
            {
                Id = 1,
                ArrivalDate = Before(_existingBooking.ArrivalDate, 10),
                DepartureDate = Before(_existingBooking.ArrivalDate, 1)
            }, _repository.Object);

            //Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void BookingStartsBeforeFinishedInTheMiddleOfAnExistingListingBooking_ReturnEmptyString()
        {
            //Act
            var result = BookingHelper.OverlappingBookingsExist(new Booking
            {
                Id = 1,
                ArrivalDate = Before(_existingBooking.ArrivalDate, 10),
                DepartureDate = After(_existingBooking.ArrivalDate, 1)
            }, _repository.Object);

            //Assert
            Assert.That(result, Is.EqualTo(_existingBooking.Reference));
        }
 
        private DateTime ArriveOn(int year, int month, int day)
        {
            return new DateTime(year, month, day, 14, 0, 0);
        }

        private DateTime DepartOn(int year, int month, int day)
        {
            return new DateTime(year, month, day, 14, 0, 0);
        }

        private DateTime Before(DateTime dateTime, int days = 1)
        {
            return dateTime.AddDays(-days);
        }

        private DateTime After(DateTime dateTime, int days = 1)
        {
            return dateTime.AddDays(days);
        }
    }
}