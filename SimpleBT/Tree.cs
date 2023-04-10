using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SimpleBT
{
    public class Node
    {
        public int Data { get; set; }
        public Node? Left { get; set; }
        public Node? Right { get; set; }

        public Node(int item)
        {
            Data = item;
            Left = null;
            Right = null;
        }
    }

    public class Tree
    {
        public Node? Root { get; set; }

        public void Mirror() { Root = Mirror(Root);  }

        public Node? Mirror(Node? node) 
        {
            if (node == null) return node;

            Node? left = Mirror(node.Left);
            Node? right = Mirror(node.Right);

            node.Left = right;
            node.Right = left;

            return node;
        }

        public void InOrder() { InOrder(Root); }

        public void InOrder(Node? node) 
        { 
            if (node == null) return;

            InOrder(node.Left);
            Console.Write($"{node.Data} ");
            InOrder(node.Right);
        }

        public Node? Add(int data) { return Add(new Node(data)); }

        private Node? Add(Node node, Node? currentNode = null) 
        { 
            if (Root == null)
            {
                return Root = node;
            }

            currentNode ??= Root;

            int cmp = node.Data.CompareTo(currentNode.Data);
            return cmp == 0 
                ? currentNode 
                : cmp < 0 
                    ? currentNode.Left == null ? currentNode.Left = node : Add(node, currentNode.Left)
                    : currentNode.Right == null ? currentNode.Right = node : Add(node, currentNode.Right);
        }

        public Node? Find(int data, Node? startNode = null)
        {
            startNode ??= Root;

            if (startNode == null) return startNode;

            int cmp = data.CompareTo(startNode.Data);
            return cmp == 0
                ? startNode
                : cmp < 0
                    ? startNode.Left == null ? null : Find(data, startNode.Left)
                    : startNode.Right == null ? null : Find(data, startNode.Right);
        }

        public bool Contains(int data)
        {
            return Contains(Root, data);
        }

        private bool Contains(Node? node, int data)
        {
            if (node == null) return false;

            int cmp = data.CompareTo(node.Data);

            if (cmp < 0) 
                return Contains(node.Left, data);
            else if (cmp > 0) 
                return Contains(node.Right, data);
            else 
                return true;
        }

        private Node FindMin(Node node)
        {
            while (node.Left != null) node = node.Left;
            return node;
        }

        private Node FindMax(Node node)
        {
            while (node.Right != null) node = node.Right;
            return node;
        }

        public void Remove(int data)
        {
            if (Contains(data))
            {
                Root = Remove(Root, data);
            }
        }

        private Node? Remove(Node? node, int data)
        {
            if (node == null) return null;

            int cmp = data.CompareTo(node.Data);

            if (cmp < 0)
            {
                node.Left = Remove(node.Left, data);
            }
            else if (cmp > 0)
            {
                node.Right = Remove(node.Right, data);
            }
            else
            {
                if (node.Left == null)
                {
                    return node.Right;
                }
                else if (node.Right == null)
                {
                    return node.Left;
                }
                else
                {
                    Node tmp = FindMin(node.Right);
                    node.Data = tmp.Data;
                    node.Right = Remove(node.Right, tmp.Data);

                    // If instead we wanted to find the largest node in the left
                    // subtree as opposed to smallest node in the right subtree
                    // here is what we would do:
                    //Node tmp = FindMax(node.Left);
                    //node.Data = tmp.Data;
                    //node.Left = Remove(node.Left, tmp.Data);
                }
            }

            return node;
        }

        public void FlipTree()
        {
            //Root = FlipTree(Root);
            //Root = RightRotation(Root);
            Root = LeftRotation(Root);            
        }

        private Node? FlipTree(Node? node)
        {
            if (node == null) return null;

            if (node.Left == null && node.Right == null) return node;

            var flippedNode = FlipTree(node.Left);

            node.Left.Left = node.Right;
            node.Left.Right = node;
            node.Left = node.Right = null;
            return flippedNode;
        }

        public void PrintTree()
        {
            PrintTree(Root);
        }

        private void PrintTree(Node? startNode, string indent = "", string? side = null)
        {
            if (startNode != null)
            {
                var nodeSide = side == null ? "+" : side;
                Console.WriteLine($"{indent} [{nodeSide}]- {startNode.Data}");
                indent += new string(' ', 3);
                PrintTree(startNode.Left, indent, "L");
                PrintTree(startNode.Right, indent, "R");
            }
        }

        private Node? LeftRotation(Node? node)
        {
            if (node == null) return null;

            Node? newParent = node.Right;
            node.Right = newParent.Left;
            newParent.Left = node;
            //update(node);
            //update(newParent);
            return newParent;
        }

        private Node? RightRotation(Node? node)
        {
            Node newParent = node.Left;
            node.Left = newParent.Right;
            newParent.Right = node;
            //update(node);
            //update(newParent);
            return newParent;
        }
    }
}
