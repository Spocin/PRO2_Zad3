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
                ArrivalDate = new DateTime(2022, 1, 10, 14, 0, 0),
                DepartureDate = new DateTime(2022, 1, 12, 14, 0, 0),
            }, _repository.Object);

            //Assert
            Assert.That(result, Is.Empty);
        }
    }
}