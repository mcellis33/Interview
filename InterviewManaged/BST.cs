using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    public static class TestBST
    {
        public static void Test()
        {
            BST<int, int> bst = new BST<int,int>();
            Random rng = new Random();
            int[] toInsert = Enumerable.Range(0, 30).OrderBy(i => rng.Next()).ToArray();
            foreach (int i in toInsert)
            {
                Console.WriteLine("Inserting {0}", i);
                bst.Insert(i, i);
                //Console.WriteLine(bst);
            }

            Console.Write(bst);

            Console.WriteLine("Inorder Traversal (recursive): ");
            foreach (KeyValuePair<int, int> kvp in bst.InorderTraversalRecursive())
                Console.Write("{0}:{1}, ", kvp.Key, kvp.Value);
            Console.WriteLine();

            Console.WriteLine("Inorder Traversal (iterative): ");
            foreach (KeyValuePair<int, int> kvp in bst.InorderTraversal())
                Console.Write("{0}:{1}, ", kvp.Key, kvp.Value);
            Console.WriteLine();
            
            foreach (int i in toInsert)
            {
                Console.WriteLine("Removing {0}", i);
                bst.Delete(i);
                //Console.WriteLine(bst);
            }

        }
    }

    public class BST<TKey, TValue> where TKey : IComparable<TKey>
    {
	    protected class Node
	    {
		    public Node(TKey key, TValue value)
		    {
			    Key = key;
			    Value = value;
		    }
            public Node Left;
		    public Node Right;
            public TKey Key;
            public TValue Value;

		    public bool IsLeaf()
		    {
			    return (Left == null && Right == null);
		    }

            public override string ToString()
            {
                return string.Format("{{{0}:{1}}}", Key, Value);
            }
	    }

        protected enum ChildType
        {
            Left,
            Right,
            Root
        }

	    private Node _root;

	    // Create new empty BST
	    public BST()
	    {
	    }

	    public void Insert(TKey key, TValue value)
	    {
            Node newNode = new Node(key, value);
		    if (_root == null)
		    {
			    _root = newNode;
		    }
		    else
		    {
			    Node node;
			    Node parent;
			    ChildType childType;
			    FindNode(key, out node, out parent, out childType);
			    if (node != null)
				    throw new Exception("Key already exists.");
                ReplaceChild(parent, childType, newNode);
		    }
	    }

	    public void Delete(TKey key)
	    {
		    if (_root == null)
			    return;
			
		    // Get the node to remove
            Node node;
		    Node parent;
		    ChildType childType;
            FindNode(key, out node, out parent, out childType);
            if (node == null)
                throw new Exception("Key not found.");

            DeleteNode(node, parent, childType);
        }

        protected void DeleteNode(Node node, Node parent, ChildType childType)
        {
            // If the node is a leaf, then we can simply remove it by removing its parent’s reference to it
            if (node.IsLeaf())
                ReplaceChild(parent, childType, null);

            // If only one subtree exists, then put that subtree in place of the node.
            else if (node.Left == null && node.Right != null)
                ReplaceChild(parent, childType, node.Right);
            else if (node.Left != null && node.Right == null)
                ReplaceChild(parent, childType, node.Left);

            // If both subtrees exist, then
            // 1) find the inorder successor
            // 2) copy its key and value into the node (basically replacing the node)
            // 3) Delete the inorder successor from its original location by recursing
            else
            {
                // Find the inorder successor (the minimum node in the right subtree)
                Node replacement = node.Right;
                Node replacementParent = node;
                ChildType replacementChildType = ChildType.Right;
                while (replacement.Left != null)
                {
                    replacementParent = replacement;
                    replacement = replacement.Left;
                    replacementChildType = ChildType.Left;
                }
                node.Key = replacement.Key;
                node.Value = replacement.Value;
                DeleteNode(replacement, replacementParent, replacementChildType);
            }
        }

        protected void ReplaceChild(Node parent, ChildType childType, Node replacement)
        {
            switch (childType)
            {
                case ChildType.Left:
                    parent.Left = replacement;
                    break;
                case ChildType.Right:
                    parent.Right = replacement;
                    break;
                case ChildType.Root:
                    _root = replacement;
                    break;
            }
        }

	    public TValue Find(TKey key)
	    {
            Node node;
            Node parent;
            ChildType childType;
            FindNode(key, out node, out parent, out childType);
            if (node == null)
                throw new Exception("Key not found.");
            return node.Value;
	    }

	    // Using a key, find a node, its parent, and whether it is a left or right child.
	    // If the node is not found, node is null, but parent is still the parent that the node would have if it were inserted
	    // If the node is the root, parent is null.
	    protected void FindNode(TKey key, out Node node, out Node parent, out ChildType childType)
	    {
		    parent = null;
		    node = _root;
		    childType = ChildType.Root;

		    if (_root == null)
			    return;
		
		    while(node != null)
		    {
			    int keyVsNode = key.CompareTo(node.Key);
			    // If the new key is less than the current node’s
			    if (keyVsNode < 0)
			    {
				    // Go to the left child
				    parent = node;
                    childType = ChildType.Left;
				    node = node.Left;
			    }
			    // If the new key is greater than the current node’s
			    else if (keyVsNode > 0)
			    {
				    // Go to the right child
				    parent = node;
				    childType = ChildType.Right;
				    node = node.Right;
                }
                // Otherwise we’ve found a matching key, so return
                else
                {
	                return;
                }
		    }
		    // Here we have fallen off the tree. The parent is the desired parent if we were to insert the key, and the node is null.
	    }

        public IEnumerable<KeyValuePair<TKey, TValue>> InorderTraversalRecursive()
        {
            var result = new List<KeyValuePair<TKey, TValue>>();
            InorderTraversalRecursiveHelper(_root, result);
            return result;
        }

        private void InorderTraversalRecursiveHelper(Node current, List<KeyValuePair<TKey, TValue>> result)
        {
            if (current == null)
                return;
            if (current.Left != null)
                InorderTraversalRecursiveHelper(current.Left, result);
            result.Add(new KeyValuePair<TKey, TValue>(current.Key, current.Value));
            if (current.Right != null)
                InorderTraversalRecursiveHelper(current.Right, result);
        }

        public IEnumerable<KeyValuePair<TKey, TValue>> InorderTraversal()
        {
            var result = new List<KeyValuePair<TKey, TValue>>();
            if (_root == null)
                return result;

            var stack = new Stack<Node>();
            stack.Push(_root);
            bool done = false;
            while (!done)
            {
                Node current = stack.Pop();
                if (current != null)
                {
                    if (current.Left != null)
                    {
                        stack.Push(current.Left);
                    }
                    result.Add(new KeyValuePair<TKey, TValue>(current.Key, current.Value));
                    if (current.Right != null)
                    {
                        current = current.Right;
                    }
                    else
                    {
                        current = stack.Pop();
                    }
                }
            }
            return result;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            
            ToStringHelper(sb, _root, 0);

            return sb.ToString();
        }

        private void ToStringHelper(StringBuilder sb, Node node, int level)
        {
            sb.Append(' ', 3*level);
            if (node != null)
            {
                sb.Append(node.ToString());
                sb.AppendLine();
                if (!node.IsLeaf())
                {
                    ToStringHelper(sb, node.Right, level + 1);
                    ToStringHelper(sb, node.Left, level + 1);
                }
            }
            else
                sb.AppendLine("<NULL>");
        }
    }
}