using System.Collections.Generic;

/// <summary>
/// Example of multiple indexers within a single class.
///
/// Why you might want/need to do this, who knows?
/// </summary>

namespace CSharp_Examples
{
    /// <summary>
    /// Test object.
    /// </summary>
    public class Person
    {
        public int EmployeeID { get; set; }
        public int CitizenID { get; set; }
        public string Name { get; set; }
        public Person(int empID, int citID, string name)
        {
            EmployeeID = empID;
            CitizenID = citID;
            Name = name;
        }
    }

    /// <summary>
    /// Interface for explicit indexer usage.
    /// </summary>
    public interface ICitizen
    {
        Person this[int index] { get; }
    }

    /// <summary>
    /// Interface for alternative explicit indexer.
    /// </summary>
    public interface IEmployee
    {
        Person this[int index] { get; }
    }

    /// <summary>
    /// Object with a list of Person objects.
    /// </summary>

    public class Indexers : ICitizen, IEmployee
    {
        private List<Person> people = new List<Person>();
        /// <summary>
        /// Standard indexer.
        /// </summary>
        /// <param name="index">Index in array of Person objects.</param>
        /// <returns></returns>
        public Person this[int index]
        {
            get
            {
                return people[index];
            }
        }

        /// <summary>
        /// Add a Person object to the array.
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public Person Add(Person person)
        {
            int idx;
            lock (people)
            {
                idx = people.FindIndex(p => p.EmployeeID == person.EmployeeID || p.CitizenID == person.CitizenID);
                if (idx >= 0)
                {
                    throw new ArgumentException("Duplicate employee ID or citizen ID", "person");
                }
                people.Add(person);
            }
            return person;
        }

        /// <summary>
        /// Indexer for the IEmployee interface. Gets an employee based on employee ID.
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        Person IEmployee.this[int employeeID]
        {
            get
            {
                return people.Find(p => p.EmployeeID == employeeID);
            }
        }

        /// <summary>
        /// Indexer for ICitizen interface. Gets an employee based on citizen ID.
        /// </summary>
        /// <param name="citizenID"></param>
        /// <returns></returns>
        Person ICitizen.this[int citizenID]
        {
            get
            {
                return people.Find(p => p.CitizenID == citizenID);
            }
        }
    }

    public class TestIndexers
    {
        public void Test()
        {
            Indexers persons = new Indexers();
            persons.Add(new Person(1, 301938912, "Bob Smith"));
            persons.Add(new Person(3, 920001233, "Jane Doe"));

            Person p;

            // Use ICitizen indexer to get person based on citizen ID.
            ICitizen c = persons;
            p = c[920001233];
            // Use IEmployee indexer to get person based on employee ID.
            IEmployee e = persons;
            p = e[1];
            // Returns null.
            p = ((IEmployee)persons)[2];
        }
    }
}