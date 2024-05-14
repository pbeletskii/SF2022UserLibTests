using SF2022UserLib;
namespace SF2022UserLibTests
{
    [TestClass]
    public class CalculationsTests
    {
        [TestMethod]
        public void AvailablePeriods_StartTimesNull_ThrowsArgumentNullException()
        {
            // Arrange
            Calculations calculations = new Calculations();

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
                calculations.AvailablePeriods(null, Array.Empty<int>(), TimeSpan.FromHours(9), TimeSpan.FromHours(17), 30));
        }

        [TestMethod]
        public void AvailablePeriods_DurationsNull_ThrowsArgumentNullException()
        {
            // Arrange
            Calculations calculations = new Calculations();

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
                calculations.AvailablePeriods(Array.Empty<TimeSpan>(), null, TimeSpan.FromHours(9), TimeSpan.FromHours(17), 30));
        }

        [TestMethod]
        public void AvailablePeriods_StartTimesDurationsLengthMismatch_ThrowsArrayMismatchException()
        {
            // Arrange
            Calculations calculations = new Calculations();

            // Act & Assert
            Assert.ThrowsException<ArrayMismatchException>(() =>
                calculations.AvailablePeriods([TimeSpan.FromHours(9)], Array.Empty<int>(), TimeSpan.FromHours(9), TimeSpan.FromHours(17), 30));
        }

        [TestMethod]
        public void AvailablePeriods_ConsultationTimeZero_ThrowsArgumentException()
        {
            // Arrange
            Calculations calculations = new Calculations();

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() =>
                calculations.AvailablePeriods([], Array.Empty<int>(), TimeSpan.FromHours(9), TimeSpan.FromHours(17), 0));
        }

        [TestMethod]
        public void AvailablePeriods_BeginWorkingTimeAfterEndWorkingTime_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            Calculations calculations = new Calculations();

            // Act & Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                calculations.AvailablePeriods(Array.Empty<TimeSpan>(), Array.Empty<int>(), TimeSpan.FromHours(18), TimeSpan.FromHours(9), 30));
        }

        [TestMethod]
        public void AvailablePeriods_StartTimesOutOfRange_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            Calculations calculations = new Calculations();

            // Act & Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                calculations.AvailablePeriods([TimeSpan.FromHours(8)], [30], TimeSpan.FromHours(9), TimeSpan.FromHours(17), 30));
        }

        [TestMethod]
        public void AvailablePeriods_NoExistingAppointments_ReturnsAllAvailablePeriods()
        {
            // Arrange
            Calculations calculations = new Calculations();

            // Act
            var result = calculations.AvailablePeriods(Array.Empty<TimeSpan>(), Array.Empty<int>(), TimeSpan.FromHours(9), TimeSpan.FromHours(17), 30);

            // Assert
            CollectionAssert.AreEqual(new[] { "09:00-09:30", "09:30-10:00", "10:00-10:30", "10:30-11:00", "11:00-11:30", "11:30-12:00", "12:00-12:30", "12:30-13:00", "13:00-13:30", "13:30-14:00", "14:00-14:30", "14:30-15:00", "15:00-15:30", "15:30-16:00", "16:00-16:30", "16:30-17:00" }, result);
        }

        [TestMethod]
        public void AvailablePeriods_NoInput_ReturnsAllPeriodsInRange()
        {
            // Arrange
            Calculations calculations = new Calculations();
            TimeSpan[] startTimes = Array.Empty<TimeSpan>();
            int[] durations = Array.Empty<int>();
            TimeSpan beginWorkingTime = TimeSpan.FromHours(9);
            TimeSpan endWorkingTime = TimeSpan.FromHours(17);
            int consultationTime = 30;

            // Act
            var result = calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            // Assert
            CollectionAssert.AreEqual(new[] { "09:00-09:30", "09:30-10:00", "10:00-10:30", "10:30-11:00", "11:00-11:30", "11:30-12:00", "12:00-12:30", "12:30-13:00", "13:00-13:30", "13:30-14:00", "14:00-14:30", "14:30-15:00", "15:00-15:30", "15:30-16:00", "16:00-16:30", "16:30-17:00" }, result);
        }

        [TestMethod]
        public void AvailablePeriods_WithAppointments_ReturnsAvailablePeriods()
        {
            // Arrange
            Calculations calculations = new Calculations();
            TimeSpan[] startTimes = { TimeSpan.FromHours(10), TimeSpan.FromHours(13) };
            int[] durations = { 30, 60 };
            TimeSpan beginWorkingTime = TimeSpan.FromHours(9);
            TimeSpan endWorkingTime = TimeSpan.FromHours(17);
            int consultationTime = 30;

            // Act
            var result = calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            // Assert
            CollectionAssert.AreNotEqual(new[] { "09:00-09:30", "09:30-10:00", "12:00-12:30", "12:30-13:00", "14:00-14:30", "14:30-15:00", "15:00-15:30", "15:30-16:00", "16:00-16:30", "16:30-17:00" }, result);
        }

        [TestMethod]
        public void AvailablePeriods_AllAppointmentsDuringWorkingHours_ReturnsNoAvailablePeriods()
        {
            // Arrange
            Calculations calculations = new Calculations();

            // Act
            var result = calculations.AvailablePeriods([TimeSpan.FromHours(9), TimeSpan.FromHours(17)], [480, 480], TimeSpan.FromHours(9), TimeSpan.FromHours(17), 30);

            // Assert
            CollectionAssert.AreEqual(Array.Empty<string>(), result);
        }
    }
}