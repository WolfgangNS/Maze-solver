Public Class Form1

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            Dim maze = New Bitmap(TextBox1.Text, True)
            PictureBox1.Image = enlarge(maze, 10)

            Dim compass As New List(Of coordinates)
            compass.Add(New coordinates(0, 1)) 'down
            compass.Add(New coordinates(-1, 0)) 'left
            compass.Add(New coordinates(1, 0)) 'right
            compass.Add(New coordinates(0, -1)) 'up

            Dim path As New List(Of coordinates)
            Dim past As New List(Of coordinates)

            Dim loc As coordinates = findloc(maze, Color.Lime) 'start, and current position
            Dim endloc As coordinates = findloc(maze, Color.Red) 'end
            path.Add(loc)

            Dim area As Integer = maze.Width * maze.Height
            Dim i As Integer = 0
            While i < area

                Dim temppath As New List(Of coordinates)
                For Each Dir As coordinates In compass
                    Dim eval As coordinates = addc(Dir, loc)
                    If Not (eval.outofbounds(maze) Or hascoords(path, eval) Or hascoords(past, eval)) Then
                        If maze.GetPixel(eval.x, eval.y).ToArgb = Color.White.ToArgb Then
                            temppath.Add(New coordinates(eval.x, eval.y, i))
                        ElseIf maze.GetPixel(eval.x, eval.y).ToArgb = Color.Red.ToArgb Then
                            i = area 'End While
                        End If
                    End If
                Next

                For Each coord As coordinates In temppath
                    If temppath.Count > 1 Then
                        path.Add(New coordinates(coord.x, coord.y, coord.fn, True))
                    Else
                        path.Add(New coordinates(coord.x, coord.y, coord.fn, False))
                    End If
                Next

                If temppath.Count = 0 Then
                    removebranch(path, past)
                End If
                loc = path(path.Count - 1)
                i += 1
            End While


            'remove unexplored branches
            Dim uforks As New List(Of Integer) 'useless forks
            For q As Integer = 0 To path.Count - 2
                If path(q).isFork = path(q + 1).isFork And path(q).fn = path(q + 1).fn Then
                    uforks.Add(q)
                End If
            Next
            For u As Integer = uforks.Count - 1 To 0 Step -1
                path.RemoveAt(uforks(u))
            Next


            'rasterize
            For Each item1 As coordinates In past
                maze.SetPixel(item1.x, item1.y, Color.FromArgb(255, 0, 255))
            Next
            For Each item1 As coordinates In path
                maze.SetPixel(item1.x, item1.y, Color.FromArgb(0, 128, 0))
                ListBox1.Items.Add(item1.toString)
            Next
            PictureBox1.Image = enlarge(maze, 10)

            'one extra pixel is added after the solver has reached the end. avoid this by finding the end point before starting.


        Catch ex As ArgumentException
            MessageBox.Show(ex.ToString)
        End Try
    End Sub

    Public Function findloc(ByRef img As Bitmap, ByVal col As Color) 'option byval color as color?
        For x = 0 To img.Width - 1
            For y = 0 To img.Height - 1
                If img.GetPixel(x, y).ToArgb = col.ToArgb Then
                    Return New coordinates(x, y)
                End If
            Next
        Next
        Return New coordinates(-1, -1)
    End Function

    Public Sub removebranch(ByRef list As List(Of coordinates), ByRef hist As List(Of coordinates))
        'use when you hit a dead end
        hist.Add(list(list.Count - 1))
        list.RemoveAt(list.Count - 1)
        Dim v As Integer = list.Count - 1
        Do Until list(v).isFork
            hist.Add(list(list.Count - 1))
            list.RemoveAt(list.Count - 1)
            v -= 1
        Loop
    End Sub

    Public Function addc(ByVal a As coordinates, ByVal b As coordinates)
        Return New coordinates(a.x + b.x, a.y + b.y)
    End Function

    Public Function enlarge(ByVal img As Bitmap, ByVal size As Integer) 'ByVal or ByRef? - IMPORTANT DISTINCTION
        Dim big As New Bitmap(img.Width * size, img.Height * size)
        For x = 0 To (big.Width - 1)
            For y = 0 To (big.Height - 1)
                big.SetPixel(x, y, img.GetPixel(Math.Floor(x / size), Math.Floor(y / size)))
            Next
        Next
        Return big
    End Function

    Public Function hascoords(ByVal coordlist As List(Of coordinates), ByVal compareitem As coordinates)
        'because "contains" won't work
        For Each thing As coordinates In coordlist
            If thing.x = compareitem.x And thing.y = compareitem.y Then
                Return True
            End If
        Next
        Return False

        'make this a do until, while, loop until, for each,
        'in order to remove every instance, in case the given list has a coordinate listed twice (or more)
    End Function

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim open As New OpenFileDialog
        open.DefaultExt = ".png"
        open.InitialDirectory = "C:\Users\wolf\Desktop"
        Try
            open.ShowDialog()
            TextBox1.Text = open.FileName
        Catch ex As Exception
            MsgBox("error")
        End Try
    End Sub

End Class

Public Class coordinates
    Public Property x As Integer
    Public Property y As Integer
    Public Property fn As Integer 'direction
    Public Property isFork As Integer
    Public Sub New(ByVal setx As Integer, ByVal sety As Integer, Optional ByVal forknumber As Integer = -1, Optional ByVal _isFork As Boolean = False)
        'which member of compass() it followed from the previous pixel
        x = setx
        y = sety
        fn = forknumber
        isFork = _isFork
    End Sub
    Public Overrides Function toString() As String
        Return x & ", " & y
    End Function
    Public Function outofbounds(ByRef img As Bitmap) As Boolean
        Dim h As Integer = img.Height
        Dim w As Integer = img.Width
        If x < 0 Or x > w - 1 Or y < 0 Or y > w - 1 Then
            Return True
        Else
            Return False
        End If
    End Function

End Class


'TODO: remove branches that still show up in the rasterization
'TODO: optimize for shortest amount of moves/glances (by shuffling compass directions, looking ahead in the map, expecting that the maze will generaly move down and to the right, etc.)
