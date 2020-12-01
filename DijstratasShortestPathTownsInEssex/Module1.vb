Imports System.IO
Module Module1
    Function dijkstra(graph As Dictionary(Of String, Dictionary(Of String, Integer)), start As String, goal As String)
        'Initialise
        Dim infinity As Integer = 10000000
        Dim distance As New Dictionary(Of String, Integer)
        Dim previous_vertex As New Dictionary(Of String, String)
        Dim shortest_path As New List(Of String)
        Dim shortest As String
        Dim cost As Integer

        'Set the shortest distance from the start for all vertices to infinity
        For Each vertex In graph
            distance.Add(vertex.Key, infinity)
        Next
        'Set the shortest distance from the start for the start vertex to 0
        distance(start) = 0

        'Loop until all the vertices have been visited
        While graph.Count > 0
            'Find the vertex with the shortest distance from the start
            shortest = Nothing
            For Each vertex In graph
                'Console.WriteLine("Current Vertext is" & vertex.Key)
                If shortest = Nothing Then
                    shortest = vertex.Key

                ElseIf distance(vertex.Key) < distance(shortest) Then
                    shortest = vertex.Key
                    'Console.WriteLine("Shortest is now" & shortest)
                End If
            Next

            'Console.WriteLine("Now calculate shortest distance for each stage")
            'Calculate shortest distance for each edge

            For Each neighbour In graph(shortest)
                cost = neighbour.Value
                'Update the shortest distance for the vertex if the new value is lower
                If graph.ContainsKey(neighbour.Key) And cost + distance(shortest) < distance(neighbour.Key) Then
                    distance(neighbour.Key) = cost + distance(shortest)
                    previous_vertex(neighbour.Key) = shortest
                End If
            Next
            'The vertex has now been visited, remove it from the vertices to consider
            graph.Remove(shortest)
        End While


        'Generate the shortest path
        'Start from the goal, adding vertices to the front of the list
        Dim current_vertex As String = goal
        While current_vertex <> start
            shortest_path.Insert(0, current_vertex)
            current_vertex = previous_vertex(current_vertex)
        End While
        'Add the start vertex
        shortest_path.Insert(0, start)

        'Return the shortest shortest_path
        Return shortest_path
    End Function

    Sub ShowGraph(ByRef graph As Dictionary(Of String, Dictionary(Of String, Integer)))
        For Each vertpair In graph
            For Each pair As KeyValuePair(Of String, Integer) In graph(vertpair.Key)
                Console.WriteLine("Vertex is {0} - it is a distance of {2} to {1}", vertpair.Key, pair.Key, pair.Value)
            Next
        Next
        Console.WriteLine()
    End Sub

    Sub ListVertices(ByRef graph As Dictionary(Of String, Dictionary(Of String, Integer)))
        For Each vertpair In graph
            Console.WriteLine(vertpair.Key)
        Next
        Console.WriteLine()
    End Sub

    Sub main()
        Dim myline As String
        Dim graph = New Dictionary(Of String, Dictionary(Of String, Integer))
        Dim vertexname() As String
        Dim thisvertexname As String
        Dim edges() As String
        Dim edge_weights As New Dictionary(Of String, Integer)
        Dim text() As String
        Using sr As StreamReader = New StreamReader("SouthAmerica.txt")
            myline = sr.ReadLine()
            Do While myline <> Nothing
                vertexname = myline.Split(":")
                thisvertexname = vertexname(0)
                edges = (vertexname(1).Split(";"))
                edge_weights = edges.ToDictionary(Function(value As String)
                                                      Return value.Split(",")(0)
                                                  End Function,
                                                  Function(value As String)
                                                      Return CInt(value.Split(",")(1))
                                                  End Function)
                graph.Add(thisvertexname, edge_weights)
                myline = sr.ReadLine()
            Loop
        End Using

        'ShowGraph(graph)
        Console.WriteLine("These are all the places we can visit")
        ListVertices(graph)

        Console.Write("Which Vertex shall we start from? ")
        Dim StartAt As String = Console.ReadLine()
        Console.Write("Which Vertex are we going to? ")
        Dim StopAt As String = Console.ReadLine()
        Dim shortest_path = dijkstra(graph, StartAt, StopAt)
        Console.WriteLine()
        Console.WriteLine("Dijkstra says that the optimum route from {0} to {1} is as follows: ", StartAt, StopAt)
        For Each vertex In shortest_path
            Console.WriteLine(vertex)
        Next
        Console.ReadLine()
    End Sub
End Module
