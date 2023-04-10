using System.Collections;

/// <summary>
/// Example of multiple enumerators within a class so that you can enumerate through values
/// hosted by the class in different ways. In this example, one enumerator allows you to go
/// forwards through a list, and another enumerator allows you to go backwards through the
/// list.
/// </summary>

namespace CSharp_Examples
{
    /// <summary>
    /// Interface so that we can explicitly declare a GetEnumerator method to
    /// allow enumerating backwards through a list.
    /// </summary>

    interface IEnumerableBackwards
    {
        IEnumerator GetEnumerator();
    }

    /// <summary>
    /// IEnumerable for standard enumeration operation.
    /// IEnumerableBackwards for alternative enumeration operation.
    /// </summary>
    public class MyList : IEnumerable, IEnumerableBackwards
    {
        private int[] list = new int[10];
        public int this[int idx]
        {
            get { return list[idx]; }
            set { list[idx] = value; }
        }
        public int Length { get { return list.Length; } }
        /// <summary>
        /// Standard enumerator.
        /// </summary>
        /// <returns>Instance of MyListEnumF to go forwards through list.</returns>
        public IEnumerator GetEnumerator()
        {
            return new MyListEnumF(this);
        }
        /// <summary>
        /// Alternative enumerator.
        /// To use this you must explicitly request it.
        /// </summary>
        /// <returns>Instance of MyListEnumB to go backwards through list.</returns>
        IEnumerator IEnumerableBackwards.GetEnumerator()
        {
            return new MyListEnumB(this);
        }
    }

    /// <summary>
    /// Enumerator to go forwards through the array.
    /// </summary>
    public class MyListEnumF : IEnumerator
    {
        private MyList owner;
        private int idx = -1;
        public MyListEnumF(MyList myList)
        {
            owner = myList;
            Reset();
        }
        public object Current
        {
            get
            {
                return owner[idx];
            }
        }
        public bool MoveNext()
        {
            return (++idx < owner.Length);
        }
        public void Reset()
        {
            idx = -1;
        }
    }

    /// <summary>
    /// Enumerator to go backwards through the array.
    /// </summary>
    public class MyListEnumB : IEnumerator
    {
        private MyList owner;
        private int idx;
        public MyListEnumB(MyList myList)
        {
            owner = myList;
            Reset();
        }
        public object Current
        {
            get
            {
                return owner[idx];
            }
        }
        public bool MoveNext()
        {
            return (--idx >= 0);
        }
        public void Reset()
        {
            idx = owner.Length;
        }
    }

    /// <summary>
    /// Test
    /// </summary>
    public class TestEnumerable
    {
        public void Test()
        {
            MyList list = new MyList();
            for (int i = 0; i < 10; i++)
            {
                list[i] = i * 3 + 3;
            }
            // Use standard GetEnumerator to go through the list.
            foreach (var a in list)
            {
                Console.WriteLine(a);
            }
            // Explicitly use the alternative Enumerator to go through the list backwards.
            foreach (var a in (IEnumerableBackwards)list)
            {
                Console.WriteLine(a);
            }
        }
    }
}