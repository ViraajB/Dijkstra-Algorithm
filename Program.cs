using System;
using System.Collections.Generic;

namespace dijkstra_algorithm
{
    public class Vertex
    {
        public string Name { get; set; }
        public int Distance { get; set; }
        public Vertex Parent { get; set; }
    }

    /*
    A priority queue allows the insertion of an object with priority.
    Highest priority objects can be drawn in FIFO (first in first out).
    Once objects with highest priority are fetched, objects with second highest priority can be fetched.
    Most efficient implementation uses heaps (completed in log(n) time).
    */
    public class PriorityQueue<T>
    {
        class Node
        {
            public int Priority { get; set; }
            public T Object { get; set; }
        }

        // object array
        List<Node> queue = new();
        int heapSize = -1;
        bool _isMinPriorityQueue;
        public int Count
        {
            get
            {
                return queue.Count;
            }
        }

        // If min queue or max queue
        public PriorityQueue(bool isMinPriorityQueue = false)
        {
            _isMinPriorityQueue = isMinPriorityQueue;
        }
        // Enqueue the object with priority
        public void Enqueue(int priority, T obj)
        {
            Node node = new()
            {
                Priority = priority,
                Object = obj
            };
            queue.Add(node);
            heapSize++;
            // Maintaining heap
            if (_isMinPriorityQueue){
                BuildHeapMin(heapSize);
            } else {
                BuildHeapMax(heapSize);
            }
        }
        // Dequeue the object
        public T Dequeue()
        {
            if (heapSize > -1){
                var returnVal = queue[0].Object;
                queue[0] = queue[heapSize];
                queue.RemoveAt(heapSize);
                heapSize--;
                // Maintaining lowest or highest at root based on min or max queue
                if (_isMinPriorityQueue){
                    MinHeapify(0);
                } else {
                    MaxHeapify(0);
                }
                return returnVal;
            } else {
                throw new Exception("Queue is empty");
            }
        }
        // Update the priority of specific object
        public void UpdatePriority(T obj, int priority)
        {
            int i = 0;
            for (; i <= heapSize; i++){
                Node node = queue[i];
                if (object.ReferenceEquals(node.Object, obj)){
                    node.Priority = priority;
                    if (_isMinPriorityQueue){
                        BuildHeapMin(i);
                        MinHeapify(i);
                    } else {
                        BuildHeapMax(i);
                        MaxHeapify(i);
                    }
                }
            }
        }
        public bool IsInQueue(T obj)
        {
            foreach (Node node in queue){
                if (object.ReferenceEquals(node.Object, obj)){
                    return true;
                }
            }
            return false;
        }

        // Maintain max heap
        private void BuildHeapMax(int i)
        {
            while (i >= 0 && queue[(i - 1) / 2].Priority < queue[i].Priority){
                Swap(i, (i - 1) / 2);
                i = (i - 1) / 2;
            }
        }
        // Maintain min heap
        private void BuildHeapMin(int i)
        {
            while (i >= 0 && queue[(i - 1) / 2].Priority > queue[i].Priority){
                Swap(i, (i - 1) / 2);
                i = (i - 1) / 2;
            }
        }
        private void MaxHeapify(int i)
        {
            int left = ChildL(i);
            int right = ChildR(i);
            int highest = i;

            if (left <= heapSize && queue[highest].Priority < queue[left].Priority){
                highest = left;
            }
            if (right <= heapSize && queue[highest].Priority < queue[right].Priority){
                highest = right;
            }

            if (highest != i){
                Swap(highest, i);
                MaxHeapify(highest);
            }
        }
        private void MinHeapify(int i)
        {
            int left = ChildL(i);
            int right = ChildR(i);
            int lowest = i;

            if (left <= heapSize && queue[lowest].Priority > queue[left].Priority){
                lowest = left;
            }
            if (right <= heapSize && queue[lowest].Priority > queue[right].Priority){
                lowest = right;
            }

            if (lowest != i){
                Swap(lowest, i);
                MinHeapify(lowest);
            }
        }
        private void Swap(int i, int j)
        {
            var temp = queue[i];
            queue[i] = queue[j];
            queue[j] = temp;
        }
        private int ChildL(int i)
        {
            return i * 2 + 1;
        }
        private int ChildR(int i)
        {
            return i * 2 + 2;
        }
    }

    public class Dijkstra_Implementation
    {
        public static void InitialiseSingleSource(Vertex[] vertices, Vertex s)
        {
            foreach (Vertex v in vertices){
               v.Distance = int.MaxValue;
               v.Parent = null;
            }
            s.Distance = 0;
        }

        public static void Relax(Vertex u, Vertex v, int weight)
        {
            if (v.Distance > u.Distance + weight){
                v.Distance = u.Distance + weight;
                v.Parent = u;
            }
        }

        public static List<Vertex> Dijkstra(Vertex[] vertices, int[][] graph, int source)
        {
            InitialiseSingleSource(vertices, vertices[source]);
            List<Vertex> result = new();
            // adding all vertex to priority queue
            PriorityQueue<Vertex> queue = new(true);
            for (int i = 0; i < vertices.Length; i++){
                queue.Enqueue(vertices[i].Distance, vertices[i]);
            }

            // traversing to all vertices
            while (queue.Count > 0){
                var u = queue.Dequeue();
                result.Add(u);
                // again traversing to all vertices
                for (int v = 0; v < graph[Convert.ToInt32(u.Name)].Length; v++){
                    if (graph[Convert.ToInt32(u.Name)][v] > 0){
                        Relax(u, vertices[v], graph[Convert.ToInt32(u.Name)][v]);
                        // updating priority value since distance is changed
                        queue.UpdatePriority(vertices[v], vertices[v].Distance);
                    }
                }
            }
            return result;
        }

        public static void PrintPath(Vertex u, Vertex v, List<Vertex> vertices)
        {
            if (v != u){
                PrintPath(u, v.Parent, vertices);
                Console.WriteLine($"Vertex {v.Name} Weight: {v.Distance}");
            } else {
                Console.WriteLine($"Vertex {v.Name} Weight: {v.Distance}");
            }
        }

        public static void Main(string[] args)
        {
            int[][] adjacencyMatrix = new int[][]{
                new int[] {0, 0, 0, 3, 12},
                new int[] {0, 0, 2, 0, 0},
                new int[] {0, 0, 0, -2, 0},
                new int[] {0, 5, 3, 0, 0},
                new int[] {0, 0, 7, 0 ,0}
            };
            Vertex[] vertices = new Vertex[adjacencyMatrix.GetLength(0)];
            // Source vertex
            int source = 0;

            for (int i = 0; i < vertices.Length; i++){
                vertices[i] = new Vertex() {
                    Name = i.ToString()
                };
            }
            // calling dijkstra algorithm
            List<Vertex> result = Dijkstra(vertices, adjacencyMatrix, source);
            // printing distance
            PrintPath(vertices[0], vertices[2], result);
            Console.ReadLine();
        }
    }
}
