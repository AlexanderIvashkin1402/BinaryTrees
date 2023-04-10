namespace UnionFind
{
    public class UnionFind
    {
        // The number of elements in this union find
        private int _size;

        // Used to track the size of each of the component
        private int[] _sz;

        // id[i] points to the parent of i, if id[i] = i then i is a root node
        private int[] _id;

        // Tracks the number of components in the union find
        private int _numComponents;

        public UnionFind(int size)
        {

            if (size <= 0) throw new ArgumentException("Size <= 0 is not allowed");

            _size = _numComponents = size;
            _sz = new int[size];
            _id = new int[size];

            for (int i = 0; i < size; i++)
            {
                _id[i] = i; // Link to itself (self root)
                _sz[i] = 1; // Each component is originally of size one
            }
        }

        // Find which component/set 'p' belongs to, takes amortized constant time.
        public int Find(int p, bool compress = true)
        {

            // Find the root of the component/set
            int root = p;
            while (root != _id[root]) root = _id[root];

            // Compress the path leading back to the root.
            // Doing this operation is called "path compression"
            // and is what gives us amortized time complexity.
            if (compress)
            {
                while (p != root)
                {
                    int next = _id[p];
                    _id[p] = root;
                    p = next;
                }
            }

            return root;
        }

        // This is an alternative recursive formulation for the find method
        // public int find(int p) {
        //   if (p == id[p]) return p;
        //   return id[p] = find(id[p]);
        // }

        // Return whether or not the elements 'p' and
        // 'q' are in the same components/set.
        public bool Connected(int p, int q)
        {
            return Find(p) == Find(q);
        }

        // Return the size of the components/set 'p' belongs to
        public int ComponentSize(int p)
        {
            return _sz[Find(p)];
        }

        // Return the number of elements in this UnionFind/Disjoint set
        public int Size  => _size;

        // Returns the number of remaining components/sets
        public int Components => _numComponents;

        // Unify the components/sets containing elements 'p' and 'q'
        public bool Unify(int p, int q, bool compress = true)
        {

            // These elements are already in the same group!
            if (Connected(p, q)) return false;

            int root1 = Find(p, compress);
            int root2 = Find(q, compress);

            // Merge smaller component/set into the larger one.
            if (_sz[root1] < _sz[root2])
            {
                _sz[root2] += _sz[root1];
                _id[root1] = root2;
                _sz[root1] = 0;
            }
            else
            {
                _sz[root1] += _sz[root2];
                _id[root2] = root1;
                _sz[root2] = 0;
            }

            // Since the roots found are different we know that the
            // number of components/sets has decreased by one
            _numComponents--;

            return true;
        }
    }
}

