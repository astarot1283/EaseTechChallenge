using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace EaseTechChallenge
{
    public enum Orientation 
    {
        Left,
        Right,
        Top,
        Bottom
    }

    public class GraphFileBuilder
    {
        public List<Node> Nodes { get; set; }

        public List<Node> MaximumNodes{ get; set; }

        public int MapSize { get; set; }

        public string MapFilePath { get; set; }

        public string MapValueSeparator { get; set; }

        public GraphFileBuilder(string mapfilePath, string mapValueSeparator) 
        {
            MapFilePath = mapfilePath;
            MapValueSeparator = mapValueSeparator;
            Nodes = new List<Node>();
            MaximumNodes = new List<Node>();
        }

        public void BuildMapFromFile()
        {
            try
            {
                var filesLines = File.ReadAllLines(MapFilePath);

                ValidateFile(filesLines);

                CreateNodesFromFileLines(filesLines.Skip(1),
                    MapValueSeparator);

                CreateRelationshipsBetweenNodes(Nodes);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void BuildMapWithMaximumNodesFromFile()
        {
            try
            {
                var filesLines = File.ReadAllLines(MapFilePath);

                ValidateFile(filesLines);

                CreateNodesFromFileLines(filesLines.Skip(1),
                    MapValueSeparator);

                CreateRelationshipsBetweenNodes(MaximumNodes);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void CreateNodesFromFileLines(IEnumerable<string> bodyMapLines,
            string valuesSeparator)
        {
            for (int i = 0; i < bodyMapLines.Count(); i++)
            {
                var integerNodeList = bodyMapLines
                    .ElementAt(i)
                    .ConvertToIntegerNodeList(valuesSeparator)
                    .ToArray();

                var maxNodeValue = integerNodeList.Max();
                var maxNodePosition = Array.IndexOf(
                    integerNodeList,
                    integerNodeList.Max());

                var maximumNode = maxNodeValue
                    .ToString()
                    .ConvertToNode(i, maxNodePosition);

                MaximumNodes.Add(maximumNode);
                Nodes.AddRange(integerNodeList
                    .ConvertToNodeList(i));
            }
        }

        public void CreateInlineRelationshipsBetweenNodes() 
        {
            var nodes = MaximumNodes;
            var nodesQueue = new Queue<Node>();

            foreach (var node in nodes)
            {
                nodesQueue.Enqueue(node);
            }

            while (nodesQueue.Any())
            {
                var currentNode = nodesQueue.Dequeue();

                currentNode.Edges.AddRange(
                    GetNextRelatedNodes(currentNode));

                if (currentNode.Edges.Any())
                {
                    foreach (var relatedNode in currentNode.Edges)
                    {
                        nodesQueue.Enqueue(relatedNode);
                    }
                }
            }
        }

        private void CreateRelationshipsBetweenNodes(List<Node> nodes)
        {
            var count = 1;

            foreach (var node in nodes)
            {
                node.Edges.AddRange(
                    GetNextRelatedNodes(node));
                count++;
                Console.WriteLine(string
                    .Concat($"Procesing Node[{count}] = ",
                    node.Value,
                    " At the position (",
                    node.Position.X.ToString(),
                    ",",
                    node.Position.Y.ToString(),
                    ")"));
            }
        }

        private List<Node> GetNextRelatedNodes(Node node) 
        {
            var relatedNodes = new List<Node>();

            var leftRelatedNode = GetNextRelatedNode(node, Orientation.Left);

            if (leftRelatedNode != null)
            {
                relatedNodes.Add(leftRelatedNode);
            }

            var rightRelatedNode = GetNextRelatedNode(node, Orientation.Right);

            if (rightRelatedNode != null)
            {
                relatedNodes.Add(rightRelatedNode);
            }

            var topRelatedNode = GetNextRelatedNode(node, Orientation.Top);

            if (topRelatedNode != null)
            {
                relatedNodes.Add(topRelatedNode);
            }

            var bottomtRelatedNode = GetNextRelatedNode(node, Orientation.Bottom);

            if (bottomtRelatedNode != null)
            {
                relatedNodes.Add(bottomtRelatedNode);
            }

            return relatedNodes;
        }

        private Node GetNextRelatedNode(Node node, Orientation orientation) 
        {
            var nextRelatedNode = GetNodeByPosition(
                GetNextRelatedNodePosition(node, orientation));

            if (nextRelatedNode == null)
                return null;

            if (!NextNodePositionIsValid(nextRelatedNode.Position) || 
                nextRelatedNode.Value >= node.Value)
                return null;

            return nextRelatedNode;
        }

        private void ValidateFile(string[] filesLines)
        {
            if (filesLines == null || !filesLines.Any())
            {
                throw new Exception("Error ... The File Map is empty");
            }

            var headMapLine = filesLines.ElementAt(0);

            if (headMapLine == null || !headMapLine.Any())
            {
                throw new Exception("Error ... The File Map has not specified size");
            }

            var headMapValues = headMapLine.Split(MapValueSeparator);

            if (headMapValues.Length != 2)
            {
                throw new Exception("Error ... The Map size has not two dimensions");
            }

            var convertedSizeX = int.TryParse(
                headMapValues[0], out int sizeX) ?
                sizeX : 0;

            var convertedSizeY = int.TryParse(
                headMapValues[1], out int sizeY) ?
                sizeY : 0;

            if (convertedSizeX <= 0)
            {
                throw new Exception(string.Concat("Error ... ",
                    "The Map size dimensions must be an integer number greather than zero"));
            }

            if (convertedSizeY <= 0)
            {
                throw new Exception(string.Concat("Error ... ",
                    "The Map size dimensions must be an integer number greather than zero"));
            }

            if (!convertedSizeX.Equals(convertedSizeY))
            {
                throw new Exception("Error ... The Map size dimensions are differents");
            }

            MapSize = convertedSizeX;
        }

        private Point GetNextRelatedNodePosition(Node node, Orientation orientation)
        {
            var nextNodePosition = new Point();

            if (orientation.Equals(Orientation.Left))
            {
                nextNodePosition.X = node.Position.X;
                nextNodePosition.Y = node.Position.Y - 1;
            }

            if (orientation.Equals(Orientation.Right))
            {
                nextNodePosition.X = node.Position.X;
                nextNodePosition.Y = node.Position.Y + 1;
            }

            if (orientation.Equals(Orientation.Top))
            {
                nextNodePosition.X = node.Position.X - 1;
                nextNodePosition.Y = node.Position.Y;
            }

            if (orientation.Equals(Orientation.Bottom))
            {
                nextNodePosition.X = node.Position.X + 1;
                nextNodePosition.Y = node.Position.Y;
            }

            return nextNodePosition;
        }

        private bool NextNodePositionIsValid(Point position)
        {
            return position.X >= 0 &&
                position.X < MapSize &&
                position.Y >= 0 &&
                position.Y < MapSize;
        }

        private Node GetNodeByPosition(Point position) 
        {
            return Nodes
                .FirstOrDefault(x => x.Position.X
                .Equals(position.X) &&
                x.Position.Y.Equals(position.Y));
        }
    }
}