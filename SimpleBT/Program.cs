namespace SimpleBT
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Tree tree = new Tree();
            /*
            // Try 1
            tree.Root = new Node(1);
            tree.Root.Left= new Node(2);
            tree.Root.Right= new Node(3);
            tree.Root.Left.Left= new Node(4);
            tree.Root.Left.Right = new Node(5);
            */

            // Try 2
            tree.Add(8);
            tree.Add(3);
            tree.Add(10);
            tree.Add(1);
            tree.Add(6);
            tree.Add(4);
            tree.Add(7);
            tree.Add(14);
            tree.Add(16);

            Console.WriteLine("Inorder traversal of the constracted tree is");
            tree.InOrder();

            //tree.Mirror();
            //Console.WriteLine($"{Environment.NewLine}Inorder traversal of the mirror tree is");
            //tree.InOrder();

            var sr = tree.Find(3);
            sr = tree.Find(14);
            sr = tree.Find(15);

            tree.Remove(1);

            Console.WriteLine(Environment.NewLine + "Inorder traversal of the new tree is" + Environment.NewLine);
            var flippedTree = new Tree();
            flippedTree.Root = new Node(2);
            flippedTree.Root.Left = new Node(4);
            flippedTree.Root.Right = new Node(5);

            //flippedTree.Root = new Node(1);
            //flippedTree.Root.Left = new Node(2);
            //flippedTree.Root.Left.Left = new Node(4);
            //flippedTree.Root.Left.Right = new Node(5);
            //flippedTree.Root.Right = new Node(3);
            //flippedTree.Root.Right.Left = new Node(6);
            //flippedTree.Root.Right.Right = new Node(7);

            flippedTree.PrintTree();

            Console.WriteLine(Environment.NewLine + "Inorder traversal of the flipped tree is" + Environment.NewLine);
            flippedTree.FlipTree();
            flippedTree.PrintTree();
        }
    }
}