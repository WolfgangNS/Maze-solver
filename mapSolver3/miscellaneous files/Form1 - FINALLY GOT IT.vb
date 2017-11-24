Public Class Form1

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'Try
        ' Retrieve the image.
        Dim image1 = New Bitmap("C:\Users\wolf\Desktop\map.png", True)

        PictureBox1.Image = enlarge(image1, 10)


        Dim compass As New List(Of coordinates)
        compass.Add(New coordinates(0, 1))
        compass.Add(New coordinates(-1, 0))
        compass.Add(New coordinates(1, 0))
        compass.Add(New coordinates(0, -1))

        Dim loc As New coordinates(0, 0) 'current position
        Dim path As New List(Of coordinates)
        Dim forks As New List(Of coordinates)

        'FIND STARTING POSITION.
        For x = 0 To image1.Width - 1
            For y = 0 To image1.Height - 1
                If image1.GetPixel(x, y).ToArgb = Color.Lime.ToArgb Then
                    loc = New coordinates(x, y, False)
                    path.Add(loc)
                    GoTo startfound 'exit both for loops
                End If
            Next
        Next
startfound:
        'TODO: make this a subroutine
        'TODO: get "contains" method to work properly


        Dim history As New List(Of coordinates)

        Dim i As Integer = 0
        While i < 1000



            Dim temppath As New List(Of coordinates)
            For Each Dir As coordinates In compass 'check four directions
                Dim eval As coordinates = addc(Dir, loc)
                If Not (eval.x < 0 Or eval.y < 0 Or contains2(path, eval) Or contains2(history, eval)) Then
                    If image1.GetPixel(eval.x, eval.y).ToArgb = Color.White.ToArgb Then
                        temppath.Add(New coordinates(eval.x, eval.y))
                    ElseIf image1.GetPixel(eval.x, eval.y).ToArgb = Color.Red.ToArgb Then
                        i = 1000 'End While
                    End If
                End If
            Next

            'copy temppath coordinates to path
            'MsgBox(temppath.Count)
            For Each coord As coordinates In temppath
                If temppath.Count > 1 Then
                    path.Add(New coordinates(coord.x, coord.y, True))
                Else
                    path.Add(New coordinates(coord.x, coord.y, False))
                End If
            Next

            If temppath.Count = 0 Then
                'dead end or crossroads
                'either way, go to the last branch
                removebranch(path, history)
            End If

            loc = path(path.Count - 1)

            'MsgBox(i)

            i += 1
        End While



        For Each item1 As coordinates In path
            If item1.isFork Then
                image1.SetPixel(item1.x, item1.y, Color.FromArgb(0, 255, 0))
            Else
                image1.SetPixel(item1.x, item1.y, Color.FromArgb(0, 128, 0))

            End If
            ListBox1.Items.Add(item1.toString)
        Next
        For Each item1 As coordinates In history
            image1.SetPixel(item1.x, item1.y, Color.FromArgb(255, 0, 255))
        Next
        PictureBox1.Image = enlarge(image1, 10)

        'Catch ex As ArgumentException
        '    MessageBox.Show(ex.ToString)
        'End Try
    End Sub

    Public Sub removebranch(ByRef list As List(Of coordinates), ByRef hist As List(Of coordinates))
        'NOTE: INPUT LIST MUST INCLUDE A FORK (PREFERABLY AT THE START), OR ELSE THIS WILL BREAK
        'removebranch sub checks for isfork patterns like: "+----" at the end of "--+++-+----++----"
        'or even "+" in "----++" if it's a dead end

        hist.Add(list(list.Count - 1))
        list.RemoveAt(list.Count - 1)
        Dim v As Integer = list.Count - 1
        Do Until list(v).isFork
            hist.Add(list(list.Count - 1))
            list.RemoveAt(list.Count - 1)
            v -= 1
        Loop


        'If Not list(list.Count - 1).isFork Then
        '    Dim v As Integer = list.Count - 1
        '    While (Not list(v).isFork) And v > 0
        '        v -= 1
        '    End While
        '    Do Until v > (list.Count - 1)
        '        list.RemoveAt(list.Count - 1)
        '    Loop
        'End If
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

    Public Function contains2(ByVal coordlist As List(Of coordinates), ByVal compareitem As coordinates) 'I really need to think of better names
        For Each thing As coordinates In coordlist
            If thing.x = compareitem.x And thing.y = compareitem.y Then
                Return True
            End If
        Next
        Return False

        'make this a do until, while, loop until, for each,
        'in order to remove every instance
    End Function
End Class

Public Class coordinates
    Public Property x As Integer
    Public Property y As Integer
    Public Property isFork As Integer
    Public Sub New(ByVal setx As Integer, ByVal sety As Integer, Optional ByVal _isFork As Boolean = False)
        x = setx
        y = sety
        isFork = _isFork
    End Sub
    Public Overrides Function toString() As String
        Return x & ", " & y
    End Function
End Class