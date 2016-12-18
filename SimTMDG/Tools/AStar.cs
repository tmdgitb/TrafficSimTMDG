using SimTMDG.Road;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimTMDG.Tools
{
    public class PriorityQueue<T>
    {
        // I'm using an unsorted array for this example, but ideally this
        // would be a binary heap. Find a binary heap class:
        // * https://bitbucket.org/BlueRaja/high-speed-priority-queue-for-c/wiki/Home
        // * http://visualstudiomagazine.com/articles/2012/11/01/priority-queues-with-c.aspx
        // * http://xfleury.github.io/graphsearch.html
        // * http://stackoverflow.com/questions/102398/priority-queue-in-net

        private List<Tuple<T, double>> elements = new List<Tuple<T, double>>();

        public int Count
        {
            get { return elements.Count; }
        }

        public void Enqueue(T item, double priority)
        {
            elements.Add(Tuple.Create(item, priority));
        }

        public T Dequeue()
        {
            int bestIndex = 0;

            for (int i = 0; i < elements.Count; i++)
            {
                if (elements[i].Item2 < elements[bestIndex].Item2)
                {
                    bestIndex = i;
                }
            }

            T bestItem = elements[bestIndex].Item1;
            elements.RemoveAt(bestIndex);
            return bestItem;
        }
    }


    /* NOTE about types: in the main article, in the Python code I just
     * use numbers for costs, heuristics, and priorities. In the C++ code
     * I use a typedef for this, because you might want int or double or
     * another type. In this C# code I use double for costs, heuristics,
     * and priorities. You can use an int if you know your values are
     * always integers, and you can use a smaller size number if you know
     * the values are always small. */

    public class AStar
    {
        public Dictionary<RoadSegment, RoadSegment> cameFrom
            = new Dictionary<RoadSegment, RoadSegment>();
        public Dictionary<RoadSegment, double> costSoFar
            = new Dictionary<RoadSegment, double>();

        // Note: a generic version of A* would abstract over Location and
        // also Heuristic
        static public double Heuristic(RoadSegment a, RoadSegment b)
        {
            return Math.Abs(a.startNode.Position.X - b.startNode.Position.X) + Math.Abs(a.endNode.Position.Y - b.endNode.Position.Y);
        }

        public AStar(List<RoadSegment> graph, RoadSegment start, RoadSegment goal)
        {
            var frontier = new PriorityQueue<RoadSegment>();
            frontier.Enqueue(start, 0);

            cameFrom[start] = start;
            costSoFar[start] = 0;

            while (frontier.Count > 0)
            {
                var current = frontier.Dequeue();

                if (current.Equals(goal))
                {
                    break;
                }

                foreach (var next in current.nextSegment)
                {
                    double newCost = costSoFar[current]
                        + next.Length;
                    if (!costSoFar.ContainsKey(next)
                        || newCost < costSoFar[next])
                    {
                        costSoFar[next] = newCost;
                        double priority = newCost + Heuristic(next, goal);
                        frontier.Enqueue(next, priority);
                        cameFrom[next] = current;
                    }
                }
            }
        }
    }
}
