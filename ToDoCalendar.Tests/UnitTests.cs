using Moq.EntityFrameworkCore;
using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;

namespace ToDoCalendar.Tests
{
    public class UnitTests
    {
        private Mock<DbSet<Date>> _mockDatesDbSet;
        private Mock<DbSet<Activity>> _mockActivitiesDbSet;
        private Mock<CalendarContext> _mockContext;
        private CalendarContext _context;

        [SetUp]
        public void Setup()
        {
            var dates = TestUtils.getDateList();
            var activities = TestUtils.getActivityList();

            _mockDatesDbSet = new Mock<DbSet<Date>>();
            _mockDatesDbSet.As<IQueryable<Date>>().Setup(m => m.Provider).Returns(dates.AsQueryable().Provider);
            _mockDatesDbSet.As<IQueryable<Date>>().Setup(m => m.Expression).Returns(dates.AsQueryable().Expression);
            _mockDatesDbSet.As<IQueryable<Date>>().Setup(m => m.ElementType).Returns(dates.AsQueryable().ElementType);
            _mockDatesDbSet.As<IQueryable<Date>>().Setup(m => m.GetEnumerator()).Returns(dates.GetEnumerator());
            _mockDatesDbSet.Setup(m => m.Add(It.IsAny<Date>())).Callback<Date>(dates.Add);

            _mockActivitiesDbSet = new Mock<DbSet<Activity>>();
            _mockActivitiesDbSet.As<IQueryable<Activity>>().Setup(m => m.Provider).Returns(activities.AsQueryable().Provider);
            _mockActivitiesDbSet.As<IQueryable<Activity>>().Setup(m => m.Expression).Returns(activities.AsQueryable().Expression);
            _mockActivitiesDbSet.As<IQueryable<Activity>>().Setup(m => m.ElementType).Returns(activities.AsQueryable().ElementType);
            _mockActivitiesDbSet.As<IQueryable<Activity>>().Setup(m => m.GetEnumerator()).Returns(activities.GetEnumerator());

            _mockContext = new Mock<CalendarContext>();
            _mockContext.SetupGet(c => c.Dates).Returns(_mockDatesDbSet.Object);
            _mockContext.SetupGet(c => c.Activities).Returns(_mockActivitiesDbSet.Object);

            _context = _mockContext.Object;
        }

        [Test]
        public void TestIsActivityValid()
        {
            // Arrange
            string invalidName = "";
            string validTime = "12:45";

            // Act
            var validationResult = Utils.Utils.isActivityValid(invalidName, validTime);

            // Assert
            Assert.False(validationResult);
        }

        [Test]
        public void TestIconForActivityDone()
        {
            // Arrange
            var mockActivity = new Activity()
            {
                Done = true
            };

            // Act
            var icon = Utils.Utils.getIconForActivity(mockActivity);

            // Assert
            Assert.True(icon == FontAwesome.WPF.FontAwesomeIcon.CheckCircle);
        }

        [Test]
        public void GetDates_ReturnsDates()
        {
            // Arrange
            var expectedDatesCount = TestUtils.getDateList().Count;

            // Act
            var dates = _context.Dates;

            // Assert
            Assert.AreEqual(expectedDatesCount, dates.Count());
        }

        [Test]
        public void GetActivities_ReturnsActivities()
        {
            // Arrange
            var expectedActivityCount = TestUtils.getActivityList().Count;

            // Act
            var activities = _context.Activities;

            // Assert
            Assert.AreEqual(expectedActivityCount, activities.Count());
        }

        [Test]
        public void GetActivitiesOnDate_ReturnsCorrectActivities()
        {
            // Arrange
            var expectedActivitiesOnDateCount = TestUtils.getActivityList().Count;

            // Act
            var activities = _context.Dates.Where(a => a.ID == 2).First();

            // Assert
            Assert.AreEqual(expectedActivitiesOnDateCount, activities.Activities.Count());
        }

        [Test]
        public void AddDate_CallsAddOnCollection()
        {
            // Act
            _context.Dates.Add(new Date() {Day = DateTime.Now,  Activities = new List<Activity>()});

            // Assert
            _mockDatesDbSet.Verify(m => m.Add(It.IsAny<Date>()), Times.Once);
        }

        class TestUtils
        {
            public static List<Date> getDateList()
            {
                return new List<Date>
                    {
                        new Date()
                        {
                            ID = 1,
                            Day = DateTime.Today.AddDays(2),
                            Activities = new Collection<Activity>
                            {
                                new Activity()
                                {
                                    ID = 3,
                                    Name = "Bieg",
                                    StartTime = "13:00",
                                    Done = true,
                                    DateID = 1
                                }
                            }
                        },
                        new Date()
                        {
                            ID = 2,
                            Day = DateTime.Now,
                            Activities = getActivityList()
                        }
                    };
            }

            public static List<Activity> getActivityList()
            {
                return new List<Activity>
                    {
                        new Activity()
                        {
                            ID = 1,
                            Name = "Bieg",
                            StartTime ="10:00",
                            Done = false,
                            DateID = 2
                        },
                        new Activity()
                        {
                            ID = 2,
                            Name = "Pływanie",
                            StartTime = "19:30",
                            Done = false,
                            DateID = 2
                        },
                        new Activity()
                        {
                            ID = 4,
                            Name = "Skakanie",
                            StartTime = "17:30",
                            Done = true,
                            DateID = 2
                        },
                        new Activity()
                        {
                            ID = 4,
                            Name = "Jeżdzenie",
                            StartTime = "7:15",
                            Done = false,
                            DateID = 2
                        }
                    };
            }
        }
    }
}
