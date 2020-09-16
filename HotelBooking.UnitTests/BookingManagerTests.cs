using System;
using System.Collections.Generic;
using HotelBooking.Core;
using HotelBooking.UnitTests.Fakes;
using Xunit;

namespace HotelBooking.UnitTests
{
    public class BookingManagerTests
    {
        private IBookingManager bookingManager;

        public BookingManagerTests(){
            DateTime start = DateTime.Today.AddDays(10);
            DateTime end = DateTime.Today.AddDays(20);
            IRepository<Booking> bookingRepository = new FakeBookingRepository(start, end);
            IRepository<Room> roomRepository = new FakeRoomRepository();
            bookingManager = new BookingManager(bookingRepository, roomRepository);
        }

        public static IEnumerable<object[]> GetData_StartDateAndEndDateCorrectButOverlapsExistingBooking()
        {
            var data = new List<object[]>
            {
                new object[] {DateTime.Today.AddDays(1), DateTime.Today.AddDays(30)},
                new object[] {DateTime.Today.AddDays(2), DateTime.Today.AddDays(22)},
                new object[] {DateTime.Today.AddDays(9), DateTime.Today.AddDays(41)},
                new object[] {DateTime.Today.AddDays(5), DateTime.Today.AddDays(25)},
                new object[] {DateTime.Today.AddDays(8), DateTime.Today.AddDays(25)}
            };

            return data;
        }

        public static IEnumerable<object[]> GetData_StartDateCorrectEndDateOverlapsExistingBooking()
        {
            var data = new List<object[]>
            {
                new object[] {DateTime.Today.AddDays(1), DateTime.Today.AddDays(11)},
                new object[] {DateTime.Today.AddDays(2), DateTime.Today.AddDays(12)},
                new object[] {DateTime.Today.AddDays(3), DateTime.Today.AddDays(13)},
                new object[] {DateTime.Today.AddDays(8), DateTime.Today.AddDays(18)},
                new object[] {DateTime.Today.AddDays(9), DateTime.Today.AddDays(20)},
            };

            return data;
        }

        public static IEnumerable<object[]> GetData_StartDateOverlapsExistingBookingEndDateCorrect()
        {
            var data = new List<object[]>
            {
                new object[] {DateTime.Today.AddDays(11), DateTime.Today.AddDays(21)},
                new object[] {DateTime.Today.AddDays(12), DateTime.Today.AddDays(22)},
                new object[] {DateTime.Today.AddDays(13), DateTime.Today.AddDays(23)},
                new object[] {DateTime.Today.AddDays(18), DateTime.Today.AddDays(40)},
                new object[] {DateTime.Today.AddDays(19), DateTime.Today.AddDays(50)}
            };

            return data;
        }

        public static IEnumerable<object[]> GetData_StartDateAndEndDateOverlapsExistingBooking()
        {
            var data = new List<object[]>
            {
                new object[] {DateTime.Today.AddDays(10), DateTime.Today.AddDays(11)},
                new object[] {DateTime.Today.AddDays(11), DateTime.Today.AddDays(12)},
                new object[] {DateTime.Today.AddDays(12), DateTime.Today.AddDays(14)},
                new object[] {DateTime.Today.AddDays(19), DateTime.Today.AddDays(19)},
                new object[] {DateTime.Today.AddDays(20), DateTime.Today.AddDays(20)}
            };

            return data;
        }

        [Fact]
        public void FindAvailableRoom_StartDateNotInTheFuture_ThrowsArgumentException()
        {
            DateTime date = DateTime.Today;
            Assert.Throws<ArgumentException>(() => bookingManager.FindAvailableRoom(date, date));
        }

        [Fact]
        public void FindAvailableRoom_RoomAvailable_RoomIdNotMinusOne()
        {
            // Arrange
            DateTime date = DateTime.Today.AddDays(1);
            // Act
            int roomId = bookingManager.FindAvailableRoom(date, date);
            // Assert
            Assert.NotEqual(-1, roomId);
        }

        [Theory]
        [MemberData(nameof(GetData_StartDateAndEndDateCorrectButOverlapsExistingBooking))]
        public void FindAvailableRoom_StartDateAndEndDateCorrectButOverlapsExistingBooking_RoomIDMinusOne(DateTime start, DateTime end)
        {
            int roomId = bookingManager.FindAvailableRoom(start, end);

            Assert.Equal(-1, roomId);
        }

        [Theory]
        [MemberData(nameof(GetData_StartDateCorrectEndDateOverlapsExistingBooking))]
        public void FindAvailableRoom_StartDateCorrectEndDateOverlapsExistingBooking_RoomIDMinusOne(DateTime start, DateTime end)
        {
            int roomId = bookingManager.FindAvailableRoom(start, end);

            Assert.Equal(-1, roomId);
        }

        [Theory]
        [MemberData(nameof(GetData_StartDateOverlapsExistingBookingEndDateCorrect))]
        public void FindAvailableRoom_StartDateOverlapsExistingBookingEndDateCorrect_RoomIDMinusOne(DateTime start, DateTime end)
        {
            int roomId = bookingManager.FindAvailableRoom(start, end);

            Assert.Equal(-1, roomId);
        }

        [Theory]
        [MemberData(nameof(GetData_StartDateAndEndDateOverlapsExistingBooking))]
        public void FindAvailableRoom_StartDateAndEndDateOverlapsExistingBooking_RoomIDMinusOne(DateTime start, DateTime end)
        {
            int roomId = bookingManager.FindAvailableRoom(start, end);

            Assert.Equal(-1, roomId);
        }
    }
}
