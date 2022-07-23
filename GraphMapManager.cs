using System;
using System.Linq;
using System.Collections.Generic;


namespace EaseTechChallenge
{
    public class GraphMapManager
    {
        //public List<Node> Paths { get; set; }

        public GraphMapManager()
        {
            //Paths = new List<Node>();
        }

        public Stack<Node> GetLongestMapPathFromNode(Node node, bool isRootNode) 
        {
            var nodePaths = BuildNodePathsFromRootNode(node, isRootNode);

            return GetLongestPath(nodePaths);
        }

        public Stack<Node> GetLongestMapPathFromNodes(List<Node> rootNodes) 
        {
            var nodePaths = new List<Stack<Node>>();

            foreach (var rootNode in rootNodes)
            {
                var longestPath = GetLongestMapPathFromNode(rootNode, true);
                nodePaths.Add(longestPath);
            }

            var longetsPathCount = 0;

            foreach (var nodePath in nodePaths)
            {
                if (nodePath.Count > longetsPathCount)
                {
                    longetsPathCount = nodePath.Count;
                }
            }

            var longestPaths = nodePaths
                .Where(x => x.Count.Equals(longetsPathCount))
                .ToList();

            if (longestPaths.Count.Equals(1))
            {
                return longestPaths.FirstOrDefault();
            }

            return longestPaths
                .OrderByDescending(x => x.First().Value - x.Last().Value)
                .FirstOrDefault();
        }

        public string GetCalculatedPath(Stack<Node> calculatedPath)
        {
            var path = string.Empty;
            var calculatedPathList = calculatedPath.ToList();

            for (int i = 0; i < calculatedPath.Count; i++)
            {
                path += calculatedPathList[i].Value.ToString();

                if (i<(calculatedPath.Count-1))
                {
                    path += " - ";
                }
            }

            return path;
        }

        public int GetCalculatedDropPath(Stack<Node> calculatedPath) 
        {
            return calculatedPath.First().Value - calculatedPath.Last().Value;
        }

        private List<Node> BuildNodePathsFromRootNode(Node node, bool isRootNode)
        {
            var nodePaths = new List<Node>();
            var nodesQueue = new Queue<Node>();
            nodesQueue.Enqueue(node);

            while (nodesQueue.Any())
            {
                var currentNode = nodesQueue.Dequeue();

                if (isRootNode)
                {
                    currentNode.ParentNode = null;
                    isRootNode = false;
                }

                nodePaths.Add(currentNode);

                if (currentNode.Edges.Any())
                {
                    foreach (var relatedNode in currentNode.Edges)
                    {
                        relatedNode.ParentNode = currentNode;
                        nodesQueue.Enqueue(relatedNode);
                    }
                }
            }

            return nodePaths;
        }

        private Stack<Node> GetLongestPath(List<Node> nodePaths)
        {
            var leafNodes = nodePaths
                .Where(x => !x.Edges.Any());
            var paths = new List<Stack<Node>>();
            var longetsPathCount = 0; 

            foreach (var leafNode in leafNodes)
            {
                var path = GetPathFromLeafNode(leafNode);
                
                paths.Add(path);

                if (path.Count > longetsPathCount)
                {
                    longetsPathCount = path.Count;
                }
            }

            var longestPaths = paths
                .Where(x => x.Count.Equals(longetsPathCount))
                .ToList();

            if (longestPaths.Count.Equals(1))
            {
                return longestPaths.FirstOrDefault();
            }

            return longestPaths
                .OrderByDescending(x => x.First().Value - x.Last().Value)
                .FirstOrDefault();
        }

        private Stack<Node> GetPathFromLeafNode(Node leafNode) 
        {
            var path = new Stack<Node>();

            while (leafNode != null)
            {
                path.Push(leafNode);
                leafNode = leafNode.ParentNode;
            }

            return path;
        }
    }
}