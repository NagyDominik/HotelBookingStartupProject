using System;
using System.Collections.Generic;
using System.Text;

namespace HotelBooking.UnitTests.TestData
{
    public class TestDataProvider
    {
        #region Member data for FindAvailableRoom() tests
        public static IEnumerable<object[]> GetData_StartDateAndEndDateCorrectButOverlapsExistingBooking()
        {
            var data = new List<object[]>
            {
                new object[] {DateTime.Today.AddDays(1), DateTime.Today.AddDays(30)},
                new object[] {DateTime.Today.AddDays(2), DateTime.Today.AddDays(22)},
                new object[] { DateTime.Today.AddDays(9), DateTime.Today.AddDays(41)},
                new object[] { DateTime.Today.AddDays(5), DateTime.Today.AddDays(25)},
                new object[] { DateTime.Today.AddDays(8), DateTime.Today.AddDays(25)}
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

        public static IEnumerable<object[]> GetData_StartDateNotInTheFuture()
        {
            var data = new List<object[]>
            {
                new object[] { DateTime.Today, DateTime.Today },
                new object[] { DateTime.Today.AddDays(-5), DateTime.Today }
            };
            return data;
        }

        public static IEnumerable<object[]> GetData_StartDateLaterThanTheEndDate()
        {
            var data = new List<object[]>
            {
                new object[] { DateTime.Today, DateTime.Today.AddDays(-5) },
                new object[] { DateTime.Today.AddDays(5), DateTime.Today }
            };
            return data;
        }

        public static IEnumerable<object[]> GetData_RoomAvailable()
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

        #endregion

        #region GetFullyOccupiedDates() tests

        public static IEnumerable<object[]> GetData_StartDateLaterThanEndDate()
        {
            var data = new List<object[]>
            {
                new object[] {DateTime.Today.AddDays(5), DateTime.Today.AddDays(2)},
                new object[] {DateTime.Today.AddDays(4), DateTime.Today.AddDays(2)},
                new object[] {DateTime.Today.AddDays(2), DateTime.Today.AddDays(1)},
                new object[] {DateTime.Today.AddDays(10), DateTime.Today.AddDays(9)},
                new object[] {DateTime.Today.AddDays(11), DateTime.Today.AddDays(8)},
            };

            return data;
        }

        #endregion
    }
}
