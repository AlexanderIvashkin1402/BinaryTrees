using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace BBST
{
    public class AVLTreeRecursive<T> : IEnumerable<T>, IPreorderIEnumerator<T> where T : IComparable
    //public class AVLTreeRecursive<T> where T : IComparable
    {
        private ICustomIEnumerator<T> _postOrderIEnumerator;
        private ICustomIEnumerator<T> _levelOrderIEnumerator;

        public AVLTreeRecursive()
        {
            _postOrderIEnumerator = new PostOrderIEnumerator<T>(this);
            _levelOrderIEnumerator = new LevelOrderIEnumerator<T>(this);
        }
        
        //public class Node implements PrintableNode
        //  {

        //  // 'bf' is short for Balance Factor
        //  public int bf;

        //  // The value/data contained within the node.
        //  public T value;

        //  // The height of this node in the tree.
        //  public int height;

        //  // The left and the right children of this node.
        //  public Node left, right;

        //  public Node(T value)
        //  {
        //      this.value = value;
        //  }

        //  @Override
        //  public PrintableNode getLeft()
        //  {
        //      return left;
        //  }

        //  @Override
        //  public PrintableNode getRight()
        //  {
        //      return right;
        //  }

        //  @Override
        //  public String getText()
        //  {
        //      return value.toString();
        //  }
        //}

        // The root node of the AVL tree.
        public BinaryTreeNode<T> Root { get; set; }

        // Tracks the number of nodes inside the tree.
        private int nodeCount = 0;

        // The height of a rooted tree is the number of edges between the tree's
        // root and its furthest leaf. This means that a tree containing a single
        // node has a height of 0.
        public int Height()
        {
            if (Root == null) return 0;
            return Root.Height;
        }

        // Returns the number of nodes in the tree.
        public int Size()
        {
            return nodeCount;
        }

        // Returns whether or not the tree is empty.
        public bool IsEmpty()
        {
            return Size() == 0;
        }

        // Return true/false depending on whether a value exists in the tree.
        public bool Contains(T value)
        {
            return Contains(Root, value);
        }

        // Recursive contains helper method.
        private bool Contains(BinaryTreeNode<T> node, T value)
        {
            if (node == null) return false;

            // Compare current value to the value in the node.
            int cmp = value.CompareTo(node.Data);

            // Dig into left subtree.
            if (cmp < 0) return Contains(node.LeftNode, value);

            // Dig into right subtree.
            if (cmp > 0) return Contains(node.RightNode, value);

            // Found value in tree.
            return true;
        }

        // Insert/add a value to the AVL tree. The value must not be null, O(log(n))
        public bool Insert(T value)
        {
            if (value == null) return false;
            if (!Contains(Root, value))
            {
                Root = Insert(Root, value);
                nodeCount++;
                return true;
            }
            return false;
        }

        // Inserts a value inside the AVL tree.
        private BinaryTreeNode<T> Insert(BinaryTreeNode<T> node, T value)
        {
            // Base case.
            if (node == null) return new BinaryTreeNode<T>(value);

            // Compare current value to the value in the node.
            int cmp = value.CompareTo(node.Data);
        
            if (cmp < 0)
            {
                // Insert node in left subtree.
                node.LeftNode = Insert(node.LeftNode, value);
            }
            else
            {
                // Insert node in right subtree.
                node.RightNode = Insert(node.RightNode, value);
            }

            // Update balance factor and height values.
            Update(node);

            // Re-balance tree.
            return Balance(node);
        }

        // Update a node's height and balance factor.
        private void Update(BinaryTreeNode<T> node)
        {
            int leftNodeHeight = (node.LeftNode == null) ? -1 : node.LeftNode.Height;
            int rightNodeHeight = (node.RightNode == null) ? -1 : node.RightNode.Height;

            // Update this node's height.
            node.Height = 1 + Math.Max(leftNodeHeight, rightNodeHeight);

            // Update balance factor.
            node.Bf = rightNodeHeight - leftNodeHeight;
        }

        // Re-balance a node if its balance factor is +2 or -2.
        private BinaryTreeNode<T> Balance(BinaryTreeNode<T> node)
        {
            // Left heavy subtree.
            if (node.Bf <= -2)
            {
                if (node.LeftNode.Bf <= 0)
                {
                    // Left-Left case.
                    return LeftLeftCase(node);
                }
                else
                {
                    // Left-Right case.
                    return LeftRightCase(node);
                }

                // Right heavy subtree needs balancing.
            }
            else if (node.Bf >= 2)
            {

                // Right-Right case.
                if (node.RightNode.Bf >= 0)
                {
                    return RightRightCase(node);

                    // Right-Left case.
                }
                else
                {
                    return RightLeftCase(node);
                }
            }

            // Node either has a balance factor of 0, +1 or -1 which is fine.
            return node;
        }

        private BinaryTreeNode<T> LeftLeftCase(BinaryTreeNode<T> node)
        {
            return RightRotation(node);
        }

        private BinaryTreeNode<T> LeftRightCase(BinaryTreeNode<T> node)
        {
            node.LeftNode = LeftRotation(node.LeftNode);
            return LeftLeftCase(node);
        }

        private BinaryTreeNode<T> RightRightCase(BinaryTreeNode<T> node)
        {
            return LeftRotation(node);
        }

        private BinaryTreeNode<T> RightLeftCase(BinaryTreeNode<T> node)
        {
            node.RightNode = RightRotation(node.RightNode);
            return RightRightCase(node);
        }

        private BinaryTreeNode<T> LeftRotation(BinaryTreeNode<T> node)
        {
            BinaryTreeNode<T> newParent = node.RightNode;
            if (node != Root)
                node.RightNode = newParent.LeftNode;
            else
                node.RightNode = null;
            newParent.LeftNode = node;
            Update(node);
            Update(newParent);
            return newParent;
        }

        private BinaryTreeNode<T> RightRotation(BinaryTreeNode<T> node)
        {
            BinaryTreeNode<T> newParent = node.LeftNode;
            if (node != Root)
                node.LeftNode = newParent.LeftNode;
            else
                node.LeftNode = null;
            newParent.RightNode = node;
            Update(node);
            Update(newParent);
            return newParent;
        }

        // Remove a value from this binary tree if it exists, O(log(n))
        public bool Remove(T elem)
        {
            if (elem == null) return false;

            if (Contains(Root, elem))
            {
                Root = Remove(Root, elem);
                nodeCount--;
                return true;
            }

            return false;
        }

        // Removes a value from the AVL tree.
        private BinaryTreeNode<T> Remove(BinaryTreeNode<T> node, T elem)
        {
            if (node == null) return null;

            int cmp = elem.CompareTo(node.Data);

            // Dig into left subtree, the value we're looking
            // for is smaller than the current value.
            if (cmp < 0)
            {
                node.LeftNode = Remove(node.LeftNode, elem);

                // Dig into right subtree, the value we're looking
                // for is greater than the current value.
            }
            else if (cmp > 0)
            {
                node.RightNode = Remove(node.RightNode, elem);

                // Found the node we wish to remove.
            }
            else
            {

                // This is the case with only a right subtree or no subtree at all.
                // In this situation just swap the node we wish to remove
                // with its right child.
                if (node.LeftNode == null)
                {
                    return node.RightNode;

                    // This is the case with only a left subtree or
                    // no subtree at all. In this situation just
                    // swap the node we wish to remove with its left child.
                }
                else if (node.RightNode == null)
                {
                    return node.LeftNode;

                    // When removing a node from a binary tree with two links the
                    // successor of the node being removed can either be the largest
                    // value in the left subtree or the smallest value in the right
                    // subtree. As a heuristic, I will remove from the subtree with
                    // the greatest height in hopes that this may help with balancing.
                }
                else
                {

                    // Choose to remove from left subtree
                    if (node.LeftNode.Height > node.RightNode.Height)
                    {

                        // Swap the value of the successor into the node.
                        T successorValue = FindMax(node.LeftNode);
                        node.Data = successorValue;

                        // Find the largest node in the left subtree.
                        node.LeftNode = Remove(node.LeftNode, successorValue);

                    }
                    else
                    {

                        // Swap the value of the successor into the node.
                        T successorValue = FindMin(node.RightNode);
                        node.Data = successorValue;

                        // Go into the right subtree and remove the leftmost node we
                        // found and swapped data with. This prevents us from having
                        // two nodes in our tree with the same value.
                        node.RightNode = Remove(node.RightNode, successorValue);
                    }
                }
            }

            // Update balance factor and height values.
            Update(node);

            // Re-balance tree.
            return Balance(node);
        }

        // Helper method to find the leftmost node (which has the smallest value)
        private T FindMin(BinaryTreeNode<T> node)
        {
            while (node.LeftNode != null) node = node.LeftNode;
            return node.Data;
        }

        // Helper method to find the rightmost node (which has the largest value)
        private T FindMax(BinaryTreeNode<T> node)
        {
            while (node.RightNode != null) node = node.RightNode;
            return node.Data;
        }

        public void PrintTree()
        {
            PrintTree(Root);
        }

        public void PrintInorder(BinaryTreeNode<T> node)
        {
            if (node == null) return;

            PrintInorder(node.LeftNode);
            Console.Write($"{node.ToString()} ");
            PrintInorder(node.RightNode);
        }

        public void PrintPreorder(BinaryTreeNode<T> node)
        {
            if (node == null) return;

            Console.Write($"{node.ToString()} ");
            PrintPreorder(node.LeftNode);
            PrintPreorder(node.RightNode);
        }

        public void PrintPostorder(BinaryTreeNode<T> node)
        {
            if (node == null) return;

            PrintPostorder(node.LeftNode);
            PrintPostorder(node.RightNode);
            Console.Write($"{node.ToString()} ");
        }

        private void PrintTree(BinaryTreeNode<T> startNode, string indent = "", string? side = null)
        {
            if (startNode != null)
            {
                var nodeSide = side == null ? "+" : side;
                Console.WriteLine($"{indent} [{nodeSide}]- {startNode.Data}");
                indent += new string(' ', 3);
                PrintTree(startNode.LeftNode, indent, "L");
                PrintTree(startNode.RightNode, indent, "R");
            }
        }

        public IEnumerable<T> GetEnumeratorInOrder()
        {
            var traversal = Root;
            var stack = new Stack<BinaryTreeNode<T>>();
            stack.Push(Root);

            while (stack.Any())
            {
                while (traversal != null && traversal.LeftNode != null)
                {
                    stack.Push(traversal.LeftNode);
                    traversal = traversal.LeftNode;
                }

                var node = stack.Pop();

                if (node.RightNode != null)
                {
                    stack.Push(node.RightNode);
                    traversal = node.RightNode;
                }

                yield return node.Data;
            }
        }

        public IEnumerator<T> GetEnumeratorPreOrder()
        {
            var stack = new Stack<BinaryTreeNode<T>>();
            stack.Push(Root);

            while (stack.Any())
            {
                var node = stack.Pop();

                if (node.RightNode != null) stack.Push(node.RightNode);
                if (node.LeftNode != null) stack.Push(node.LeftNode);                

                yield return node.Data;
            }
        }

        public IEnumerator<T> GetEnumeratorPostOrder()
        {
            var stack1 = new Stack<BinaryTreeNode<T>>();
            var stack2 = new Stack<BinaryTreeNode<T>>();
            stack1.Push(Root);

            while (stack1.Any())
            {
                var node = stack1.Pop();
                if (node != null)
                {
                    stack2.Push(node);
                    if (node.LeftNode != null) stack1.Push(node.LeftNode);
                    if (node.RightNode != null) stack1.Push(node.RightNode);
                }
            }

            while (stack2.Any())
            {
                yield return stack2.Pop().Data;
            }
        }

        public IEnumerator<T> GetEnumeratorLevelOrder()
        {
            var queue = new Queue<BinaryTreeNode<T>>();
            queue.Enqueue(Root);

            while (queue.Any())
            {
                var node = queue.Dequeue();
                if (node.LeftNode != null) queue.Enqueue(node.LeftNode);
                if (node.RightNode != null) queue.Enqueue(node.RightNode);
                yield return node.Data;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return null; // GetEnumeratorInOrder();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IPreorderIEnumerator<T>.GetEnumerator()
        {
            return GetEnumeratorPreOrder();
        }

        public ICustomIEnumerator<T> PostOrdered => _postOrderIEnumerator;
        public ICustomIEnumerator<T> LevelOrdered => _levelOrderIEnumerator;

        public ICustomIEnumerator<T> GetCustomEnumerator(TreeTraversalOrder traversalOrder)
        {
            return traversalOrder switch
            {
                TreeTraversalOrder.POST_ORDER => _postOrderIEnumerator,
                TreeTraversalOrder.LEVEL_ORDER => _levelOrderIEnumerator,
                _ => _postOrderIEnumerator
            };
        }

        //public IEnumerator InOrderEnumerator => return GetEnumeratorInOrder<T>();

        //public IEnumerator<T> GetEnumerator()
        //{
        //    var stack = new Stack<BinaryTreeNode<T>>();
        //    var queue = new Queue<BinaryTreeNode<T>>();
        //    queue.Enqueue(Root);

        //    while (true)
        //    {
        //        var node = queue.Peek();
        //        if (node.RightNode != null && !queue.Contains(node.RightNode)) stack.Push(node.RightNode);
        //        if (node.LeftNode != null && !queue.Contains(node.LeftNode)) stack.Push(node.LeftNode);                
        //        stack.Push(node);
        //    }

        //    while(stack.Any()) yield return stack.Pop().Data;
        //}

        //public IEnumerator InOrderEnumerator =>    return GetEnumerator();
        //}

        //public TreeTraversalOrder TraversalOrder { get; set; }

        //IEnumerable GetFlexibleEnumerator()
        //{
        //    return GetEnumerator();
        //}

        //// Returns as iterator to traverse the tree in order.
        //public java.util.Iterator<T> iterator()
        //{

        //    final int expectedNodeCount = nodeCount;
        //    final java.util.Stack<Node> stack = new java.util.Stack<>();
        //    stack.push(root);

        //    return new java.util.Iterator<T>() {
        //      Node trav = root;

        //    @Override
        //      public boolean hasNext()
        //    {
        //        if (expectedNodeCount != nodeCount) throw new java.util.ConcurrentModificationException();
        //        return root != null && !stack.isEmpty();
        //    }

        //    @Override
        //      public T next()
        //    {

        //        if (expectedNodeCount != nodeCount) throw new java.util.ConcurrentModificationException();

        //        while (trav != null && trav.left != null)
        //        {
        //            stack.push(trav.left);
        //            trav = trav.left;
        //        }

        //        Node node = stack.pop();

        //        if (node.right != null)
        //        {
        //            stack.push(node.right);
        //            trav = node.right;
        //        }

        //        return node.value;
        //    }

        //    @Override
        //      public void remove()
        //    {
        //        throw new UnsupportedOperationException();
        //    }
        //};
        //  }

        //  @Override
        //  public String toString()
        //{
        //    return TreePrinter.getTreeDisplay(root);
        //}

        //// Make sure all left child nodes are smaller in value than their parent and
        //// make sure all right child nodes are greater in value than their parent.
        //// (Used only for testing)
        //public boolean validateBSTInvarient(Node node)
        //{
        //    if (node == null) return true;
        //    T val = node.value;
        //    boolean isValid = true;
        //    if (node.left != null) isValid = isValid && node.left.value.compareTo(val) < 0;
        //    if (node.right != null) isValid = isValid && node.right.value.compareTo(val) > 0;
        //    return isValid && validateBSTInvarient(node.left) && validateBSTInvarient(node.right);
        //}
    }

    public enum TreeTraversalOrder
    {
        None,
        PRE_ORDER,
        IN_ORDER,
        POST_ORDER,
        LEVEL_ORDER
    }

    public interface IPreorderIEnumerator<T> where T : IComparable
    {
        IEnumerator GetEnumerator();
    }

    public interface IPostOrderIEnumerator<T> where T : IComparable
    {
        IEnumerator GetEnumerator();
    }

    public interface ICustomIEnumerator<T> where T : IComparable
    {
        IEnumerator GetEnumerator();
    }

    //public class PostOrderIEnumerator<T> : IPostOrderIEnumerator<T> where T : IComparable
    public class PostOrderIEnumerator<T> : ICustomIEnumerator<T> where T : IComparable
    {
        private readonly AVLTreeRecursive<T> _tree;

        public PostOrderIEnumerator(AVLTreeRecursive<T> tree)
        {
            _tree = tree;
        }

        public IEnumerator GetEnumerator()
        {
            var stack1 = new Stack<BinaryTreeNode<T>>();
            var stack2 = new Stack<BinaryTreeNode<T>>();
            stack1.Push(_tree.Root);

            while (stack1.Any())
            {
                var node = stack1.Pop();
                if (node != null)
                {
                    stack2.Push(node);
                    if (node.LeftNode != null) stack1.Push(node.LeftNode);
                    if (node.RightNode != null) stack1.Push(node.RightNode);
                }
            }

            while (stack2.Any())
            {
                yield return stack2.Pop().Data;
            }
        }
    }

    public class LevelOrderIEnumerator<T> : ICustomIEnumerator<T> where T : IComparable
    {
        private readonly AVLTreeRecursive<T> _tree;

        public LevelOrderIEnumerator(AVLTreeRecursive<T> tree)
        {
            _tree = tree;
        }

        public IEnumerator GetEnumerator()
        {
            var queue = new Queue<BinaryTreeNode<T>>();
            queue.Enqueue(_tree.Root);

            while (queue.Any())
            {
                var node = queue.Dequeue();
                if (node.LeftNode != null) queue.Enqueue(node.LeftNode);
                if (node.RightNode != null) queue.Enqueue(node.RightNode);
                yield return node.Data;
            }
        }
    }
}

