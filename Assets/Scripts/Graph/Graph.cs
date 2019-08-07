using System.Collections;
using System.Collections.Generic;

public class Graph<T> {
    private List<Vertex<T>> vertexSet;

    public Graph() { }

    public Graph(List<Vertex<T>> vertexSet) {
        if (vertexSet == null)
        {
            this.vertexSet = new List<Vertex<T>>();
        }
        else {
            this.vertexSet = vertexSet;
        }
    }

    public void AddVertex(Vertex<T> vertex) {
        vertexSet.Add(vertex);
    }

    public void AddVertex(T data)
    {
        vertexSet.Add(new Vertex<T>(data));
    }

    //dir is the direction relative to FROM vertex
    public void AddUndirectedEdge(Vertex<T> from, Vertex<T> to, int dir, int cost) {
        from.SetNeighbor(dir, to);
        from.SetCost(dir, cost);
        if (dir == 0) {
            to.SetNeighbor(3, from);
            to.SetCost(3, cost);
        }
        else if (dir == 1)
        {
            to.SetNeighbor(2, from);
            to.SetCost(2, cost);
        }
        else if (dir == 2)
        {
            to.SetNeighbor(1, from);
            to.SetCost(1, cost);
        }
        else if (dir == 3)
        {
            to.SetNeighbor(0, from);
            to.SetCost(0, cost);
        }
    }

    public bool Remove(T value) {
        Vertex<T> foo = null;

        //find the vertex with VALUE
        foreach (Vertex<T> item in vertexSet)
        {
            if (item.GetData().Equals(value)) {
                foo = item;
                break;
            }
        }

        //we didn't find any such vertex
        if (foo == null) {
            return false;
        }

        //remove the vertex from the graph's list of vertices
        vertexSet.Remove(foo);

        //remove edges connecting to the vertex
        foreach (Vertex<T> item in vertexSet)
        {
            int index = item.GetNeighbors().IndexOf(foo);
            if (index != -1) {
                item.SetNeighbor(index, default(Vertex<T>));
                //assuming existing nodes have non-negative edges, this will force algorithms to ignore this edge
                item.SetCost(index, -1);
            }
        }

        return true;
    }

    public List<Vertex<T>> GetVertices() {
        return vertexSet;
    }

    public int GetGraphSize() {
        return vertexSet.Count;
    }
}
