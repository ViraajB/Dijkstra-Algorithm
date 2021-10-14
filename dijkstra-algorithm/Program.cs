using System;
using System.Collections.Generic;
using priorityqueue_csharp;

namespace dijkstra_algorithm
{
    public class Vertex
    {
        public string Name { get; set; }
        public int Distance { get; set; }
        public Vertex Parent { get; set; }
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
