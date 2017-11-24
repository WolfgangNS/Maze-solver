Public Class Form1

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            ' Retrieve the image.
            Dim image1 = New Bitmap("C:\Users\wolf\Desktop\map.png", True)

            PictureBox1.Image = enlarge(image1, 5)


            Dim compass As New List(Of coordinates)
            compass.Add(New coordinates(0, 1))
            compass.Add(New coordinates(-1, 0))
            compass.Add(New coordinates(1, 0))
            compass.Add(New coordinates(0, -1))

            Dim cpos As New coordinates(0, 0) 'current position
            Dim pathway As New List(Of coordinates)
            Dim forks As New List(Of coordinates)

            'just testing this out
            If True Then
                Dim newlist As New List(Of coordinates)
                newlist.Add(New coordinates(1, 1))
                newlist.Add(New coordinates(2, 1))
                newlist.Add(New coordinates(3, 1))
                newlist.Add(New coordinates(4, 1, True))
                newlist.Add(New coordinates(5, 1, False))
                newlist.Add(New coordinates(6, 1, False))
                newlist.Add(New coordinates(7, 1, False))

                Dim j As Integer = newlist.Count - 1
                Do Until newlist(j).isFork = True
                    newlist.RemoveAt(j)
                    j -= 1
                Loop

                    For Each item As coordinates In newlist
                        ListBox1.Items.Add(item.toString)
                    Next
                    Exit Sub
            End If


            'FIND STARTING POSITION.
            For x = 0 To image1.Width - 1
                For y = 0 To image1.Height - 1
                    If image1.GetPixel(x, y).ToArgb = Color.Lime.ToArgb Then
                        cpos = New coordinates(x, y)
                        pathway.Add(cpos)
                        GoTo startfound 'exit both for loops
                    End If
                Next
            Next
startfound:
            'TODO: make this a subroutine
            'TODO: get contains method to work
            'T O D O : ADD NEW PARAMETER TO COORDINATES INDICATING BRANCH NUMBER

            forks.Add(New coordinates(-1, -1, 0))

            Dim i As Integer = 0
            While i < 1000
                i += 1
                Dim numforks As Integer = 0
                For d As Integer = 0 To 3
                    Dim dir As coordinates = compass(d)
                    Dim eval As coordinates = New coordinates(dir.x + cpos.x, dir.y + cpos.y)
                    If Not (eval.x < 0 Or eval.y < 0 Or contains2(pathway, eval) Or contains2(forks, eval)) Then
                        If image1.GetPixel(eval.x, eval.y).ToArgb = Color.White.ToArgb Then
                            forks.Add(New coordinates(eval.x, eval.y))
                            numforks += 1
                        ElseIf image1.GetPixel(eval.x, eval.y).ToArgb = Color.Red.ToArgb Then
                            i = 1000 'End While
                            'THIS NEEDS SOMETHING ADDED, I FORGOT
                        End If
                    End If
                Next

                'TODO: clear lists
                'TODO: compare lists to make sure you're not walking in circles


                'TODO: collapse these into one or two cases

                Select Case numforks
                    Case 0 'dead end
                        cpos = New coordinates(forks(forks.Count - 1).x, forks(forks.Count - 1).y) 'what value for b? 'TODO: combine these two into a subroutine
                        pathway.Add(cpos)
                        forks.RemoveAt(forks.Count - 1)
                    Case 1
                        'DO NOT ADD A FORK IF THERE IS ONLY ONE DIRECTION TO GO FORWARD
                        If numforks > 1 Then

                        End If
                        cpos = forks(forks.Count - 1)
                        pathway.Add(cpos)
                        forks.RemoveAt(forks.Count - 1)
                End Select
            End While



            For Each item1 As coordinates In pathway
                image1.SetPixel(item1.x, item1.y, Color.FromArgb(0, 255, 0))
            Next

            PictureBox1.Image = enlarge(image1, 10)
            MsgBox("Done!")

            'TODO: make a loading bar
            'TODO: write a list of coordinates



        Catch ex As ArgumentException
            MessageBox.Show(ex.ToString)
        End Try
    End Sub

    Public Sub lastbranch(ByRef blist As List(Of coordinates))
        For i = (blist.Count - 1) To 0 Step -1 'is list.count static here?
            Do Until blist(i).isFork = True
                blist.RemoveAt(i)
            Loop
        Next
    End Sub


    Public Function add(ByVal a As coordinates, ByVal b As coordinates)
        Return New coordinates(a.x + b.x, a.y + b.y)
    End Function

    Public Function enlarge(ByVal asdf As Bitmap, ByVal size As Integer) 'ByVal or ByRef? - IMPORTANT DISTINCTION
        Dim ghi As New Bitmap(asdf.Width * size, asdf.Height * size)
        For p = 0 To (ghi.Width - 1)
            For q = 0 To (ghi.Height - 1)
                ghi.SetPixel(p, q, asdf.GetPixel(Math.Floor(p / size), Math.Floor(q / size)))
            Next
        Next
        Return ghi
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

    ' '''<summary>removes duplicate entries from list1 in list2</summary>
    ' ''' could be a subroutine of a new "list" class
    'Public Sub removeduplicates(ByVal list1 As List(Of coordinates), ByRef list2 As List(Of coordinates))
    '    Dim k As Integer = 0
    '    For Each e As coordinates In list2
    '        For Each f As coordinates In list1
    '            If f.x = e.x And f.y = e.y Then
    '                list1.Remove(f)
    '            End If
    '        Next
    '    Next
    'End Sub

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