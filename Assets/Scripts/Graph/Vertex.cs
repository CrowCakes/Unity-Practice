using System.Collections;
using System.Collections.Generic;

public class Vertex<T>
{
    /*
     * The indices of the 2 lists correspond to a logical direction relative to it
     * 0 = north
     * 1 = west
     * 2 = east
     * 3 = south
     */
    private List<Vertex<T>> neighbors = new List<Vertex<T>>(4);
    /*
     * The default value of the elements in this list is -1
     */
    private List<int> costs = new List<int>(4);
    private T data;

    public Vertex() {
        InitializeLists();
    }

    public Vertex(T input) {
        InitializeLists();
        data = input;
    }

    public Vertex(T input, List<Vertex<T>> inputNeighbors) {
        InitializeLists();
        data = input;
        neighbors = inputNeighbors;
    }

    public Vertex(T input, List<Vertex<T>> inputNeighbors, List<int> inputCosts)
    {
        data = input;
        neighbors = inputNeighbors;
        costs = inputCosts;
    }

    public void SetNeighbor(int dir, Vertex<T> neighbor) {
        neighbors[dir] = neighbor;
    }

    public void SetCost(int dir, int cost) {
        costs[dir] = cost;
    }

    public void SetData(T inputData) {
        data = inputData;
    }

    public List<Vertex<T>> GetNeighbors() {
        return neighbors;
    }

    public List<int> GetCosts() {
        return costs;
    }

    public T GetData() {
        return data;
    }

    private void InitializeLists() {
        for (int i = 0; i < 4; i++) {
            neighbors.Add(null); costs.Add(-1);
        }
    }
}
