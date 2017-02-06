using System;
using System.Data.SQLite;
using PocoUtils;

namespace PocoUtilsTest
{
    using NUnit.Framework;

    [TestFixture]
    // ReSharper disable once InconsistentNaming
    class TestPocoDBUtils
    {
        private static readonly String MEMORY_DB_CONNECTION_STRING = "Data Source=:memory:;Version=3;New=True;";
        private static readonly String SQL_CREATE_DDL = @"CREATE TABLE names
                   (pri_key INTEGER NOT NULL,
                   SSN TEXT,
                   VisitorFirstName TEXT,
                   MiddleName TEXT,
                   VisitorLastName TEXT,
                   Age INT,
                   PRIMARY KEY (pri_key),
                   UNIQUE (SSN));

                   INSERT INTO names (SSN, VisitorFirstName, MiddleName, VisitorLastName, Age) VALUES ('NotRealSSN', 'Billy', 'Bob', 'Thornton', 55);
                   INSERT INTO names (SSN, VisitorFirstName, MiddleName, VisitorLastName, Age) VALUES ('SillySSN', 'Jane', 'Silly', 'Anne', 32);
                   INSERT INTO names (SSN, VisitorFirstName, MiddleName, VisitorLastName, Age) VALUES ('GrayOne', 'Graham', 'Bleak', 'Grayson', 81);

                   ";

        private static readonly String SQL_SELECT_ALL = "SELECT * FROM names";

        [Test]
        public void DoesStuff()
        {
            // Open a SQLLite connection and create an in-memory table with some data
            using (SQLiteConnection connection = new SQLiteConnection(MEMORY_DB_CONNECTION_STRING))
            {
                connection.Open();

                SQLiteCommand createDataCmd = new SQLiteCommand(SQL_CREATE_DDL, connection);
                createDataCmd.ExecuteNonQuery();
                SQLiteCommand selectCommand = new SQLiteCommand(SQL_SELECT_ALL, connection);
                using (SQLiteDataReader dataReader = selectCommand.ExecuteReader())
                {
                    // Create the POCO (plain old C# object)
                    Person person = new Person();

                    // Read first record
                    Assert.True(dataReader.Read());

                    // Test copying all fields from the data reader to the POCO
                    PocoDBUtils.CopyFields(person, dataReader);

                    // Assert the results
                    Assert.AreEqual("Billy", person.FirstName);
                    Assert.AreEqual("Thornton", person.LastName);
                    Assert.AreEqual(55, person.Age);
                    Assert.AreEqual("55", person.StringAge);
                }
            }
        }
    }
}
