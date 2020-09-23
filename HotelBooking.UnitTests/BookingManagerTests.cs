using System;
using System.Collections;
using System.Collections.Generic;
using HotelBooking.Core;
using HotelBooking.UnitTests.Fakes;
using HotelBooking.UnitTests.TestData;
using Moq;
using Xunit;

namespace HotelBooking.UnitTests
{
    public class BookingManagerTests
    {
        private IBookingManager bookingManager;

        private Mock<IRepository<Room>> fakeRoomRepository;
        private Mock<IRepository<Booking>> fakeBookingRepository;

        public BookingManagerTests(){
            DateTime start = DateTime.Today.AddDays(10);
            DateTime end = DateTime.Today.AddDays(20);

            // Set up mock room repository

            var rooms = new List<Room>
            {
                new Room { Id=1, Description="A" },
                new Room { Id=2, Description="B" },
            };

            fakeRoomRepository = new Mock<IRepository<Room>>();

            fakeRoomRepository.Setup(x => x.GetAll()).Returns(rooms);
            fakeRoomRepository.Setup(x => x.Get(It.IsInRange<int>(1, 2, Moq.Range.Inclusive))).Returns(rooms[0]);
            fakeRoomRepository.Setup(x => x.Remove(It.Is<int>(id => id < 1 || id > 2))).Throws<InvalidOperationException>();


            // Set up mock booking repository

            var bookings = new List<Booking>
            {
                new Booking {Id = 1, CustomerId = 1, IsActive = true, StartDate = start, EndDate = end, RoomId = 1},
                new Booking {Id = 2, CustomerId = 2, IsActive = true, StartDate = start, EndDate = end, RoomId = 2}
            };

            fakeBookingRepository = new Mock<IRepository<Booking>>();

            fakeBookingRepository.Setup(x => x.GetAll()).Returns(bookings);
            fakeBookingRepository.Setup(x => x.Get(It.IsInRange<int>(1, 2, Moq.Range.Exclusive))).Returns(bookings[0]);
            fakeBookingRepository.Setup(x => x.Remove(It.Is<int>(id => id < 1 || id > 2))).Throws<InvalidOperationException>();

            bookingManager = new BookingManager(fakeBookingRepository.Object, fakeRoomRepository.Object);
        }

        #region Tests for FindAvailableRoom

        [Theory, MemberData(nameof(TestDataProvider.GetData_StartDateNotInTheFuture), MemberType = typeof(TestDataProvider))]
        public void FindAvailableRoom_StartDateNotInTheFuture_ThrowsArgumentException(DateTime start, DateTime end)
        {
            Assert.Throws<ArgumentException>(() => bookingManager.FindAvailableRoom(start, end));
        }

        [Theory, MemberData(nameof(TestDataProvider.GetData_StartDateLaterThanEndDate), MemberType = typeof(TestDataProvider))]
        public void FindAvailableRoom_StartDateLaterThanTheEndDate_ThrowsArgumentException(DateTime start, DateTime end)
        {
            Assert.Throws<ArgumentException>(() => bookingManager.FindAvailableRoom(start, end));
        }

        [Theory, MemberData(nameof(TestDataProvider.GetData_RoomAvailable), MemberType = typeof(TestDataProvider))]
        public void FindAvailableRoom_RoomAvailable_RoomIdNotMinusOne(DateTime start, DateTime end)
        {
            // Act
            int roomId = bookingManager.FindAvailableRoom(start, end);
            // Assert
            Assert.NotEqual(-1, roomId);
        }

        [Theory]
        [MemberData(nameof(TestDataProvider.GetData_StartDateAndEndDateCorrectButOverlapsExistingBooking), MemberType = typeof(TestDataProvider))]
        [MemberData(nameof(TestDataProvider.GetData_StartDateCorrectEndDateOverlapsExistingBooking), MemberType = typeof(TestDataProvider))]
        [MemberData(nameof(TestDataProvider.GetData_StartDateOverlapsExistingBookingEndDateCorrect), MemberType = typeof(TestDataProvider))]
        [MemberData(nameof(TestDataProvider.GetData_StartDateAndEndDateOverlapsExistingBooking), MemberType = typeof(TestDataProvider))]
        public void FindAvailableRoom_RoomNotAvailable_RoomIdMinusOne(DateTime start, DateTime end)
        {
            int roomId = bookingManager.FindAvailableRoom(start, end);

            Assert.Equal(-1, roomId);
        }

        #endregion

        #region Tests for GetFullyOccupiedDates

        [Theory]
        [MemberData(nameof(TestDataProvider.GetData_StartDateLaterThanEndDate), MemberType = typeof(TestDataProvider))]
        public void GetFullyOccupideDates_StartDateLaterThanEndDate_ThrowsArgumentException(DateTime start, DateTime end)
        {
            Assert.Throws<ArgumentException>(() => bookingManager.GetFullyOccupiedDates(start, end));
        }

        #endregion
    }
}
