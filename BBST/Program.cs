namespace BBST
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var avlTree = new AVLTreeRecursive<int>();

            avlTree.Insert(8);
            avlTree.Insert(3);
            avlTree.Insert(10);
            avlTree.Insert(1);
            avlTree.Insert(6);
            avlTree.Insert(4);

            avlTree.PrintTree();

            avlTree.Insert(7);
            avlTree.Insert(14);
            avlTree.Insert(16);

            avlTree.PrintTree();

            Console.WriteLine(Environment.NewLine + "Preorder");
            foreach (var nodeValue in (IPreorderIEnumerator<int>)avlTree)
                Console.Write($"{nodeValue} ");
            Console.WriteLine(Environment.NewLine + "PostOrder");
            foreach (var nodeValue in avlTree.GetCustomEnumerator(TreeTraversalOrder.POST_ORDER))//avlTree.PostOrdered)
                Console.Write($"{nodeValue} ");
            Console.WriteLine(Environment.NewLine + "LevelOrder");
            foreach (var nodeValue in avlTree.GetCustomEnumerator(TreeTraversalOrder.LEVEL_ORDER))
                Console.Write($"{nodeValue} ");
            Console.WriteLine(Environment.NewLine + "InOrder");
            foreach (var nodeValue in avlTree.GetEnumeratorInOrder())
            {
                Console.Write($"{nodeValue} ");
            }
        }
    }
}