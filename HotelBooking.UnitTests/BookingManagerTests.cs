using System;
using System.Collections;
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
        public static IEnumerable<object[]> GetData_StartDateNotInTheFuture_ThrowsArgumentException()
        {
            var data = new List<object[]>
            {
                new object[] { DateTime.Today, DateTime.Today },
                new object[] { DateTime.Today.AddDays(-5), DateTime.Today }
            };
            return data;
        }

        public static IEnumerable<object[]> GetData_StartDateLaterThanTheEndDate_ThrowsArgumentException()
        {
            var data = new List<object[]>
            {
                new object[] { DateTime.Today, DateTime.Today.AddDays(-5) },
                new object[] { DateTime.Today.AddDays(5), DateTime.Today }
            };
            return data;
        }

        public static IEnumerable<object[]> GetData_RoomAvailable_RoomIdNotMinusOne()
        {
            var data = new List<object[]>
            {
                new object[] { DateTime.Today.AddDays(1), DateTime.Today.AddDays(1) },
                new object[] { DateTime.Today.AddDays(5), DateTime.Today.AddDays(9) },
                new object[] { DateTime.Today.AddDays(21), DateTime.Today.AddDays(21) },
                new object[] { DateTime.Today.AddDays(22), DateTime.Today.AddDays(30) }
            };
            return data;
        }

        [Theory, MemberData(nameof(GetData_StartDateNotInTheFuture_ThrowsArgumentException))]
        public void FindAvailableRoom_StartDateNotInTheFuture_ThrowsArgumentException(DateTime start, DateTime end)
        {
            Assert.Throws<ArgumentException>(() => bookingManager.FindAvailableRoom(start, end));
        }

        [Theory, MemberData(nameof(GetData_StartDateNotInTheFuture_ThrowsArgumentException))]
        public void FindAvailableRoom_StartDateLaterThanTheEndDate_ThrowsArgumentException(DateTime start, DateTime end)
        {
            Assert.Throws<ArgumentException>(() => bookingManager.FindAvailableRoom(start, end));
        }

        [Theory, MemberData(nameof(GetData_RoomAvailable_RoomIdNotMinusOne))]
        public void FindAvailableRoom_RoomAvailable_RoomIdNotMinusOne(DateTime start, DateTime end)
        {
            // Act
            int roomId = bookingManager.FindAvailableRoom(start, end);
            // Assert
            Assert.NotEqual(-1, roomId);
        }

        [Theory]
        [MemberData(nameof(GetData_StartDateAndEndDateCorrectButOverlapsExistingBooking))]
        [MemberData(nameof(GetData_StartDateCorrectEndDateOverlapsExistingBooking))]
        [MemberData(nameof(GetData_StartDateOverlapsExistingBookingEndDateCorrect))]
        [MemberData(nameof(GetData_StartDateAndEndDateOverlapsExistingBooking))]
        public void FindAvailableRoom_RoomNotAvailable_RoomIdMinusOne(DateTime start, DateTime end)
        {
            int roomId = bookingManager.FindAvailableRoom(start, end);

            Assert.Equal(-1, roomId);
        }
    }
}
